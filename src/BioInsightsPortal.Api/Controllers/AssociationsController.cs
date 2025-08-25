// AssociationsController.cs
// This controller handles API requests related to gene-disease association data.

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BioInsightsPortal.Api.Controllers
{
    // Defines this class as an API controller and sets the base route for its endpoints.
    [ApiController]
    [Route("api/[controller]")]
    public class AssociationsController : ControllerBase
    {
        // Defines the data model for an Association.
        public record Association(int GeneId, int DiseaseId, double EvidenceScore, string Source);

        // A private, static list to hold mock association data.
        private static readonly List<Association> mockAssociations = new List<Association>
        {
            new Association(1, 101, 0.95, "PubMed ID: 12345"),
            new Association(2, 101, 0.99, "PubMed ID: 67890"),
            new Association(3, 101, 0.88, "PubMed ID: 11223"),
            new Association(1, 102, 0.70, "Predictive Model")
        };

        // HTTP GET endpoint to retrieve all gene-disease associations.
        [HttpGet]
        public IActionResult Get()
        {
            // Returns the mock association data with an HTTP 200 OK status.
            return Ok(mockAssociations);
        }
    }
}
