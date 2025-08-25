using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioInsightsPortal.Data.Entities
{
    public class Publication
    {
        public int Id { get; set; }
        public string PubMedId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Abstract { get; set; }
        public int? Year { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
