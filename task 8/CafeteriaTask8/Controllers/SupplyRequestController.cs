using CafeteriaTask8.Data;
using CafeteriaTask8.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CafeteriaTask8.Controllers;

[Authorize]
public class SupplyRequestController : Controller
{
    private readonly ApplicationDbContext db;
    private readonly UserManager<ApplicationUser> users;

    public SupplyRequestController(ApplicationDbContext db, UserManager<ApplicationUser> users)
    {
        this.db = db;
        this.users = users;
    }

    public IActionResult Index()
    {
        var query = db.SupplyRequests
            .Include(r => r.User)
            .AsQueryable();

        if (!IsManager())
        {
            var userId = users.GetUserId(User);
            query = query.Where(r => r.UserId == userId);
        }

        var list = query.OrderByDescending(r => r.RequestDate).ToList();
        return View(list);
    }

    public IActionResult Details(int id)
    {
        var request = Load(id);
        if (request == null)
            return NotFound();
        if (!CanSee(request))
            return Forbid();
        return View(request);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new RequestFormViewModel
        {
            CanChangeItems = true,
            Lines = BuildLines()
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(RequestFormViewModel model)
    {
        if (!TryBuildItems(model.Lines, out var items, out var total))
        {
            model.CanChangeItems = true;
            KeepQuantities(model);
            return View(model);
        }

        var request = new SupplyRequest
        {
            RequestDate = DateTime.Now,
            Status = RequestStatus.Pending,
            TotalPrice = total,
            UserId = users.GetUserId(User)!,
            Items = items
        };

        db.SupplyRequests.Add(request);
        db.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var request = Load(id);
        if (request == null)
            return NotFound();
        if (!CanSee(request))
            return Forbid();

        var model = new RequestFormViewModel
        {
            Id = request.Id,
            Status = request.Status,
            Lines = BuildLines(request)
        };
        SetPermissions(model, request);

        if (!model.CanChangeItems && !model.CanChangeStatus)
            return RedirectToAction(nameof(Details), new { id });

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(RequestFormViewModel model)
    {
        var request = Load(model.Id);
        if (request == null)
            return NotFound();
        if (!CanSee(request))
            return Forbid();

        var isOwner = request.UserId == users.GetUserId(User);
        var canChangeItems = isOwner && request.Status == RequestStatus.Pending;

        if (canChangeItems)
        {
            if (!TryBuildItems(model.Lines, out var items, out var total))
            {
                model.CanChangeItems = true;
                model.CanChangeStatus = IsManager();
                KeepQuantities(model);
                return View(model);
            }

            db.RequestItems.RemoveRange(request.Items);
            request.Items.Clear();
            foreach (var item in items)
                request.Items.Add(item);
            request.TotalPrice = total;
        }

        if (IsManager() && RequestStatus.All.Contains(model.Status))
            request.Status = model.Status;

        db.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        var request = Load(id);
        if (request == null)
            return NotFound();

        var isOwner = request.UserId == users.GetUserId(User);
        if (!IsManager() && !(isOwner && request.Status == RequestStatus.Pending))
            return Forbid();

        db.RequestItems.RemoveRange(request.Items);
        db.SupplyRequests.Remove(request);
        db.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    private SupplyRequest? Load(int id)
    {
        return db.SupplyRequests
            .Include(r => r.User)
            .Include(r => r.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefault(r => r.Id == id);
    }

    private bool IsManager()
    {
        return User.IsInRole("Admin") || User.IsInRole("SuperAdmin");
    }

    private bool CanSee(SupplyRequest request)
    {
        return IsManager() || request.UserId == users.GetUserId(User);
    }

    private void SetPermissions(RequestFormViewModel model, SupplyRequest request)
    {
        model.CanChangeStatus = IsManager();
        model.CanChangeItems = request.UserId == users.GetUserId(User)
            && request.Status == RequestStatus.Pending;
    }

    private List<RequestLine> BuildLines(SupplyRequest? request = null)
    {
        return db.Products.OrderBy(p => p.Name).Select(p => new RequestLine
        {
            ProductId = p.Id,
            ProductName = p.Name,
            Price = p.Price,
            Available = p.Quantity,
            Quantity = 0
        }).ToList()
        .Select(line =>
        {
            if (request != null)
            {
                var existing = request.Items.FirstOrDefault(i => i.ProductId == line.ProductId);
                if (existing != null)
                    line.Quantity = existing.Quantity;
            }
            return line;
        }).ToList();
    }

    private void KeepQuantities(RequestFormViewModel model)
    {
        var fresh = BuildLines();
        foreach (var line in fresh)
        {
            var posted = model.Lines.FirstOrDefault(x => x.ProductId == line.ProductId);
            if (posted != null)
                line.Quantity = posted.Quantity;
        }
        model.Lines = fresh;
    }

    private bool TryBuildItems(List<RequestLine> lines, out List<RequestItem> items, out decimal total)
    {
        items = new List<RequestItem>();
        total = 0;
        lines ??= new List<RequestLine>();

        if (lines.Any(l => l.Quantity < 0))
        {
            ModelState.AddModelError("", "Quantity cannot be negative.");
            return false;
        }

        var selected = lines.Where(l => l.Quantity > 0).ToList();
        if (selected.Count == 0)
        {
            ModelState.AddModelError("", "Enter a quantity for at least one product.");
            return false;
        }

        foreach (var line in selected)
        {
            var product = db.Products.Find(line.ProductId);
            if (product == null)
            {
                ModelState.AddModelError("", "One of the products was not found.");
                return false;
            }
            if (line.Quantity > product.Quantity)
            {
                ModelState.AddModelError("", product.Name + " has only " + product.Quantity + " available.");
                return false;
            }

            items.Add(new RequestItem
            {
                ProductId = product.Id,
                Quantity = line.Quantity,
                Price = product.Price
            });
            total += product.Price * line.Quantity;
        }

        return true;
    }
}
