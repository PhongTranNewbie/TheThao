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
	public IActionResult AddToCart(int id, int quantityToAdd)
	{
		var sanphamToAdd = _context.SanPhams.Find(id);
		var cartItems = HttpContext.Session.Get<List<ShoppingCartItem>>("Cart") ?? new List<ShoppingCartItem>();
		var existingCartItem = cartItems.FirstOrDefault(item => item.SanPham.Id == id);

		if (sanphamToAdd == null)
		{
			// Xử lý khi không tìm thấy sản phẩm với id cung cấp
			TempData["Message"] = "Sản phẩm không tồn tại.";
			return RedirectToAction("ViewCart");
		}

		if (existingCartItem != null && sanphamToAdd.SoLuong > 0)
		{
			// Nếu sản phẩm đã tồn tại trong giỏ hàng, tăng số lượng của nó lên số lượng đã chọn
			existingCartItem.Quantity += quantityToAdd;
		}
		else if (existingCartItem != null && sanphamToAdd.SoLuong <= 0 || existingCartItem == null && sanphamToAdd.SoLuong <= 0)
		{
			TempData["error"] = "Sản phẩm này đã hết hàng.";
		}
		else
		{
			// Thêm một đối tượng mới vào giỏ hàng với số lượng đã chọn
			cartItems.Add(new ShoppingCartItem
			{
				SanPham = sanphamToAdd,
				Quantity = quantityToAdd
			});
		}

		HttpContext.Session.Set("Cart", cartItems);
		TempData["success"] = $"{sanphamToAdd.Name} {quantityToAdd} added to Cart";
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
            
                
                cartItems.Remove(itemToRemove);
            
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
            if (itemToIncrease.Quantity >= itemToIncrease.SanPham.SoLuong)
            {
                // Nếu số lượng đã đạt tối đa, hiển thị thông báo và không thực hiện thêm nữa
                TempData["Message"] = "Sản phẩm này đã hết hàng.";
            }
            else
            {
                ++itemToIncrease.Quantity;
                HttpContext.Session.Set("Cart", cartItems);
                return RedirectToAction("ViewCart");
            }
        }
        else
        {
            // Xử lý khi không tìm thấy sản phẩm trong giỏ hàng
            TempData["Message"] = "Không tìm thấy sản phẩm trong giỏ hàng.";
        }
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
