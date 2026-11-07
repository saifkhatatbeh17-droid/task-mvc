using AcademyGraduation.Models;
using Microsoft.EntityFrameworkCore;

namespace AcademyGraduation.Data;

public class GraduationContext : DbContext
{
    public GraduationContext(DbContextOptions<GraduationContext> options) : base(options) { }
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Offer> Offers => Set<Offer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Day5Task");
        modelBuilder.Entity<Category>().HasMany(c => c.Suppliers)
            .WithOne(s => s.Category).HasForeignKey(s => s.CategoryId);
        modelBuilder.Entity<Supplier>().HasOne(s => s.Offer)
            .WithOne(o => o.Supplier).HasForeignKey<Offer>(o => o.SupplierId);
        modelBuilder.Entity<Offer>().Property(o => o.UnitPrice).HasPrecision(10, 2);
    }
}

