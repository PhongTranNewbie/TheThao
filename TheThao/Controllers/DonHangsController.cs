using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TheThao.Models;
using TheThao.ViewModels;

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
            decimal tongTien = 0;
            foreach (var chiTiet in chiTietDonHangs)
            {
                tongTien += chiTiet.Gia  * chiTiet.SoLuong ;
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
