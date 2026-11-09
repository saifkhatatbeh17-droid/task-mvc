using CafeteriaTask7.Models;
using Microsoft.EntityFrameworkCore;
namespace CafeteriaTask7.Data;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Order> Orders => Set<Order>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasOne(p => p.Category).WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Product>().HasOne(p => p.Supplier).WithMany(s => s.Products)
            .HasForeignKey(p => p.SupplierId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Order>().HasOne(o => o.Employee).WithMany(e => e.Orders)
            .HasForeignKey(o => o.EmployeeId).OnDelete(DeleteBehavior.Restrict);
    }
}
