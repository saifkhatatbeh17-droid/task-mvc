using System;
using System.Collections.Generic;

namespace CafeteriaDatabaseFirst.Models;

public partial class Suppliers
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public virtual ICollection<Products> Products { get; set; } = new List<Products>();

    public virtual ICollection<PurchaseOrders> PurchaseOrders { get; set; } = new List<PurchaseOrders>();
}
