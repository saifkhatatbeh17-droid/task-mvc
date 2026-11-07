using System.ComponentModel.DataAnnotations;
namespace CafeteriaCodeFirst.Models;

public class PurchaseOrder
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    [Required, StringLength(20)]
    public string Status { get; set; } = "Draft";
    public int SupplierId { get; set; }
    public Supplier Supplier { get; set; } = null!;
    public List<PurchaseOrderItem> PurchaseOrderItems { get; set; } = new List<PurchaseOrderItem>();
}
