using CafeteriaTask7.Data;
using CafeteriaTask7.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
namespace CafeteriaTask7.Controllers;
public class OrderController : Controller
{
    private readonly ApplicationDbContext db;
    public OrderController(ApplicationDbContext context)
    {
        db = context;
    }
    public IActionResult Index()
    {
        return View(db.Orders.Include(x => x.Employee).ToList());
    }
    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.Employees = new SelectList(db.Employees.ToList(), "Id", "FullName");
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("CustomerName,OrderDate,TotalPrice,Status,EmployeeId")] Order item)
    {
        if (item.TotalPrice < 0)
            ModelState.AddModelError("TotalPrice", "Total price cannot be negative.");
        if (item.OrderDate == default)
            ModelState.AddModelError("OrderDate", "Choose an order date.");
        if (!db.Employees.Any(e => e.Id == item.EmployeeId))
            ModelState.AddModelError("EmployeeId", "Choose an employee.");
        if (ModelState.IsValid)
        {
            db.Orders.Add(item);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        ViewBag.Employees = new SelectList(db.Employees.ToList(), "Id", "FullName");
        return View(item);
    }
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var item = db.Orders.Find(id);
        if (item == null) return NotFound();
        ViewBag.Employees = new SelectList(db.Employees.ToList(), "Id", "FullName");
        return View(item);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [Bind("Id,CustomerName,OrderDate,TotalPrice,Status,EmployeeId")] Order item)
    {
        if (id != item.Id) return BadRequest();
        var saved = db.Orders.Find(id);
        if (saved == null) return NotFound();
        if (item.TotalPrice < 0)
            ModelState.AddModelError("TotalPrice", "Total price cannot be negative.");
        if (item.OrderDate == default)
            ModelState.AddModelError("OrderDate", "Choose an order date.");
        if (!db.Employees.Any(e => e.Id == item.EmployeeId))
            ModelState.AddModelError("EmployeeId", "Choose an employee.");
        if (ModelState.IsValid)
        {
            saved.CustomerName = item.CustomerName;
            saved.OrderDate = item.OrderDate;
            saved.TotalPrice = item.TotalPrice;
            saved.Status = item.Status;
            saved.EmployeeId = item.EmployeeId;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        ViewBag.Employees = new SelectList(db.Employees.ToList(), "Id", "FullName");
        return View(item);
    }
    public IActionResult Details(int id)
    {
        var item = db.Orders.Include(x => x.Employee).FirstOrDefault(x => x.Id == id);
        if (item == null) return NotFound();
        return View(item);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        var item = db.Orders.Find(id);
        if (item == null) return NotFound();

        db.Orders.Remove(item);
        db.SaveChanges();
        return RedirectToAction("Index");
    }
}
