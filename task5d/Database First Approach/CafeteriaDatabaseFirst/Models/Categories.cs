using System;
using System.Collections.Generic;

namespace CafeteriaDatabaseFirst.Models;

public partial class Categories
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Products> Products { get; set; } = new List<Products>();
}
