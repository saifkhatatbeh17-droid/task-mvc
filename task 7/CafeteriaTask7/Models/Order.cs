using System.ComponentModel.DataAnnotations.Schema;
namespace CafeteriaTask7.Models;
public class Order
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = "";
    public DateTime OrderDate { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = "";
    public int EmployeeId { get; set; }
    public Employee? Employee { get; set; }
}
