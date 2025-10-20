using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopLite.Data;
using ShopLite.Helpers;
using ShopLite.Models;
using System.Linq;

namespace ShopLite.Controllers;

public class CartController : Controller
{
    private readonly ShopLiteContext _context;

    public CartController(ShopLiteContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart");

        if (cart == null || !cart.Any())
        {
            return View(new CartViewModel { Products = new List<Product>(), Quantities = new List<int>()});
        }

        var productsInCart = new List<Product>();
        var quantities = new List<int>();

        var productIDs = cart.Select(c => c.ProductID).ToList();
        productsInCart = await _context.Product.Where(p => productIDs.Contains(p.ID)).ToListAsync();

        foreach (var product in productsInCart)
        {
            var item = cart.First(c => c.ProductID == product.ID);
            quantities.Add(item.Quantity);
        }

        return View(new CartViewModel { Products = productsInCart, Quantities = quantities});
    }


    public async Task<IActionResult> AddToCart(int id)
    {
        var product = await _context.Product.FirstOrDefaultAsync(p => p.ID == id);

        if (product == null) { return RedirectToAction(nameof(Index)); }

        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddToCartConfirmed(int productID, int quantity)
    {
        var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart");
        if (cart == null) cart = new List<CartItem>();

        var itemInCart = cart.Where(ci => ci.ProductID == productID).FirstOrDefault();
        if (itemInCart != null)
        {
            itemInCart.Quantity += quantity;
        }
        else
        {
            cart.Add(new CartItem { ProductID = productID, Quantity = quantity });
        }
        HttpContext.Session.SetObject("Cart", cart);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Remove(int id)
    {
        var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart");
        if (cart == null || !cart.Any()) { return RedirectToAction(nameof(Index)); }

        var removedItem = cart.FirstOrDefault(ci => ci.ProductID == id);

        if (removedItem == null) {  return RedirectToAction(nameof(Index)); }

        cart.Remove(removedItem);

        HttpContext.Session.SetObject("Cart", cart);

        return RedirectToAction(nameof(Index));
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Clear()
    {
        HttpContext.Session.Clear();
        return RedirectToAction(nameof(Index));
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Checkout()
    {
        var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart");

        if (cart == null || !cart.Any())
        {
            return RedirectToAction(nameof(Index));
        }

        var orderItemsIds = cart.Select(ci => ci.ProductID).ToList();
        var orderItems = await _context.Product.Where(p => orderItemsIds.Contains(p.ID)).ToListAsync();
        orderItems.OrderBy(i => i.ID);
        var orderItemsQuantities = cart.OrderBy(c => c.ProductID).Select(c => c.Quantity).ToList();
        
        var totalPrice = 0M;

        for (int i = 0; i < orderItems.Count; i++)
        {
            totalPrice += orderItems[i].Price * orderItemsQuantities[i];
        }
        var newOrder = new Order { OrderDate = DateTime.Now, Status = OrderStatus.InProgress, TotalPrice = totalPrice };
        _context.Order.Add(newOrder);
        _context.SaveChanges();



        foreach (var item in cart)
        {
            var orderItem = new OrderProduct { ProductID = item.ProductID, OrderID = newOrder.ID, Quantity = item.Quantity };
            _context.OrderProduct.Add(orderItem);
        }
        _context.SaveChanges();

        HttpContext.Session.Clear();

        return View();
    }

}
