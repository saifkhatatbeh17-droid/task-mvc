using System.ComponentModel.DataAnnotations;
namespace CafeteriaCodeFirst.Models;

public class Supplier
{
    public int Id { get; set; }
    [Required, StringLength(100)]
    public string Name { get; set; } = "";
    [Required, EmailAddress, StringLength(150)]
    public string Email { get; set; } = "";
    [Required, StringLength(30)]
    public string Phone { get; set; } = "";
    public List<Product> Products { get; set; } = new List<Product>();
    public List<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
}
