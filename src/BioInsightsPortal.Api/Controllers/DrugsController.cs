// DrugsController.cs
// This controller handles API requests related to drug data.

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace BioInsightsPortal.Api.Controllers
{
    // Defines this class as an API controller and sets the base route for its endpoints.
    [ApiController]
    [Route("api/[controller]")]
    public class DrugsController : ControllerBase
    {
        // Defines the data model for a Drug.
        public record Drug(int Id, string Name, int? TargetGeneId, string? Indication);

        // A private, static list to hold mock drug data.
        private static readonly List<Drug> mockDrugs = new List<Drug>
        {
            new Drug(1, "Aspirin", null, "Pain relief, fever reduction, anti-inflammatory"),
            new Drug(2, "Herceptin", 2, "Breast cancer"), // Targets BRCA1
            new Drug(3, "Gleevec", null, "Chronic myeloid leukemia"),
            new Drug(4, "Advil", null, "Pain relief, fever reduction"),
        };

        // HTTP GET endpoint to retrieve all drugs.
        [HttpGet]
        public IActionResult Get()
        {
            // Returns the mock drug data with an HTTP 200 OK status.
            return Ok(mockDrugs);
        }

        // HTTP GET endpoint to retrieve a single drug by its ID.
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            // Searches for the drug with the matching ID.
            var drug = mockDrugs.FirstOrDefault(d => d.Id == id);
            if (drug == null)
            {
                // Returns a 404 Not Found if the drug is not found.
                return NotFound();
            }
            // Returns the found drug.
            return Ok(drug);
        }
    }
}
