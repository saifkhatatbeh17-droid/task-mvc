using AcademyGraduation.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddDbContext<GraduationContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Graduation")));

var app = builder.Build();
app.UseRouting();
app.UseSession();
app.UseAuthorization();
app.MapStaticAssets();
app.MapControllerRoute(name: "default",
    pattern: "{controller=Graduation}/{action=Index}/{id?}").WithStaticAssets();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<GraduationContext>();
    db.Database.EnsureCreated();
    var connection = db.Database.GetDbConnection();
    connection.Open();
    using (var command = connection.CreateCommand())
    {
        command.CommandText = "SELECT OBJECT_ID(N'Day5Task.Categories')";
        if (command.ExecuteScalar() == DBNull.Value)
        {
            string script = db.Database.GenerateCreateScript();
            foreach (string batch in script.Split("GO", StringSplitOptions.RemoveEmptyEntries))
            {
                if (!string.IsNullOrWhiteSpace(batch))
                    db.Database.ExecuteSqlRaw(batch);
            }
        }
    }
    connection.Close();
    if (!db.Categories.Any())
    {
        var categories = OfferData.GetCategories();
        foreach (var category in categories)
        {
            category.Id = 0;
            foreach (var supplier in category.Suppliers)
            {
                supplier.Id = 0;
                supplier.CategoryId = 0;
                supplier.Offer.Id = 0;
                supplier.Offer.SupplierId = 0;
            }
        }
        db.Categories.AddRange(categories);
        db.SaveChanges();
    }
}
app.Run();


