using CafeteriaCodeFirst.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace CafeteriaCodeFirst.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext db;
    public HomeController(ApplicationDbContext context)
    {
        db = context;
    }

    public IActionResult Index()
    {
        ViewBag.DatabaseName = db.Database.GetDbConnection().Database;
        ViewBag.Categories = db.Categories.Count();
        ViewBag.Suppliers = db.Suppliers.Count();
        ViewBag.Products = db.Products.Count();
        ViewBag.PurchaseOrders = db.PurchaseOrders.Count();
        ViewBag.PurchaseOrderItems = db.PurchaseOrderItems.Count();
        return View();
    }
}

