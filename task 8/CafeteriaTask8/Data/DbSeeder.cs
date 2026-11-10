using CafeteriaTask8.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CafeteriaTask8.Data;

public static class DbSeeder
{
    public static async Task Seed(IServiceProvider services)
    {
        var db = services.GetRequiredService<ApplicationDbContext>();
        db.Database.Migrate();

        var roles = services.GetRequiredService<RoleManager<IdentityRole>>();
        foreach (var roleName in new[] { "User", "Admin", "SuperAdmin" })
        {
            if (!await roles.RoleExistsAsync(roleName))
                await roles.CreateAsync(new IdentityRole(roleName));
        }

        var users = services.GetRequiredService<UserManager<ApplicationUser>>();
        const string email = "superadmin@cafeteria.com";
        if (await users.FindByEmailAsync(email) == null)
        {
            var superAdmin = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FullName = "Super Admin"
            };
            var result = await users.CreateAsync(superAdmin, "Admin123");
            if (result.Succeeded)
                await users.AddToRoleAsync(superAdmin, "SuperAdmin");
        }

        if (!db.Products.Any())
        {
            db.Products.AddRange(
                new Product { Name = "Coffee", Description = "Coffee beans", Price = 5.00m, Quantity = 100 },
                new Product { Name = "Milk", Description = "Fresh milk", Price = 2.50m, Quantity = 80 },
                new Product { Name = "Juice", Description = "Orange juice", Price = 3.00m, Quantity = 60 },
                new Product { Name = "Sandwiches", Description = "Ready sandwiches", Price = 4.00m, Quantity = 40 },
                new Product { Name = "Bottled Water", Description = "Water bottles", Price = 1.00m, Quantity = 200 },
                new Product { Name = "Packaging Materials", Description = "Boxes and bags", Price = 1.50m, Quantity = 150 }
            );
            db.SaveChanges();
        }
    }
}
