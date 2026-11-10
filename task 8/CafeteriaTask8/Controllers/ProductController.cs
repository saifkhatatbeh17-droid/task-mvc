using CafeteriaTask8.Data;
using CafeteriaTask8.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CafeteriaTask8.Controllers;

[Authorize]
public class ProductController : Controller
{
    private readonly ApplicationDbContext db;

    public ProductController(ApplicationDbContext db)
    {
        this.db = db;
    }

    public IActionResult Index()
    {
        return View(db.Products.OrderBy(p => p.Name).ToList());
    }

    public IActionResult Details(int id)
    {
        var item = db.Products.Find(id);
        if (item == null)
            return NotFound();
        return View(item);
    }

    [Authorize(Roles = "Admin,SuperAdmin")]
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [Authorize(Roles = "Admin,SuperAdmin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Product item)
    {
        if (!ModelState.IsValid)
            return View(item);

        db.Products.Add(item);
        db.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin,SuperAdmin")]
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var item = db.Products.Find(id);
        if (item == null)
            return NotFound();
        return View(item);
    }

    [Authorize(Roles = "Admin,SuperAdmin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Product item)
    {
        if (id != item.Id)
            return BadRequest();
        if (!ModelState.IsValid)
            return View(item);

        db.Products.Update(item);
        db.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin,SuperAdmin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        var item = db.Products.Find(id);
        if (item == null)
            return NotFound();

        if (db.RequestItems.Any(x => x.ProductId == id))
        {
            TempData["Error"] = "This product is used in a supply request and cannot be deleted.";
            return RedirectToAction(nameof(Index));
        }

        db.Products.Remove(item);
        db.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
}
