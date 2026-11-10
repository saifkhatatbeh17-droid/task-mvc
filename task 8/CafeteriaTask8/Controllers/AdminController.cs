using CafeteriaTask8.Data;
using CafeteriaTask8.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CafeteriaTask8.Controllers;

[Authorize(Roles = "SuperAdmin")]
public class AdminController : Controller
{
    private readonly UserManager<ApplicationUser> users;
    private readonly ApplicationDbContext db;

    public AdminController(UserManager<ApplicationUser> users, ApplicationDbContext db)
    {
        this.users = users;
        this.db = db;
    }

    public async Task<IActionResult> Index()
    {
        var list = new List<UserRoleViewModel>();
        foreach (var user in users.Users.OrderBy(u => u.FullName).ToList())
        {
            var roles = await users.GetRolesAsync(user);
            list.Add(new UserRoleViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email ?? "",
                Role = roles.FirstOrDefault() ?? "User"
            });
        }
        return View(list);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        var user = await users.FindByIdAsync(id);
        if (user == null)
            return NotFound();

        var roles = await users.GetRolesAsync(user);
        var model = new UserRoleViewModel
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email ?? "",
            Role = roles.FirstOrDefault() ?? "User"
        };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UserRoleViewModel model)
    {
        if (model.Role is not ("User" or "Admin" or "SuperAdmin"))
            ModelState.AddModelError("Role", "Choose a valid role.");

        if (!ModelState.IsValid)
            return View(model);

        var user = await users.FindByIdAsync(model.Id);
        if (user == null)
            return NotFound();

        if (user.Id == users.GetUserId(User))
        {
            TempData["Error"] = "You cannot change your own role.";
            return RedirectToAction(nameof(Index));
        }

        var current = await users.GetRolesAsync(user);
        await users.RemoveFromRolesAsync(user, current);
        await users.AddToRoleAsync(user, model.Role);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
        var user = await users.FindByIdAsync(id);
        if (user == null)
            return NotFound();

        if (user.Id == users.GetUserId(User))
        {
            TempData["Error"] = "You cannot delete your own account.";
            return RedirectToAction(nameof(Index));
        }

        if (db.SupplyRequests.Any(r => r.UserId == user.Id))
        {
            TempData["Error"] = "This user has supply requests and cannot be deleted.";
            return RedirectToAction(nameof(Index));
        }

        await users.DeleteAsync(user);
        return RedirectToAction(nameof(Index));
    }
}
