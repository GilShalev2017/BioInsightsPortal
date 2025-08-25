using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioInsightsPortal.Data.Entities
{
    public class Gene
    {
        public int Id { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Chromosome { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Association> Associations { get; set; } = new List<Association>();
        public ICollection<Drug> Drugs { get; set; } = new List<Drug>();
    }
}
