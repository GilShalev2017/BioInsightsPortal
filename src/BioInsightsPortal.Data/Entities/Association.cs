using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioInsightsPortal.Data.Entities
{
    public class Association
    {
        public int Id { get; set; }
        public int GeneId { get; set; }
        public int DiseaseId { get; set; }
        public double EvidenceScore { get; set; }
        public string? Source { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Gene Gene { get; set; } = null!;
        public Disease Disease { get; set; } = null!;
    }
}
