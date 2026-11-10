namespace CafeteriaTask8.Models;

public class RequestLine
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = "";
    public decimal Price { get; set; }
    public int Available { get; set; }
    public int Quantity { get; set; }
}

public class RequestFormViewModel
{
    public int Id { get; set; }
    public string Status { get; set; } = RequestStatus.Pending;
    public bool CanChangeItems { get; set; }
    public bool CanChangeStatus { get; set; }
    public List<RequestLine> Lines { get; set; } = new();
}
