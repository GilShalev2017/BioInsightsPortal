using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioInsightsPortal.Data.Entities
{
    public class Drug
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? TargetGeneId { get; set; }
        public string? Indication { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Gene? TargetGene { get; set; }
    }
}
