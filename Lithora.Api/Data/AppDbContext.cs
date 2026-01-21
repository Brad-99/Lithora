using Microsoft.EntityFrameworkCore;
using Lithora.Api.Models;

namespace Lithora.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Inspection> Inspections => Set<Inspection>();
    public DbSet<Defect> Defects => Set<Defect>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Inspection>()
            .HasMany(i => i.Defects)
            .WithOne(d => d.Inspection)
            .HasForeignKey(d => d.InspectionId)
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }
}
