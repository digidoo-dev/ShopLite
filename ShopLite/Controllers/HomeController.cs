using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopLite.Data;
using ShopLite.Models;

namespace ShopLite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ShopLiteContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ShopLiteContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string? search, string? category, int? minPrice, int? maxPrice, int? page)
        {
            var categories = await _context.Category.ToListAsync();

            ViewData["Categories"] = new SelectList(categories, "Name", "Name", category);
            ViewData["SelectedCategory"] = category;

            var model = _context.Product.AsQueryable();
            
            if (!string.IsNullOrWhiteSpace(search))
            {
                model = model.Where(m => m.Name.ToLower().Contains(search.ToLower()));
                ViewData["SearchTerm"] = search;
            }

            if (!string.IsNullOrEmpty(category) && category != "All")
            {
                model = model.Where(m => m.Category!.Name == category);
            }

            if (minPrice != null)
            {
                model = model.Where(m => m.Price > minPrice);
                ViewData["FilterMinPrice"] = minPrice;
            }

            if (maxPrice != null)
            {
                model = model.Where(m => m.Price < maxPrice);
                ViewData["FilterMaxPrice"] = maxPrice;
            }

            var count = await model.CountAsync();

            var pageSize = 18;
            int totalPages = (int)Math.Ceiling((decimal)count / pageSize);


            int pageNumber = page ?? 1;

            pageNumber = pageNumber > totalPages ? totalPages : pageNumber;

            ViewBag.TotalProductsCount = count;
            ViewBag.PageNumber = pageNumber;
            ViewBag.TotalPages = totalPages;

            return View(await model.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
