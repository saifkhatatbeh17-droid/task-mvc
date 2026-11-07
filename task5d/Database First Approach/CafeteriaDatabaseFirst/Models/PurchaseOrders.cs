using System;
using System.Collections.Generic;

namespace CafeteriaDatabaseFirst.Models;

public partial class PurchaseOrders
{
    public int Id { get; set; }

    public DateTime OrderDate { get; set; }

    public string Status { get; set; } = null!;

    public int SupplierId { get; set; }

    public virtual ICollection<PurchaseOrderItems> PurchaseOrderItems { get; set; } = new List<PurchaseOrderItems>();

    public virtual Suppliers Supplier { get; set; } = null!;
}
