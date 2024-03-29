using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheThao.Models;

namespace TheThao.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly TheThaoContext _context;

        public CheckoutController(TheThaoContext context)
        {
            _context = context;
            

        }
        public async Task<IActionResult> Checkout()
        {
            // Lấy userId từ Session
            var userIdString = HttpContext.Session.GetString("UserId");

            // Kiểm tra xem userId có giá trị không và có thể chuyển đổi thành int không
            if (!string.IsNullOrEmpty(userIdString) && int.TryParse(userIdString, out int userId))
            {
                // Làm bất cứ điều gì bạn muốn với userId ở đây

                // Kiểm tra email của người dùng
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null || string.IsNullOrEmpty(user.Email))
                {
                    // Nếu email của người dùng là null hoặc rỗng, chuyển hướng đến trang đăng nhập
                    return RedirectToAction("Login", "Login");
                }
                else
                {
                    // Tạo mã đơn hàng mới
                    var orderCode = Guid.NewGuid().ToString();

                    // Tạo một đơn hàng mới
                    var orderItem = new DonHang();
                   orderItem.Id = orderCode;
                    orderItem.UserId = userId;
                    orderItem.TrangThai = 1;
                    orderItem.NgayTao = DateTime.Now;
                    _context.Add(orderItem);
                    await _context.SaveChangesAsync();
                    
                    var cartItems = HttpContext.Session.Get<List<ShoppingCartItem>>("Cart") ?? new List<ShoppingCartItem>();
                    foreach(var cart in cartItems)
                    {
                        var orderdetails = new ChiTietDonHang();
                        orderdetails.MaSanPham = cart.SanPham.Id;
                        orderdetails.MaDonHang = orderCode;
                        orderdetails.Gia = cart.SanPham.Gia;
                        orderdetails.SoLuong = cart.Quantity;
                        _context.Add(orderdetails);
                        _context.SaveChangesAsync();

                    }
                    TempData["success"] = "Checkout thành công, vui lòng chờ duyệt đơn hàng";
                    // Return view checkout với đối tượng đơn hàng
                    return RedirectToAction("ViewCart", "Cart");

                }
            }
            else
            {
                // Nếu userId không hợp lệ hoặc không tồn tại, chuyển hướng đến trang đăng nhập
                return RedirectToAction("Login", "Login");
            }
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
