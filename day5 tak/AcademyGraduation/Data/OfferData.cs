using AcademyGraduation.Models;

namespace AcademyGraduation.Data;

public static class OfferData
{
    public static List<Category> GetCategories()
    {
        var categories = new List<Category>
        {
            new Category { Id = 1, Name = "Graduation Certificates" },
            new Category { Id = 2, Name = "Hospitality" },
            new Category { Id = 3, Name = "Graduation Gifts" }
        };
        string[] names = { "Premium Certificate Package", "Standard Certificate Package", "Luxury Certificate Package",
            "Coffee & Drinks Package", "Sandwiches & Snacks Package", "Full Hospitality Package",
            "Customized Mugs Package", "Graduation Bags Package", "Premium Gift Boxes Package" };
        decimal[] prices = { 2m, 1.5m, 2.5m, 2m, 3m, 4m, 2m, 3m, 4m };
        for (int i = 0; i < 9; i++)
        {
            int categoryIndex = i / 3;
            var supplier = new Supplier();
            supplier.Id = i + 1;
            supplier.Name = "Supplier " + (char)('A' + i);
            supplier.CategoryId = categoryIndex + 1;
            supplier.Category = categories[categoryIndex];
            supplier.Offer = new Offer
            {
                Id = i + 1, Name = names[i], Quantity = categoryIndex == 1 ? 100 : 35,
                UnitPrice = prices[i], SupplierId = supplier.Id, Supplier = supplier
            };
            categories[categoryIndex].Suppliers.Add(supplier);
        }
        return categories;
    }
}
