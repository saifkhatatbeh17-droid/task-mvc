using System;
using System.Collections.Generic;

namespace CafeteriaDatabaseFirst.Models;

public partial class Products
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public int CategoryId { get; set; }

    public int SupplierId { get; set; }

    public virtual Categories Category { get; set; } = null!;

    public virtual ICollection<PurchaseOrderItems> PurchaseOrderItems { get; set; } = new List<PurchaseOrderItems>();

    public virtual Suppliers Supplier { get; set; } = null!;
}
