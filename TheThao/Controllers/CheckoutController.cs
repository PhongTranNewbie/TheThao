using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheThao.Models;
using TheThao.Services;
using TheThao.ViewModels;

namespace TheThao.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly TheThaoContext _context;
        private readonly IEmailSender _emailSender;

        public CheckoutController(TheThaoContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
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
                    var orderCode = Guid.NewGuid().ToString();

                    // Tạo một đơn hàng mới
                    var orderItem = new DonHang();
                    orderItem.Id = orderCode;
                    orderItem.UserId = userId;
                    orderItem.TrangThai = 1;
                    orderItem.NgayTao = DateTime.UtcNow;
                    _context.Add(orderItem);
                    await _context.SaveChangesAsync(); // Lưu đơn hàng vào cơ sở dữ liệu

                    var cartItems = HttpContext.Session.Get<List<ShoppingCartItem>>("Cart") ?? new List<ShoppingCartItem>();
                    foreach (var cart in cartItems)
                    {
                        var orderdetails = new ChiTietDonHang();
                        orderdetails.MaSanPham = cart.SanPham.Id;
                        orderdetails.MaDonHang = orderCode;
                        orderdetails.Gia = cart.SanPham.Gia;
                        orderdetails.SoLuong = cart.Quantity;
                        _context.Add(orderdetails);
                    }

                    await _context.SaveChangesAsync(); // Lưu các chi tiết đơn hàng vào cơ sở dữ liệu
                    await _emailSender.SendEmailAsync(user.Email, "Đơn hàng mới", $"Đơn hàng của bạn có mã {orderCode} đã được tạo thành công.");
                    HttpContext.Session.Remove("Cart");
                    // Tính tổng tiền
                    decimal tongTien = _context.ChiTietDonHangs
                        .Where(ct => ct.MaDonHang == orderCode)
                        .Sum(ct => ct.Gia * ct.SoLuong);

                    var viewModel = new CheckoutSuccessViewModel
                    {
                        DonHang = orderItem,
                        ChiTietDonHangs = _context.ChiTietDonHangs
                            .Include(ct => ct.MaSanPhamNavigation)
                            .Where(ct => ct.MaDonHang == orderCode)
                            .ToList(),
                        TongTien = tongTien // Gán giá trị tổng tiền vào ViewModel
                    };

                    return View(viewModel);

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
