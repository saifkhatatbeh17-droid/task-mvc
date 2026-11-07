using System;
using System.Collections.Generic;

namespace CafeteriaDatabaseFirst.Models;

public partial class PurchaseOrderItems
{
    public int Id { get; set; }

    public int PurchaseOrderId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public virtual Products Product { get; set; } = null!;

    public virtual PurchaseOrders PurchaseOrder { get; set; } = null!;
}
