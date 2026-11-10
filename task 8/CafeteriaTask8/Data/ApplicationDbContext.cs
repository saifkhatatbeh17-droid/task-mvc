using CafeteriaTask8.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CafeteriaTask8.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<SupplyRequest> SupplyRequests => Set<SupplyRequest>();
    public DbSet<RequestItem> RequestItems => Set<RequestItem>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Product>().Property(p => p.Price).HasColumnType("decimal(18,2)");
        builder.Entity<SupplyRequest>().Property(p => p.TotalPrice).HasColumnType("decimal(18,2)");
        builder.Entity<RequestItem>().Property(p => p.Price).HasColumnType("decimal(18,2)");

        builder.Entity<SupplyRequest>()
            .HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
