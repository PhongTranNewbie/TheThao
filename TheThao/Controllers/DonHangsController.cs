using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TheThao.Models;
using TheThao.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TheThao.Controllers
{
    public class DonHangsController : Controller
    {
        private readonly TheThaoContext _context;

        public DonHangsController(TheThaoContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Define a dictionary to map status values (int) to corresponding texts
            var statusTexts = new Dictionary<int, string>
            {
                { 1, "Đang chờ xác nhận" },
                { 2, "Đang giao hàng" },
                { 3, "Đã thanh toán" },
                { 4, "Đã thanh toán và chờ giao hàng" },
                { 5, "Đã hủy" }
            };

            // Pass the dictionary to the view
            ViewBag.StatusTexts = statusTexts;



            var donHangs = _context.DonHangs.ToList();
            return View(donHangs);
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
                tongTien += chiTiet.Gia  * chiTiet.SoLuong ;
            }

            // Tạo view model và truyền dữ liệu đơn hàng và tổng tiền vào
            var viewModel = new ChiTietDonHangViewModel
            {
                DonHang = donHang,
                ChiTietDonHangs = chiTietDonHangs,
                TongTien = (decimal)tongTien
            };
            ViewBag.User = _context.Users.ToList();
            // Trả về view và truyền view model
            return View(viewModel);
        }
        // GET: DonHangs/Edit/id
        public IActionResult Edit(string id)
        {
            var donHang = _context.DonHangs
                                    .Include(d => d.ChiTietDonHangs)
                                    .ThenInclude(ctdh => ctdh.MaSanPhamNavigation)
                                    .FirstOrDefault(d => d.Id == id);

            if (donHang == null)
            {
                return NotFound();
            }

            // Calculate total price
            decimal? tongTien = donHang.ChiTietDonHangs.Sum(ctdh => ctdh.Gia * ctdh.SoLuong);

            ViewBag.TongTien = tongTien; // Pass total price to view

            return View(donHang);
        }

        // POST: DonHangs/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(DonHang donHang)
        {
            if (!ModelState.IsValid)
            {
                // If the model is not valid, return the edit view with model errors
                return View(donHang);
            }

            var existingDonHang = _context.DonHangs.FirstOrDefault(d => d.Id == donHang.Id);

            if (existingDonHang == null)
            {
                return NotFound();
            }

            // Update order status
            existingDonHang.TrangThai = donHang.TrangThai;

            _context.SaveChanges();

            return RedirectToAction("Details", new { id = donHang.Id });
        }
    }
}
