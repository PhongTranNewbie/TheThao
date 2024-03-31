using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheThao.Models;
using Microsoft.AspNetCore.Http;
public class CartController : Controller
{
    private readonly TheThaoContext _context;
    private readonly List<ShoppingCartItem> _cartItems;

    public CartController(TheThaoContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _cartItems = new List<ShoppingCartItem>();

    }
    public IActionResult Index()
    {
        return View();
    }
    public IActionResult AddToCart(int id)
    {
        var sanphamToAdd = _context.SanPhams.Find(id);
        var cartItems = HttpContext.Session.Get<List<ShoppingCartItem>>("Cart") ?? new List<ShoppingCartItem>();
        var existingCartItem = cartItems.FirstOrDefault(item => item.SanPham.Id==id);
        if (existingCartItem != null)
        {
            existingCartItem.Quantity++;
        }
        else
        {
            cartItems.Add(new ShoppingCartItem
            {
                SanPham = sanphamToAdd,
                Quantity = 1
            });
        }
        HttpContext.Session.Set("Cart", cartItems);
        TempData["CartMessage"] = $"{sanphamToAdd.Name} ${sanphamToAdd.TheLoai} added to Cart";
        return RedirectToAction("ViewCart");
    }
    [HttpGet]
    public IActionResult ViewCart()
    {

        var cartItems = HttpContext.Session.Get<List<ShoppingCartItem>>("Cart") ?? new List<ShoppingCartItem>();
        var cartViewModel = new ShoppingCartViewModel
        {
            CartItems = cartItems,
            TotalPrice = cartItems.Sum(item => item.SanPham.Gia * item.Quantity)
        };
        ViewBag.SanPham = _context.SanPhams.ToList();
        ViewBag.CartMessage = TempData["CartMessage"];
        return View(cartViewModel);
    }
    public IActionResult RemoveItem(int id)
    {
        var cartItems = HttpContext.Session.Get<List<ShoppingCartItem>>("Cart") ?? new List<ShoppingCartItem>();
        var itemToRemove = cartItems.FirstOrDefault(item => item.SanPham.Id == id);
        TempData["CartMessage"] = $"{itemToRemove.SanPham.Name} ${itemToRemove.SanPham.TheLoai} Item Removed From Cart";
        if (itemToRemove != null)
        {
            if(itemToRemove.Quantity > 1)
            {
                itemToRemove.Quantity--;
            }
            else
            {
                
                cartItems.Remove(itemToRemove);
            }
        }
        HttpContext.Session.Set("Cart", cartItems);
        
        return RedirectToAction("ViewCart");
    }
    public IActionResult IncreaseQuantity(int id)
    {
        var cartItems = HttpContext.Session.Get<List<ShoppingCartItem>>("Cart") ?? new List<ShoppingCartItem>();
        var itemToIncrease = cartItems.FirstOrDefault(item => item.SanPham.Id == id);
        if (itemToIncrease != null)
        {
            ++itemToIncrease.Quantity;
        }
        HttpContext.Session.Set("Cart", cartItems);
        return RedirectToAction("ViewCart");
    }

    public IActionResult DecreaseQuantity(int id)
    {
        var cartItems = HttpContext.Session.Get<List<ShoppingCartItem>>("Cart") ?? new List<ShoppingCartItem>();
        var itemToDecrease = cartItems.FirstOrDefault(item => item.SanPham.Id == id);
        if (itemToDecrease != null)
        {
            if (itemToDecrease.Quantity > 1)
            {
                --itemToDecrease.Quantity;
            }
            else
            {
                // Remove the item from the cart if its quantity is 1
                cartItems.Remove(itemToDecrease);
            }
            if (cartItems.Count == 0) 
            {
                HttpContext.Session.Remove("ViewCart");    
            }
        }
        HttpContext.Session.Set("Cart", cartItems);
        return RedirectToAction("ViewCart");
    }
    public IActionResult ClearCart()
    {
        HttpContext.Session.Remove("Cart");
        return RedirectToAction("ViewCart");
    }


}
