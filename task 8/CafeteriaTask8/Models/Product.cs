using System.ComponentModel.DataAnnotations;

namespace CafeteriaTask8.Models;

public class Product
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = "";

    public string Description { get; set; } = "";

    [Range(0.01, 100000)]
    public decimal Price { get; set; }

    [Range(0, 100000)]
    public int Quantity { get; set; }
}
