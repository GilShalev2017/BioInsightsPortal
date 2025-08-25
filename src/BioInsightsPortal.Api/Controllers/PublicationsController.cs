// PublicationsController.cs
// This controller handles API requests related to publication data.

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace BioInsightsPortal.Api.Controllers
{
    // Defines this class as an API controller and sets the base route for its endpoints.
    [ApiController]
    [Route("api/[controller]")]
    public class PublicationsController : ControllerBase
    {
        // Defines the data model for a Publication.
        public record Publication(int Id, string PubMedId, string Title, string? Abstract, int? Year);

        // A private, static list to hold mock publication data.
        private static readonly List<Publication> mockPublications = new List<Publication>
        {
            new Publication(1, "12345", "A Novel Gene Therapy for TP53 Mutations", "This study explores a new method...", 2021),
            new Publication(2, "67890", "BRCA1 and its role in DNA Repair", "We investigate the critical function of BRCA1...", 2019),
            new Publication(3, "11223", "EGFR Inhibitors for Non-Small Cell Lung Cancer", "An overview of treatment strategies...", 2023),
        };

        // HTTP GET endpoint to retrieve all publications.
        [HttpGet]
        public IActionResult Get()
        {
            // Returns the mock publication data with an HTTP 200 OK status.
            return Ok(mockPublications);
        }

        // HTTP GET endpoint to retrieve a single publication by its ID.
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            // Searches for the publication with the matching ID.
            var publication = mockPublications.FirstOrDefault(p => p.Id == id);
            if (publication == null)
            {
                // Returns a 404 Not Found if the publication is not found.
                return NotFound();
            }
            // Returns the found publication.
            return Ok(publication);
        }
    }
}
