using CafeteriaCodeFirst.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Cafeteria"), sql => sql.MigrationsHistoryTable("__Task6MigrationsHistory", "Task6")));

var app = builder.Build();
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();
app.MapControllerRoute(name: "default",
    pattern: "{controller=Category}/{action=Index}/{id?}").WithStaticAssets();
app.Run();


