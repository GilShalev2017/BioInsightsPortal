// BioInsightsService.cs
// This service handles fetching data from your API endpoints.

using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BioInsightsPortal.Web.Data
{
    public class BioInsightsService
    {
        private readonly HttpClient _httpClient;

        public BioInsightsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Define data models to match your API records
        public record Gene(int Id, string Symbol, string Name, string Chromosome, string Description);
        public record Disease(int Id, string Name, string Category, string Description);
        public record Association(int GeneId, int DiseaseId, double EvidenceScore, string Source);
        public record Drug(int Id, string Name, int? TargetGeneId, string? Indication);
        public record Publication(int Id, string PubMedId, string Title, string? Abstract, int? Year);

        // API methods
        public async Task<List<Gene>?> GetGenesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Gene>>("api/genes");
        }

        public async Task<List<Disease>?> GetDiseasesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Disease>>("api/diseases");
        }

        public async Task<List<Association>?> GetAssociationsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Association>>("api/associations");
        }

        public async Task<List<Drug>?> GetDrugsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Drug>>("api/drugs");
        }

        public async Task<List<Publication>?> GetPublicationsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Publication>>("api/publications");
        }
    }
}
