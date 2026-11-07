using AcademyGraduation.Data;
using AcademyGraduation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace AcademyGraduation.Controllers;

public class GraduationController : Controller
{
    private readonly GraduationContext db;
    public GraduationController(GraduationContext context)
    {
        db = context;
    }

    public IActionResult Index()
    {
        int phase = GetPhase();
        ViewBag.Phase = phase;
        ViewBag.Choices = GetChoices(phase);
        return View(GetCategories(phase));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult SetPhase(int phase)
    {
        if (phase < 1 || phase > 3)
        {
            TempData["Error"] = "Choose phase 1, 2 or 3.";
            return RedirectToAction("Index");
        }
        HttpContext.Session.SetInt32("Phase", phase);
        Response.Cookies.Append("GraduationPhase", phase.ToString(), new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddDays(7), HttpOnly = true, IsEssential = true
        });
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult SelectOffer(int categoryId, int offerId)
    {
        int phase = GetPhase();
        var categories = GetCategories(phase);
        var category = categories.FirstOrDefault(c => c.Id == categoryId);
        if (!ModelState.IsValid || category == null ||
            !category.Suppliers.Any(s => s.Offer.Id == offerId))
        {
            TempData["Error"] = "Please select an offer from the correct category.";
            return RedirectToAction("Index");
        }

        if (phase == 1)
        {
            string key = GetCategoryKey(category.Name);
            HttpContext.Session.SetInt32(key, offerId);
        }
        else
        {
            var choices = GetChoices(phase);
            var previous = choices.FirstOrDefault(c => c.CategoryId == categoryId);
            if (previous != null)
            {
                choices.Remove(previous);
            }
            choices.Add(new OfferChoice { CategoryId = categoryId, OfferId = offerId });
            HttpContext.Session.SetString("OffersPhase" + phase, JsonSerializer.Serialize(choices));
        }
        TempData["Message"] = "Offer selected. You can change it by choosing another offer in the same category.";
        return RedirectToAction("Index");
    }

    public IActionResult Review()
    {
        int phase = GetPhase();
        var categories = GetCategories(phase);
        var choices = GetChoices(phase);
        var selected = new List<Offer>();
        foreach (var category in categories)
        {
            var choice = choices.FirstOrDefault(c => c.CategoryId == category.Id);
            var offer = category.Suppliers.Select(s => s.Offer)
                .FirstOrDefault(o => o.Id == choice?.OfferId);
            if (offer == null)
            {
                TempData["Error"] = "Select one offer from each of the three categories before reviewing.";
                return RedirectToAction("Index");
            }
            selected.Add(offer);
        }

        decimal subtotal = 0;
        var totals = new List<decimal>();
        foreach (var offer in selected)
        {
            decimal total = offer.Quantity * offer.UnitPrice;
            totals.Add(total);
            subtotal += total;
        }
        int discountPercentage;
        if (subtotal > 500)
            discountPercentage = 15;
        else if (subtotal > 300)
            discountPercentage = 10;
        else
            discountPercentage = 0;

        decimal discount = Math.Round(subtotal * discountPercentage / 100m, 2);
        ViewBag.Totals = totals;
        ViewBag.Subtotal = subtotal;
        ViewBag.DiscountPercentage = discountPercentage;
        ViewBag.DiscountAmount = discount;
        ViewBag.FinalTotal = subtotal - discount;
        ViewBag.Phase = phase;
        return View(selected);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ClearSelections()
    {
        int phase = GetPhase();
        if (phase == 1)
        {
            HttpContext.Session.Remove("CertificateOffer");
            HttpContext.Session.Remove("HospitalityOffer");
            HttpContext.Session.Remove("GiftOffer");
        }
        else
        {
            HttpContext.Session.Remove("OffersPhase" + phase);
        }
        return RedirectToAction("Index");
    }

    public IActionResult About()
    {
        return View();
    }

    private int GetPhase()
    {
        int? phase = HttpContext.Session.GetInt32("Phase");
        if (phase.HasValue && phase >= 1 && phase <= 3)
            return phase.Value;
        if (int.TryParse(Request.Cookies["GraduationPhase"], out int saved) && saved >= 1 && saved <= 3)
            return saved;
        return 1;
    }

    private List<Category> GetCategories(int phase)
    {
        if (phase == 3)
            return db.Categories.Include(c => c.Suppliers).ThenInclude(s => s.Offer)
                .OrderBy(c => c.Id).ToList();
        return OfferData.GetCategories();
    }

    private List<OfferChoice> GetChoices(int phase)
    {
        if (phase == 1)
        {
            var choices = new List<OfferChoice>();
            foreach (var category in OfferData.GetCategories())
            {
                int? offerId = HttpContext.Session.GetInt32(GetCategoryKey(category.Name));
                if (offerId.HasValue)
                    choices.Add(new OfferChoice { CategoryId = category.Id, OfferId = offerId.Value });
            }
            return choices;
        }
        string? data = HttpContext.Session.GetString("OffersPhase" + phase);
        return data == null ? new List<OfferChoice>() :
            JsonSerializer.Deserialize<List<OfferChoice>>(data) ?? new List<OfferChoice>();
    }

    private string GetCategoryKey(string category)
    {
        switch (category)
        {
            case "Graduation Certificates": return "CertificateOffer";
            case "Hospitality": return "HospitalityOffer";
            default: return "GiftOffer";
        }
    }
}
