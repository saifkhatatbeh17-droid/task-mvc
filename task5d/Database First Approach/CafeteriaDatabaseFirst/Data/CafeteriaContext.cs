using System;
using System.Collections.Generic;
using CafeteriaDatabaseFirst.Models;
using Microsoft.EntityFrameworkCore;

namespace CafeteriaDatabaseFirst.Data;

public partial class CafeteriaContext : DbContext
{
    public CafeteriaContext(DbContextOptions<CafeteriaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categories> Categories { get; set; }

    public virtual DbSet<Products> Products { get; set; }

    public virtual DbSet<PurchaseOrderItems> PurchaseOrderItems { get; set; }

    public virtual DbSet<PurchaseOrders> PurchaseOrders { get; set; }

    public virtual DbSet<Suppliers> Suppliers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categories>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC0774E66440");

            entity.ToTable("Categories", "Task6");

            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Products>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Products__3214EC0749C34583");

            entity.ToTable("Products", "Task6");

            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products__Catego__619B8048");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Products)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products__Suppli__628FA481");
        });

        modelBuilder.Entity<PurchaseOrderItems>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Purchase__3214EC07925F2A04");

            entity.ToTable("PurchaseOrderItems", "Task6");

            entity.HasIndex(e => new { e.PurchaseOrderId, e.ProductId }, "UQ_Task6_OrderProduct").IsUnique();

            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Product).WithMany(p => p.PurchaseOrderItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PurchaseO__Produ__6C190EBB");

            entity.HasOne(d => d.PurchaseOrder).WithMany(p => p.PurchaseOrderItems)
                .HasForeignKey(d => d.PurchaseOrderId)
                .HasConstraintName("FK__PurchaseO__Purch__6B24EA82");
        });

        modelBuilder.Entity<PurchaseOrders>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Purchase__3214EC078D22380B");

            entity.ToTable("PurchaseOrders", "Task6");

            entity.Property(e => e.OrderDate).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.HasOne(d => d.Supplier).WithMany(p => p.PurchaseOrders)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PurchaseO__Suppl__6754599E");
        });

        modelBuilder.Entity<Suppliers>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Supplier__3214EC07ABAA47FF");

            entity.ToTable("Suppliers", "Task6");

            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(30);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
