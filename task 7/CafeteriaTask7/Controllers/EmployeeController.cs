using CafeteriaTask7.Data;
using CafeteriaTask7.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
namespace CafeteriaTask7.Controllers;
public class EmployeeController : Controller
{
    private readonly ApplicationDbContext db;
    public EmployeeController(ApplicationDbContext context)
    {
        db = context;
    }
    public IActionResult Index()
    {
        return View(db.Employees.Include(x => x.Orders).ToList());
    }
    [HttpGet]
    public IActionResult Create()
    {

        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("FullName,Phone,Email,Position,Salary,HireDate")] Employee item)
    {
        if (item.Salary < 0)
            ModelState.AddModelError("Salary", "Salary cannot be negative.");
        if (item.HireDate == default)
            ModelState.AddModelError("HireDate", "Choose a hire date.");
        if (ModelState.IsValid)
        {
            db.Employees.Add(item);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        return View(item);
    }
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var item = db.Employees.Find(id);
        if (item == null) return NotFound();

        return View(item);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [Bind("Id,FullName,Phone,Email,Position,Salary,HireDate")] Employee item)
    {
        if (id != item.Id) return BadRequest();
        var saved = db.Employees.Find(id);
        if (saved == null) return NotFound();
        if (item.Salary < 0)
            ModelState.AddModelError("Salary", "Salary cannot be negative.");
        if (item.HireDate == default)
            ModelState.AddModelError("HireDate", "Choose a hire date.");
        if (ModelState.IsValid)
        {
            saved.FullName = item.FullName;
            saved.Phone = item.Phone;
            saved.Email = item.Email;
            saved.Position = item.Position;
            saved.Salary = item.Salary;
            saved.HireDate = item.HireDate;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        return View(item);
    }
    public IActionResult Details(int id)
    {
        var item = db.Employees.Include(x => x.Orders).FirstOrDefault(x => x.Id == id);
        if (item == null) return NotFound();
        return View(item);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        var item = db.Employees.Find(id);
        if (item == null) return NotFound();
        if (db.Orders.Any(o => o.EmployeeId == id))
        {
            TempData["Error"] = "Cannot delete an employee who has orders.";
            return RedirectToAction("Index");
        }
        db.Employees.Remove(item);
        db.SaveChanges();
        return RedirectToAction("Index");
    }
}
