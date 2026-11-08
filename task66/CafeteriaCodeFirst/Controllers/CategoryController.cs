using CafeteriaCodeFirst.Data;
using CafeteriaCodeFirst.Models;
using Microsoft.AspNetCore.Mvc;

namespace CafeteriaCodeFirst.Controllers;

public class CategoryController : Controller
{
    private readonly ApplicationDbContext db;

    public CategoryController(ApplicationDbContext context)
    {
        db = context;
    }

    public IActionResult Index()
    {
        return View(db.Categories.ToList());
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("Name,Description")] Category category)
    {
        if (!ModelState.IsValid)
        {
            return View(category);
        }

        db.Categories.Add(category);
        db.SaveChanges();
        return RedirectToAction("Index");
    }
}
