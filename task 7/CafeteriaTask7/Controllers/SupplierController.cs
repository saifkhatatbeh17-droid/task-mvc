using CafeteriaTask7.Data;
using CafeteriaTask7.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
namespace CafeteriaTask7.Controllers;
public class SupplierController : Controller
{
    private readonly ApplicationDbContext db;
    public SupplierController(ApplicationDbContext context)
    {
        db = context;
    }
    public IActionResult Index()
    {
        return View(db.Suppliers.Include(x => x.Products).ToList());
    }
    [HttpGet]
    public IActionResult Create()
    {

        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("Name,Phone,Email,Address")] Supplier item)
    {

        if (ModelState.IsValid)
        {
            db.Suppliers.Add(item);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        return View(item);
    }
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var item = db.Suppliers.Find(id);
        if (item == null) return NotFound();

        return View(item);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [Bind("Id,Name,Phone,Email,Address")] Supplier item)
    {
        if (id != item.Id) return BadRequest();
        var saved = db.Suppliers.Find(id);
        if (saved == null) return NotFound();

        if (ModelState.IsValid)
        {
            saved.Name = item.Name;
            saved.Phone = item.Phone;
            saved.Email = item.Email;
            saved.Address = item.Address;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        return View(item);
    }
    public IActionResult Details(int id)
    {
        var item = db.Suppliers.Include(x => x.Products).FirstOrDefault(x => x.Id == id);
        if (item == null) return NotFound();
        return View(item);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        var item = db.Suppliers.Find(id);
        if (item == null) return NotFound();
        if (db.Products.Any(p => p.SupplierId == id))
        {
            TempData["Error"] = "Cannot delete a supplier that has products.";
            return RedirectToAction("Index");
        }
        db.Suppliers.Remove(item);
        db.SaveChanges();
        return RedirectToAction("Index");
    }
}
