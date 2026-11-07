namespace AcademyGraduation.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public List<Supplier> Suppliers { get; set; } = new List<Supplier>();
}

public class Supplier
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public Offer Offer { get; set; } = null!;
}

public class Offer
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public int SupplierId { get; set; }
    public Supplier Supplier { get; set; } = null!;
}
