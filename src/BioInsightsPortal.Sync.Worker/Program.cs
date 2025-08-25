using BioInsightsPortal.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Nest;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((ctx, services) =>
    {
        var cs = ctx.Configuration.GetConnectionString("Default");
        services.AddDbContext<BioInsightsDbContext>(opt =>
            opt.UseSqlServer(cs));

        // Elasticsearch
        var esUri = ctx.Configuration["Elasticsearch:Uri"];
        var settings = new ConnectionSettings(new Uri(esUri!))
            .ThrowExceptions()
            .DisableDirectStreaming(); // easier debugging
        services.AddSingleton<IElasticClient>(new ElasticClient(settings));

        services.AddHostedService<SyncWorker>();
    })
    .Build();

await host.RunAsync();
