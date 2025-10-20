using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopLite.Data;
using ShopLite.Models;

namespace ShopLite.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ShopLiteContext _context;

        public ProductsController(ShopLiteContext context)
        {
            _context = context;
        }


        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        
        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ID == id);
        }
    }
}
