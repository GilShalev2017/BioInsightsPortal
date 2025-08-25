using Microsoft.AspNetCore.Mvc;

namespace BioInsightsPortal.Api.Controllers
{
    // Defines this class as an API controller and sets the base route for its endpoints.
    [ApiController]
    [Route("api/[controller]")]
    public class GenesController : ControllerBase
    {
        // Defines the data model for a Gene.
        public record Gene(int Id, string Symbol, string Name, string Chromosome, string Description);

        // A private, static list to hold mock gene data.
        private static readonly List<Gene> mockGenes = new List<Gene>
        {
            new Gene(1, "TP53", "Tumor protein p53", "17p13.1", "A tumor suppressor gene that plays a critical role in preventing cancer formation."),
            new Gene(2, "BRCA1", "Breast cancer type 1 susceptibility protein", "17q21.31", "A human tumor suppressor gene responsible for repairing DNA."),
            new Gene(3, "EGFR", "Epidermal growth factor receptor", "7p11.2", "A protein that helps cells grow and divide. Mutations can lead to uncontrolled cell growth and cancer.")
        };

        // HTTP GET endpoint to retrieve all genes.
        [HttpGet]
        public IActionResult Get()
        {
            // Returns the mock gene data with an HTTP 200 OK status.
            return Ok(mockGenes);
        }

        // HTTP GET endpoint to retrieve a single gene by its ID.
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            // Searches for the gene with the matching ID.
            var gene = mockGenes.FirstOrDefault(g => g.Id == id);
            if (gene == null)
            {
                // Returns a 404 Not Found if the gene is not found.
                return NotFound();
            }
            // Returns the found gene.
            return Ok(gene);
        }
    }
}
