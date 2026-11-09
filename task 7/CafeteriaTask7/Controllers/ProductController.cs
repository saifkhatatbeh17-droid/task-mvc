using CafeteriaTask7.Data;
using CafeteriaTask7.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
namespace CafeteriaTask7.Controllers;
public class ProductController : Controller
{
    private readonly ApplicationDbContext db;
    public ProductController(ApplicationDbContext context)
    {
        db = context;
    }
    public IActionResult Index()
    {
        return View(db.Products.Include(x => x.Category).Include(x => x.Supplier).ToList());
    }
    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
        ViewBag.Suppliers = new SelectList(db.Suppliers.ToList(), "Id", "Name");
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("Name,Description,Price,Quantity,CategoryId,SupplierId")] Product item)
    {
        if (item.Price <= 0)
            ModelState.AddModelError("Price", "Price must be greater than zero.");
        if (item.Quantity < 0)
            ModelState.AddModelError("Quantity", "Quantity cannot be negative.");
        if (!db.Categories.Any(c => c.Id == item.CategoryId))
            ModelState.AddModelError("CategoryId", "Choose a category.");
        if (!db.Suppliers.Any(s => s.Id == item.SupplierId))
            ModelState.AddModelError("SupplierId", "Choose a supplier.");
        if (ModelState.IsValid)
        {
            db.Products.Add(item);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        ViewBag.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
        ViewBag.Suppliers = new SelectList(db.Suppliers.ToList(), "Id", "Name");
        return View(item);
    }
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var item = db.Products.Find(id);
        if (item == null) return NotFound();
        ViewBag.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
        ViewBag.Suppliers = new SelectList(db.Suppliers.ToList(), "Id", "Name");
        return View(item);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [Bind("Id,Name,Description,Price,Quantity,CategoryId,SupplierId")] Product item)
    {
        if (id != item.Id) return BadRequest();
        var saved = db.Products.Find(id);
        if (saved == null) return NotFound();
        if (item.Price <= 0)
            ModelState.AddModelError("Price", "Price must be greater than zero.");
        if (item.Quantity < 0)
            ModelState.AddModelError("Quantity", "Quantity cannot be negative.");
        if (!db.Categories.Any(c => c.Id == item.CategoryId))
            ModelState.AddModelError("CategoryId", "Choose a category.");
        if (!db.Suppliers.Any(s => s.Id == item.SupplierId))
            ModelState.AddModelError("SupplierId", "Choose a supplier.");
        if (ModelState.IsValid)
        {
            saved.Name = item.Name;
            saved.Description = item.Description;
            saved.Price = item.Price;
            saved.Quantity = item.Quantity;
            saved.CategoryId = item.CategoryId;
            saved.SupplierId = item.SupplierId;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        ViewBag.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
        ViewBag.Suppliers = new SelectList(db.Suppliers.ToList(), "Id", "Name");
        return View(item);
    }
    public IActionResult Details(int id)
    {
        var item = db.Products.Include(x => x.Category).Include(x => x.Supplier).FirstOrDefault(x => x.Id == id);
        if (item == null) return NotFound();
        return View(item);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        var item = db.Products.Find(id);
        if (item == null) return NotFound();

        db.Products.Remove(item);
        db.SaveChanges();
        return RedirectToAction("Index");
    }
}
