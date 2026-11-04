using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using UniversityCafeteria.Models;

namespace UniversityCafeteria.Controllers
{
    public class SupplyRequestController : Controller
    {
        private static List<SupplyRequest> requests = new List<SupplyRequest>();
        private static int nextId = 1;

        public IActionResult Index()
        {
            SupplyRequest draft = GetDraft();
            string preference = Request.Cookies["PreferredPriority"] ?? "Normal";
            if (!ValidPriority(preference))
            {
                preference = "Normal";
            }

            ViewBag.PreferredPriority = preference;
            ViewBag.SelectedPriority = draft.Items.Count > 0 ? draft.Priority : preference;
            Calculate(draft);
            return View(draft);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddItem(string productName, string category, int quantity,
            decimal unitPrice, string priority)
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(productName) ||
                quantity <= 0 || unitPrice <= 0 || !ValidPriority(priority))
            {
                TempData["Error"] = "Enter a product name, a positive quantity and price, and a valid priority.";
                return RedirectToAction("Index");
            }

            SupplyRequest draft = GetDraft();
            SupplyItem item = new SupplyItem();
            item.ProductName = productName.Trim();
            item.Category = category == null ? "" : category.Trim();
            item.Quantity = quantity;
            item.UnitPrice = unitPrice;
            draft.Items.Add(item);
            draft.Priority = priority;
            Calculate(draft);
            SaveDraft(draft);

            TempData["Message"] = "Product added to the draft.";
            return RedirectToAction("Index");
        }

        public IActionResult Review()
        {
            SupplyRequest draft = GetDraft();
            Calculate(draft);
            return View(draft);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmRequest()
        {
            SupplyRequest draft = GetDraft();
            if (draft.Items.Count == 0)
            {
                TempData["Error"] = "Add at least one product before confirming the request.";
                return RedirectToAction("Index");
            }

            Calculate(draft);
            draft.Status = "Confirmed";
            lock (requests)
            {
                draft.RequestId = nextId;
                nextId++;
                requests.Add(draft);
            }

            HttpContext.Session.Remove("SupplyDraft");
            TempData["Message"] = "Supply request confirmed.";
            return RedirectToAction("MyRequests");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ClearDraft()
        {
            HttpContext.Session.Remove("SupplyDraft");
            TempData["Message"] = "Draft cleared.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SavePreference(string priority)
        {
            if (!ValidPriority(priority))
            {
                TempData["Error"] = "Choose a valid priority.";
                return RedirectToAction("Index");
            }

            CookieOptions options = new CookieOptions();
            options.Expires = DateTimeOffset.UtcNow.AddDays(7);
            options.HttpOnly = true;
            options.IsEssential = true;
            options.SameSite = SameSiteMode.Lax;
            Response.Cookies.Append("PreferredPriority", priority, options);
            TempData["Message"] = "Your preferred request priority is " + priority + ".";
            return RedirectToAction("Index");
        }

        public IActionResult MyRequests()
        {
            List<SupplyRequest> savedRequests;
            lock (requests)
            {
                savedRequests = requests.ToList();
            }
            return View(savedRequests);
        }

        public IActionResult About()
        {
            return View();
        }

        private SupplyRequest GetDraft()
        {
            string? data = HttpContext.Session.GetString("SupplyDraft");
            if (data == null)
            {
                return new SupplyRequest();
            }
            return JsonSerializer.Deserialize<SupplyRequest>(data) ?? new SupplyRequest();
        }

        private void SaveDraft(SupplyRequest draft)
        {
            string data = JsonSerializer.Serialize(draft);
            HttpContext.Session.SetString("SupplyDraft", data);
        }

        private bool ValidPriority(string priority)
        {
            return priority == "Normal" || priority == "Urgent" || priority == "Emergency";
        }

        private void Calculate(SupplyRequest draft)
        {
            draft.Subtotal = 0;
            foreach (SupplyItem item in draft.Items)
            {
                item.Total = Math.Round(item.Quantity * item.UnitPrice, 2);
                draft.Subtotal += item.Total;
            }

            switch (draft.Priority)
            {
                case "Urgent":
                    draft.PreparationFee = 5;
                    break;
                case "Emergency":
                    draft.PreparationFee = 10;
                    break;
                default:
                    draft.PreparationFee = 2;
                    break;
            }

            if (draft.Subtotal > 200)
            {
                draft.Discount = Math.Round(draft.Subtotal * 0.15m, 2);
            }
            else if (draft.Subtotal > 100)
            {
                draft.Discount = Math.Round(draft.Subtotal * 0.10m, 2);
            }
            else
            {
                draft.Discount = 0;
            }

            draft.FinalTotal = draft.Subtotal + draft.PreparationFee - draft.Discount;
        }
    }
}
