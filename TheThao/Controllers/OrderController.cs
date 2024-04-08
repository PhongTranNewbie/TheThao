using Microsoft.AspNetCore.Mvc;
using TheThao.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TheThao.ViewModels;

namespace TheThao.Controllers
{
    public class OrderController : Controller
    {
        private readonly TheThaoContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderController(TheThaoContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult ListOrders()
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            int userId;
            if (!string.IsNullOrEmpty(userIdString) && int.TryParse(userIdString, out userId))
            {
                // Lấy danh sách đơn hàng của khách hàng có userId
                var orders = _context.DonHangs
                                    .Where(o => o.UserId == userId)
                                    .ToList();

                return View(orders);
            }
            else
            {
                // Xử lý trường hợp không tìm thấy userId hoặc userId không hợp lệ
                // Ví dụ: có thể chuyển hướng đến trang lỗi hoặc trang đăng nhập
                return RedirectToAction("Login", "Account"); // Chuyển hướng đến trang đăng nhập
            }
        }
        public IActionResult Details(string id)
        {

            // Lấy thông tin đơn hàng từ database và gán vào model
            var donHang = _context.DonHangs.FirstOrDefault(d => d.Id == id);

            // Kiểm tra xem đơn hàng có tồn tại không
            if (donHang == null)
            {
                return NotFound();
            }

            // Lấy thông tin chi tiết đơn hàng từ database và gán vào
            var chiTietDonHangs = _context.ChiTietDonHangs
                                        .Include(ct => ct.MaSanPhamNavigation)
                                        .Where(ct => ct.MaDonHang == id)
                                        .ToList();

            // Tính tổng tiền
            decimal tongTien = 0;
            foreach (var chiTiet in chiTietDonHangs)
            {
                tongTien += chiTiet.Gia * chiTiet.SoLuong;
            }

            // Tạo view model và truyền dữ liệu đơn hàng và tổng tiền vào
            var viewModel = new ChiTietDonHangViewModel
            {
                DonHang = donHang,
                ChiTietDonHangs = chiTietDonHangs,
                TongTien = tongTien
            };

            // Trả về view và truyền view model
            return View(viewModel);
        }



    }
}
