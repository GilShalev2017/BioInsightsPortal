using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using BioInsightsPortal.Data.Context;
using Microsoft.EntityFrameworkCore;
using Nest;

public class SyncWorker : BackgroundService
{
    private readonly ILogger<SyncWorker> _logger;
    private readonly BioInsightsDbContext _db;
    private readonly IElasticClient _es;
    private readonly string _genesIndex;
    private readonly string _diseasesIndex;
    private readonly int _batchSize;
    private readonly int _pollSeconds;

    public SyncWorker(
        ILogger<SyncWorker> logger,
        BioInsightsDbContext db,
        IElasticClient es,
        IConfiguration config)
    {
        _logger = logger;
        _db = db;
        _es = es;
        _genesIndex = config["Elasticsearch:GenesIndex"] ?? "genes";
        _diseasesIndex = config["Elasticsearch:DiseasesIndex"] ?? "diseases";
        _batchSize = int.TryParse(config["Elasticsearch:BatchSize"], out var b) ? b : 500;
        _pollSeconds = int.TryParse(config["Elasticsearch:PollSeconds"], out var p) ? p : 15;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("SyncWorker started.");

        // Ensure indexes exist (idempotent)
        await EnsureIndexesAsync(stoppingToken);

        // Simple high-water-mark using CreatedAt (could add UpdatedAt for real-world)
        DateTime lastSyncedGenes = DateTime.MinValue;
        DateTime lastSyncedDiseases = DateTime.MinValue;

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                lastSyncedGenes = await SyncGenesAsync(lastSyncedGenes, stoppingToken);
                lastSyncedDiseases = await SyncDiseasesAsync(lastSyncedDiseases, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sync iteration failed.");
            }

            await Task.Delay(TimeSpan.FromSeconds(_pollSeconds), stoppingToken);
        }
    }

    private async Task EnsureIndexesAsync(CancellationToken ct)
    {
        var gExists = await _es.Indices.ExistsAsync(_genesIndex, ct);
        if (!gExists.Exists)
        {
            await _es.Indices.CreateAsync(_genesIndex, c => c
                .Map<GeneDoc>(m => m.AutoMap()), ct);
        }

        var dExists = await _es.Indices.ExistsAsync(_diseasesIndex, ct);
        if (!dExists.Exists)
        {
            await _es.Indices.CreateAsync(_diseasesIndex, c => c
                .Map<DiseaseDoc>(m => m.AutoMap()), ct);
        }
    }

    private async Task<DateTime> SyncGenesAsync(DateTime since, CancellationToken ct)
    {
        var page = await _db.Genes
            .Where(g => g.CreatedAt > since)
            .OrderBy(g => g.CreatedAt)
            .Take(_batchSize)
            .Select(g => new GeneDoc
            {
                Id = g.Id,
                Symbol = g.Symbol,
                Name = g.Name,
                Chromosome = g.Chromosome,
                Description = g.Description
            })
            .ToListAsync(ct);

        if (page.Count == 0) return since;

        var bulk = new BulkDescriptor();
        foreach (var doc in page)
        {
            bulk.Index<GeneDoc>(op => op
                .Index(_genesIndex)
                .Id(doc.Id)
                .Document(doc));
        }
        var resp = await _es.BulkAsync(bulk, ct);
        if (resp.Errors) throw new Exception("Bulk gene sync had errors.");

        // New high-water-mark = max CreatedAt in this batch
        var newest = await _db.Genes
            .Where(g => g.CreatedAt > since)
            .OrderByDescending(g => g.CreatedAt)
            .Select(g => g.CreatedAt)
            .FirstAsync(ct);

        return newest;
    }

    private async Task<DateTime> SyncDiseasesAsync(DateTime since, CancellationToken ct)
    {
        var page = await _db.Diseases
            .Where(d => d.CreatedAt > since)
            .OrderBy(d => d.CreatedAt)
            .Take(_batchSize)
            .Select(d => new DiseaseDoc
            {
                Id = d.Id,
                Name = d.Name,
                Category = d.Category,
                Description = d.Description
            })
            .ToListAsync(ct);

        if (page.Count == 0) return since;

        var bulk = new BulkDescriptor();
        foreach (var doc in page)
        {
            bulk.Index<DiseaseDoc>(op => op
                .Index(_diseasesIndex)
                .Id(doc.Id)
                .Document(doc));
        }
        var resp = await _es.BulkAsync(bulk, ct);
        if (resp.Errors) throw new Exception("Bulk disease sync had errors.");

        var newest = await _db.Diseases
            .Where(d => d.CreatedAt > since)
            .OrderByDescending(d => d.CreatedAt)
            .Select(d => d.CreatedAt)
            .FirstAsync(ct);

        return newest;
    }

    // Minimal ES documents (decouple from EF entities)
    private record GeneDoc
    {
        public int Id { get; init; }
        public string Symbol { get; init; } = "";
        public string Name { get; init; } = "";
        public string? Chromosome { get; init; }
        public string? Description { get; init; }
    }

    private record DiseaseDoc
    {
        public int Id { get; init; }
        public string Name { get; init; } = "";
        public string? Category { get; init; }
        public string? Description { get; init; }
    }
}

//For production, switch to an UpdatedAt column and/or a ChangeLog table or SQL CDC. The demo above uses a simple CreatedAt high-water mark.