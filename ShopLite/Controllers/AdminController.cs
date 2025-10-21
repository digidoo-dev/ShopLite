using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopLite.Data;
using ShopLite.Models;

namespace ShopLite.Controllers;

public class AdminController : Controller
{
    private readonly ShopLiteContext _context;

    public AdminController(ShopLiteContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }





    // ==============================================================
    // Products
    // ==============================================================

    // GET: Admin/ManageProducts
    public async Task<IActionResult> ManageProducts(string? category, string? q, int? page)
    {
        

        var categories = await _context.Category.ToListAsync();
        ViewBag.Categories = categories.Select(c => c.Name).ToList();

        var products = _context.Product.Include(p => p.Category).AsQueryable();

        if (!string.IsNullOrEmpty(category))
        {
            ViewBag.SelectedCategory = category;
            products = products.Where(p => p.Category!.Name == category);
        }

        if (!string.IsNullOrWhiteSpace(q))
        {
            ViewBag.Search = q;
            products = products.Where(p => p.Name.ToLower().Contains(q.ToLower()) || p.ID.ToString().Contains(q));
        }

        var count = await products.CountAsync();

        var pageSize = 20;
        int totalPages = (int)Math.Ceiling((decimal)count / pageSize);

        
        int pageNumber = page ?? 1;

        pageNumber = pageNumber > totalPages ? totalPages : pageNumber;

        
        ViewBag.PageNumber = pageNumber;
        ViewBag.TotalPages = totalPages;


        return View(await products.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync());
    }

    // GET: Admin/CreateProduct
    public IActionResult CreateProduct()
    {
        ViewData["CategoryID"] = new SelectList(_context.Category, "ID", "Name");
        return View();
    }

    // POST: Admin/CreateProduct
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateProduct([Bind("ID,CategoryID,Name,Description,ImageUrl,Price")] Product product)
    {
        if (ModelState.IsValid)
        {
            _context.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ManageProducts));
        }
        ViewData["CategoryID"] = new SelectList(_context.Category, "ID", "Name", product.CategoryID);
        return View(product);
    }

    // GET: Admin/EditProduct/5
    public async Task<IActionResult> EditProduct(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _context.Product.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        ViewData["CategoryID"] = new SelectList(_context.Category, "ID", "Name", product.CategoryID);
        return View(product);
    }

    // POST: Admin/EditProduct/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditProduct(int id, [Bind("ID,CategoryID,Name,Description,ImageUrl,Price")] Product product)
    {
        if (id != product.ID)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(product);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(ManageProducts));
        }
        ViewData["CategoryID"] = new SelectList(_context.Category, "ID", "Name", product.CategoryID);
        return View(product);
    }

    // GET: Admin/DeleteProduct/5
    public async Task<IActionResult> DeleteProduct(int? id)
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

    // POST: Admin/DeleteProduct/5
    [HttpPost, ActionName("DeleteProduct")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteProductConfirmed(int id)
    {
        var product = await _context.Product.FindAsync(id);
        if (product != null)
        {
            _context.Product.Remove(product);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(ManageProducts));
    }

    private bool ProductExists(int id)
    {
        return _context.Product.Any(e => e.ID == id);
    }


    // ==============================================================
    // CATEGORIES
    // ==============================================================

    // GET: Admin/ManageCategories
    public async Task<IActionResult> ManageCategories()
    {
        return View(await _context.Category.ToListAsync());
    }



    // GET: Admin/CreateCategory
    public IActionResult CreateCategory()
    {
        return View();
    }

    // POST: Admin/CreateCategory
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateCategory([Bind("ID,Name")] Category category)
    {
        if (ModelState.IsValid)
        {
            _context.Add(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ManageCategories));
        }
        return View(category);
    }

    // GET: Admin/EditCategory/5
    public async Task<IActionResult> EditCategory(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var category = await _context.Category.FindAsync(id);
        if (category == null)
        {
            return NotFound();
        }
        return View(category);
    }

    // POST: Admin/EditCategory/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditCategory(int id, [Bind("ID,Name")] Category category)
    {
        if (id != category.ID)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(category);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(category.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(ManageCategories));
        }
        return View(category);
    }

    // GET: Admin/DeleteCategory/5
    public async Task<IActionResult> DeleteCategory(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var category = await _context.Category
            .FirstOrDefaultAsync(m => m.ID == id);
        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }

    // POST: Admin/DeleteCategory/5
    [HttpPost, ActionName("DeleteCategory")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteCategoryConfirmed(int id)
    {
        var category = await _context.Category.FindAsync(id);
        if (category != null)
        {
            _context.Category.Remove(category);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(ManageCategories));
    }

    private bool CategoryExists(int id)
    {
        return _context.Category.Any(e => e.ID == id);
    }




    // ==============================================================
    // ORDERS
    // ==============================================================


    // GET: Admin/ManageOrders
    public async Task<IActionResult> ManageOrders(string? status, DateTime? dateFrom, DateTime? dateTo, string? orderId)
    {
        var allOrders = await _context.Order.OrderDescending().Include(o => o.OrderProducts).ToListAsync();

        var last7DaysDates = new List<String> {
            DateTime.Now.ToString("d.MM"),
            DateTime.Now.AddDays(-1).ToString("d.MM"),
            DateTime.Now.AddDays(-2).ToString("d.MM"),
            DateTime.Now.AddDays(-3).ToString("d.MM"),
            DateTime.Now.AddDays(-4).ToString("d.MM"),
            DateTime.Now.AddDays(-5).ToString("d.MM"),
            DateTime.Now.AddDays(-6).ToString("d.MM")
            };

        if (allOrders == null || !allOrders.Any())
        {
            return View(new ManageOrdersViewModel {
                Orders = new List<Order>(),
                MonthOrdersTotal = 0,
                MonthOrdersCount = 0,
                AverageOrderValue = 0,
                Last7DaysCounts = new List<int> { 0, 0, 0, 0, 0, 0, 0}, 
                Last7DaysLabels = last7DaysDates
            });
        }


        List<Order> sortedFilteredOrders = allOrders;

        if (!string.IsNullOrEmpty(status))
        {
            sortedFilteredOrders = allOrders.Where(o => status == "InProgress" ? o.Status == OrderStatus.InProgress : o.Status == OrderStatus.Sent).ToList();
            ViewBag.CurrentStatus = status;
        }

        if (dateFrom != null)
        {
            sortedFilteredOrders = sortedFilteredOrders.Where(o => o.OrderDate >= dateFrom).ToList();
            ViewBag.DateFrom = dateFrom;
        }

        if (dateTo != null)
        {
            sortedFilteredOrders = sortedFilteredOrders.Where(o => o.OrderDate <= dateTo).ToList();
            ViewBag.DateTo = dateTo;
        }

        if (!string.IsNullOrWhiteSpace(orderId))
        {
            sortedFilteredOrders = sortedFilteredOrders.Where(o => o.ID.ToString().Contains(orderId)).ToList();
            ViewBag.OrderId = orderId;
        }

        var thisMonthOrders = allOrders.Where(o => o.OrderDate.Month == DateTime.Today.Month).ToList();
        
        if (thisMonthOrders == null || !thisMonthOrders.Any())
        {
            return View(new ManageOrdersViewModel { 
                Orders = sortedFilteredOrders, 
                MonthOrdersTotal = 0, 
                MonthOrdersCount = 0, 
                AverageOrderValue = 0, 
                Last7DaysCounts = new List<int> { 0, 0, 0, 0, 0, 0, 0 },
                Last7DaysLabels = last7DaysDates 
            });
        }

        var thisMonthOrderCount = thisMonthOrders.Count;
        var thisMonthOrderValue = thisMonthOrders.Sum(o => o.TotalPrice);
        var thisMonthAverageOrderValue = thisMonthOrderValue / thisMonthOrderCount;

        

        var last7DaysOrders = thisMonthOrders.Where(o => o.OrderDate > DateTime.Today.AddDays(-7)).ToList();

        if (last7DaysOrders == null || !last7DaysOrders.Any())
        {
            return View(new ManageOrdersViewModel
            {
                Orders = sortedFilteredOrders,
                MonthOrdersTotal = thisMonthOrderValue,
                MonthOrdersCount = thisMonthOrderCount,
                AverageOrderValue = thisMonthAverageOrderValue,
                Last7DaysCounts = new List<int> { 0, 0, 0, 0, 0, 0, 0 },
                Last7DaysLabels = last7DaysDates
            });
        }

        var last7DaysOrderCounts = new List<int>();

        for (int i = 0; i < 7; i++)
        {
            var thisDayOrders = last7DaysOrders.Where(o => o.OrderDate.Date > DateTime.Today.AddDays(-1 - i)).ToList();
            last7DaysOrders = last7DaysOrders.Where(o => o.OrderDate.Date <= DateTime.Today.AddDays(-1 - i)).ToList();
            last7DaysOrderCounts.Add(thisDayOrders.Count);
        }

        var model = new ManageOrdersViewModel
        {
            Last7DaysCounts = last7DaysOrderCounts,
            Last7DaysLabels = last7DaysDates,
            AverageOrderValue = thisMonthAverageOrderValue,
            MonthOrdersTotal = thisMonthOrderValue,
            MonthOrdersCount = thisMonthOrderCount,
            Orders = sortedFilteredOrders
        };

        return View(model);
    }

    // GET: Admin/OrderDetails/5
    public async Task<IActionResult> OrderDetails(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var order = await _context.Order.Include(o => o.OrderProducts).ThenInclude(op => op.Product)
            .FirstOrDefaultAsync(m => m.ID == id);
        if (order == null)
        {
            return NotFound();
        }

        return View(order);
    }



    // POST: Admin/MarkOrderAsSent/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkOrderAsSent(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var order = await _context.Order.FindAsync(id);
        if (order == null)
        {
            return NotFound();
        }

        order.Status = OrderStatus.Sent;
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(ManageOrders));
    }

    

    // GET: Admin/DeleteOrder/5
    public async Task<IActionResult> DeleteOrder(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var order = await _context.Order
            .FirstOrDefaultAsync(m => m.ID == id);
        if (order == null)
        {
            return NotFound();
        }

        return View(order);
    }

    // POST: Admin/DeleteOrder/5
    [HttpPost, ActionName("DeleteOrder")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteOrderConfirmed(int id)
    {
        var order = await _context.Order.FindAsync(id);
        if (order != null)
        {
            _context.Order.Remove(order);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(ManageOrders));
    }

    private bool OrderExists(int id)
    {
        return _context.Order.Any(e => e.ID == id);
    }


}
