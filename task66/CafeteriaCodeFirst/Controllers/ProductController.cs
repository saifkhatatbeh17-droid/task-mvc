using CafeteriaCodeFirst.Data;
using CafeteriaCodeFirst.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CafeteriaCodeFirst.Controllers;

public class ProductController : Controller
{
    private readonly ApplicationDbContext db;

    public ProductController(ApplicationDbContext context)
    {
        db = context;
    }

    public IActionResult Index()
    {
        var products = db.Products.Include(p => p.Category)
            .Include(p => p.Supplier).ToList();
        return View(products);
    }

    [HttpGet]
    public IActionResult Create()
    {
        LoadLists();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("Name,Price,Quantity,CategoryId,SupplierId")] Product product)
    {
        ModelState.Remove(nameof(Product.Category));
        ModelState.Remove(nameof(Product.Supplier));

        if (product.Price <= 0)
            ModelState.AddModelError("Price", "Price must be greater than zero.");
        if (product.Quantity < 0)
            ModelState.AddModelError("Quantity", "Quantity cannot be negative.");
        if (!db.Categories.Any(c => c.Id == product.CategoryId))
            ModelState.AddModelError("CategoryId", "Select a valid category.");
        if (!db.Suppliers.Any(s => s.Id == product.SupplierId))
            ModelState.AddModelError("SupplierId", "Select a valid supplier.");

        if (!ModelState.IsValid)
        {
            LoadLists(product.CategoryId, product.SupplierId);
            return View(product);
        }

        db.Products.Add(product);
        db.SaveChanges();
        return RedirectToAction("Index");
    }

    private void LoadLists(int? categoryId = null, int? supplierId = null)
    {
        ViewBag.Categories = new SelectList(db.Categories.ToList(), "Id", "Name", categoryId);
        ViewBag.Suppliers = new SelectList(db.Suppliers.ToList(), "Id", "Name", supplierId);
    }
}
