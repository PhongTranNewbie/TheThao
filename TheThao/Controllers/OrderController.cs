using Microsoft.AspNetCore.Mvc;
using TheThao.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TheThao.ViewModels;
using System.Collections.Generic;
using TheThao.ViewModel;

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
            var statusTexts = new Dictionary<int, string>
    {
        { 1, "Waiting for confirmation" },
        { 2, "On delivery" },
        { 3, "Paid" },
        { 4, "Paid and awaiting delivery" },
        { 5, "Cancelled" }
    };

            // Pass the dictionary to the view
            ViewBag.StatusTexts = statusTexts;

            var userIdString = HttpContext.Session.GetString("UserId");
            int userId;
            if (!string.IsNullOrEmpty(userIdString) && int.TryParse(userIdString, out userId))
            {
                var waitingForConfirmationOrders = _context.DonHangs
                                                           .Where(o => o.UserId == userId && o.TrangThai == 1)
                                                           .OrderByDescending(x => x.NgayTao)
                                                           .ToList();

                var onDeliveryOrders = _context.DonHangs
                                                 .Where(o => o.UserId == userId && o.TrangThai == 2)
                                                 .OrderByDescending(x => x.NgayTao)
                                                 .ToList();

                var paidOrders = _context.DonHangs
                                             .Where(o => o.UserId == userId && o.TrangThai == 3)
                                             .OrderByDescending(x => x.NgayTao)
                                             .ToList();

                var paidAndAwaitingDeliveryOrders = _context.DonHangs
                                                                .Where(o => o.UserId == userId && o.TrangThai == 4)
                                                                .OrderByDescending(x => x.NgayTao)
                                                                .ToList();

                var cancelledOrders = _context.DonHangs
                                                 .Where(o => o.UserId == userId && o.TrangThai == 5)
                                                 .OrderByDescending(x => x.NgayTao)
                                                 .ToList();

                var viewModel = new OrdersByStatusViewModel
                {
                    WaitingForConfirmationOrders = waitingForConfirmationOrders,
                    OnDeliveryOrders = onDeliveryOrders,
                    PaidOrders = paidOrders,
                    PaidAndAwaitingDeliveryOrders = paidAndAwaitingDeliveryOrders,
                    CancelledOrders = cancelledOrders
                };

                return View(viewModel);
            }
            else
            {
                return RedirectToAction("Login", "Account");
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
            decimal? tongTien = 0;
            foreach (var chiTiet in chiTietDonHangs)
            {
                tongTien += chiTiet.Gia * chiTiet.SoLuong;
            }

            // Tạo view model và truyền dữ liệu đơn hàng và tổng tiền vào
            var viewModel = new ChiTietDonHangViewModel
            {
                DonHang = donHang,
                ChiTietDonHangs = chiTietDonHangs,
                TongTien = (decimal)tongTien
            };

            // Trả về view và truyền view model
            return View(viewModel);
        }
    }
}
