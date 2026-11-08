using CafeteriaCodeFirst.Data;
using CafeteriaCodeFirst.Models;
using Microsoft.AspNetCore.Mvc;

namespace CafeteriaCodeFirst.Controllers;

public class SupplierController : Controller
{
    private readonly ApplicationDbContext db;

    public SupplierController(ApplicationDbContext context)
    {
        db = context;
    }

    public IActionResult Index()
    {
        return View(db.Suppliers.ToList());
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("Name,Phone,Email")] Supplier supplier)
    {
        if (!ModelState.IsValid)
        {
            return View(supplier);
        }

        db.Suppliers.Add(supplier);
        db.SaveChanges();
        return RedirectToAction("Index");
    }
}
