using CafeteriaTask7.Data;
using CafeteriaTask7.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
namespace CafeteriaTask7.Controllers;
public class CategoryController : Controller
{
    private readonly ApplicationDbContext db;
    public CategoryController(ApplicationDbContext context)
    {
        db = context;
    }
    public IActionResult Index()
    {
        return View(db.Categories.Include(x => x.Products).ToList());
    }
    [HttpGet]
    public IActionResult Create()
    {

        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("Name,Description")] Category item)
    {

        if (ModelState.IsValid)
        {
            db.Categories.Add(item);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        return View(item);
    }
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var item = db.Categories.Find(id);
        if (item == null) return NotFound();

        return View(item);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [Bind("Id,Name,Description")] Category item)
    {
        if (id != item.Id) return BadRequest();
        var saved = db.Categories.Find(id);
        if (saved == null) return NotFound();

        if (ModelState.IsValid)
        {
            saved.Name = item.Name;
            saved.Description = item.Description;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        return View(item);
    }
    public IActionResult Details(int id)
    {
        var item = db.Categories.Include(x => x.Products).FirstOrDefault(x => x.Id == id);
        if (item == null) return NotFound();
        return View(item);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        var item = db.Categories.Find(id);
        if (item == null) return NotFound();
        if (db.Products.Any(p => p.CategoryId == id))
        {
            TempData["Error"] = "Cannot delete a category that has products.";
            return RedirectToAction("Index");
        }
        db.Categories.Remove(item);
        db.SaveChanges();
        return RedirectToAction("Index");
    }
}
