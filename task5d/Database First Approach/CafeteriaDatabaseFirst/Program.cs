using CafeteriaDatabaseFirst.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<CafeteriaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Cafeteria")));

var app = builder.Build();
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();
app.MapControllerRoute(name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}").WithStaticAssets();
app.Run();
