using CafeteriaCodeFirst.Models;
using Microsoft.EntityFrameworkCore;
namespace CafeteriaCodeFirst.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
    public DbSet<PurchaseOrderItem> PurchaseOrderItems => Set<PurchaseOrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Task6");
        modelBuilder.Entity<Product>().HasOne(p => p.Category).WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Product>().HasOne(p => p.Supplier).WithMany(s => s.Products)
            .HasForeignKey(p => p.SupplierId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<PurchaseOrder>().HasOne(o => o.Supplier).WithMany(s => s.PurchaseOrders)
            .HasForeignKey(o => o.SupplierId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<PurchaseOrder>().Property(o => o.OrderDate).HasDefaultValueSql("SYSUTCDATETIME()");
        modelBuilder.Entity<PurchaseOrderItem>().HasOne(i => i.PurchaseOrder)
            .WithMany(o => o.PurchaseOrderItems).HasForeignKey(i => i.PurchaseOrderId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<PurchaseOrderItem>().HasOne(i => i.Product)
            .WithMany(p => p.PurchaseOrderItems).HasForeignKey(i => i.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<PurchaseOrderItem>().HasIndex(i => new { i.PurchaseOrderId, i.ProductId }).IsUnique();
        modelBuilder.Entity<Product>().ToTable("Products", t =>
        {
            t.HasCheckConstraint("CK_Task6_ProductPrice", "[Price] > 0");
            t.HasCheckConstraint("CK_Task6_ProductQuantity", "[Quantity] >= 0");
        });
        modelBuilder.Entity<PurchaseOrder>().ToTable("PurchaseOrders", t =>
            t.HasCheckConstraint("CK_Task6_OrderStatus", "[Status] IN ('Draft','Received')"));
        modelBuilder.Entity<PurchaseOrderItem>().ToTable("PurchaseOrderItems", t =>
        {
            t.HasCheckConstraint("CK_Task6_ItemQuantity", "[Quantity] > 0");
            t.HasCheckConstraint("CK_Task6_ItemPrice", "[UnitPrice] > 0");
        });
    }
}

