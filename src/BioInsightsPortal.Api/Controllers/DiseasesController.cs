using Microsoft.AspNetCore.Mvc;

namespace BioInsightsPortal.Api.Controllers
{
    // Defines this class as an API controller and sets the base route for its endpoints.
    [ApiController]
    [Route("api/[controller]")]
    public class DiseasesController : ControllerBase
    {
        // Defines the data model for a Disease.
        public record Disease(int Id, string Name, string Category, string Description);

        // A private, static list to hold mock disease data.
        private static readonly List<Disease> mockDiseases = new List<Disease>
        {
            new Disease(101, "Breast Cancer", "Oncology", "Cancer that forms in the cells of the breasts."),
            new Disease(102, "Alzheimer's Disease", "Neurology", "A progressive neurodegenerative disease that affects memory, thinking, and behavior."),
            new Disease(103, "Cystic Fibrosis", "Pulmonology", "A hereditary disease that affects the lungs and digestive system.")
        };

        // HTTP GET endpoint to retrieve all diseases.
        [HttpGet]
        public IActionResult Get()
        {
            // Returns the mock disease data with an HTTP 200 OK status.
            return Ok(mockDiseases);
        }

        // HTTP GET endpoint to retrieve a single disease by its ID.
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            // Searches for the disease with the matching ID.
            var disease = mockDiseases.FirstOrDefault(d => d.Id == id);
            if (disease == null)
            {
                // Returns a 404 Not Found if the disease is not found.
                return NotFound();
            }
            // Returns the found disease.
            return Ok(disease);
        }
    }
}
