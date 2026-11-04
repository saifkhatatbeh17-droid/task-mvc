using Microsoft.AspNetCore.Mvc;

namespace UniversityCafeteria.Controllers;

public class SupplyController : Controller
{
    public IActionResult Index()
    {
        string[] categories = { "Drinks", "Food", "Snacks", "Hot Drinks", "Cold Drinks" };
        ViewBag.Categories = categories;
        return View();
    }

    public IActionResult Products()
    {
        ViewData["PageTitle"] = "Cafeteria Products";
        ViewData["ManagerName"] = "Ahmad Ali";
        ViewData["NumberOfProducts"] = 5;
        string[] products = { "Water", "Orange Juice", "Coffee", "Sandwich", "Chocolate" };
        ViewData["Products"] = products;
        return View();
    }

    public IActionResult Suppliers()
    {
        ViewBag.PageTitle = "Our Suppliers";
        ViewBag.NumberOfSuppliers = 3;
        string[] suppliers = { "Fresh Food Company", "Happy Drinks Company", "Daily Supplies Company" };
        ViewBag.Suppliers = suppliers;
        return View();
    }

    [HttpGet]
    public IActionResult CreateRequest()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult SubmitRequest()
    {
        string employeeName = Request.Form["EmployeeName"].ToString().Trim();
        string productName = Request.Form["ProductName"].ToString();
        string quantityText = Request.Form["Quantity"].ToString();
        string requiredDateText = Request.Form["RequiredDate"].ToString();
        string[] products = { "Water", "Orange Juice", "Coffee", "Sandwich", "Chocolate" };

        if (string.IsNullOrWhiteSpace(employeeName))
        {
            ViewBag.ErrorMessage = "Employee name is required.";
            return View();
        }

        if (!Array.Exists(products, product => product == productName))
        {
            ViewBag.ErrorMessage = "Please select a valid product.";
            return View();
        }

        if (!int.TryParse(quantityText, out int quantity) || quantity <= 0)
        {
            ViewBag.ErrorMessage = "Quantity must be a positive whole number.";
            return View();
        }

        if (!DateTime.TryParseExact(requiredDateText, "yyyy-MM-dd",
            System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.None, out DateTime requiredDate))
        {
            ViewBag.ErrorMessage = "Please select a valid required date.";
            return View();
        }

        TempData["Message"] = $"Thank you, {employeeName}. Your request for {quantity} units of {productName}, required on {requiredDate:yyyy-MM-dd}, was submitted successfully.";
        return RedirectToAction("About");
    }

    public IActionResult About()
    {
        return View();
    }

    [HttpGet]
    public IActionResult CheckQuantity(int quantity)
    {
        if (!ModelState.IsValid || quantity <= 0)
        {
            ViewBag.Classification = "Please enter a positive whole number.";
        }
        else if (quantity < 20)
        {
            ViewBag.Classification = "Small Order";
        }
        else if (quantity <= 50)
        {
            ViewBag.Classification = "Medium Order";
        }
        else
        {
            ViewBag.Classification = "Large Order";
        }

        ViewBag.Quantity = quantity;
        return View();
    }
}
