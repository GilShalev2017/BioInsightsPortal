using BioInsightsPortal.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioInsightsPortal.Data.Context
{
    public class BioInsightsDbContext : DbContext
    {
        public BioInsightsDbContext(DbContextOptions<BioInsightsDbContext> options) : base(options) { }

        public DbSet<Gene> Genes { get; set; }
        public DbSet<Disease> Diseases { get; set; }
        public DbSet<Drug> Drugs { get; set; }
        public DbSet<Association> Associations { get; set; }
        public DbSet<Publication> Publications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Association>()
                .HasOne(a => a.Gene)
                .WithMany(g => g.Associations)
                .HasForeignKey(a => a.GeneId);

            modelBuilder.Entity<Association>()
                .HasOne(a => a.Disease)
                .WithMany(d => d.Associations)
                .HasForeignKey(a => a.DiseaseId);

            modelBuilder.Entity<Drug>()
                .HasOne(d => d.TargetGene)
                .WithMany(g => g.Drugs)
                .HasForeignKey(d => d.TargetGeneId);
        }
    }
}
