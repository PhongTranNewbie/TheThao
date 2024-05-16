using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheThao.Models;

namespace TheThao.Controllers
{
	public class WarrantyController : Controller
	{
		private readonly TheThaoContext _context;

		public WarrantyController(TheThaoContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
            var userIdString = HttpContext.Session.GetString("UserId");
            int userId;
            if (!string.IsNullOrEmpty(userIdString) && int.TryParse(userIdString, out userId))
            {
                var baohanhs = _context.BaoHanhs.Include(x => x.MaSerialNavigation.Serial.MaSanPhamNavigation).Where(x => x.UserId == userId).ToList();
                return View(baohanhs);
            }
            return RedirectToAction("Login","Login");
		}

        [HttpGet]
        public IActionResult Create(string id = null)
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            int userId;
            if (!string.IsNullOrEmpty(userIdString) && int.TryParse(userIdString, out userId))
            {
                // Lấy danh sách các đơn hàng đã thanh toán của người dùng hiện tại
                List<DonHang> orders;
                if (id == null)
                {
                    orders = _context.DonHangs
                        .Where(x => x.UserId == userId && x.TrangThai == 3)
                        .ToList();
                }
                else
                {
                    var order = _context.DonHangs
                        .FirstOrDefault(x => x.UserId == userId && x.TrangThai == 3 && x.Id == id);

                    orders = order != null ? new List<DonHang> { order } : new List<DonHang>();
                }

                // Tạo danh sách sản phẩm từ các đơn hàng đã thanh toán
                var orderWithProducts = new List<(DonHang Order, List<QuanLySerial> Products)>();
                foreach (var order in orders)
                {
                    var orderDetails = _context.ChiTietDonHangs
                        .Include(x => x.MaSanPhamNavigation)
                        .Where(x => x.MaDonHang == order.Id)
                        .ToList();

                    // Lấy danh sách quản lý serial cho từng chi tiết đơn hàng
                    var serials = new List<QuanLySerial>();
                    foreach (var orderDetail in orderDetails)
                    {
                        var orderSerials = _context.QuanLySerials
                            .Include(x => x.Serial.MaSanPhamNavigation)
                            .Where(s => s.MaChiTietDonHang == orderDetail.Id)
                            .ToList();
                        serials.AddRange(orderSerials);
                    }

                    // Lọc ra các serial chưa được bảo hành và vẫn còn thời gian bảo hành
                    var filteredSerials = serials.Where(s => !_context.BaoHanhs.Any(b => b.MaSerial == s.SerialId && b.UserId == userId && b.TrangThai == "In warranty") &&
                                          DateTime.Now < (order.NgayTao ?? DateTime.Now).AddYears((int)s.Serial.MaSanPhamNavigation.NamBaoHanh))
                               .ToList();


                    // Thêm dữ liệu vào orderWithProducts
                    orderWithProducts.Add((order, filteredSerials));
                }


                return View(orderWithProducts);
            }

            return View(new List<(DonHang, List<QuanLySerial>)>()); // Trả về view với danh sách rỗng nếu không tìm thấy UserId trong session
        }








        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Dictionary<string, Dictionary<int, string>> requests, List<int> selectedProducts)
        {
            if (requests == null || requests.Count == 0 || selectedProducts == null || selectedProducts.Count == 0)
            {
                ModelState.AddModelError(string.Empty, "Vui lòng chọn ít nhất một sản phẩm để tạo bảo hành.");
                return RedirectToAction(nameof(Create));
            }

            var userIdString = HttpContext.Session.GetString("UserId");
            int userId;
            if (!string.IsNullOrEmpty(userIdString) && int.TryParse(userIdString, out userId))
            {
                foreach (var orderId in requests.Keys)
                {
                    var order = _context.DonHangs.FirstOrDefault(x => x.Id == orderId && x.UserId == userId && x.TrangThai == 3);
                    if (order == null)
                    {
                        // Xử lý lỗi khi không tìm thấy đơn hàng hoặc đơn hàng không thuộc về người dùng
                        return BadRequest("Đơn hàng không hợp lệ.");
                    }

                    foreach (var productIdStr in requests[orderId].Keys)
                    {
                        

                        if (!selectedProducts.Contains(productIdStr))
                        {
                            // Bỏ qua sản phẩm không được chọn
                            continue;
                        }

                        var productRequest = requests[orderId][productIdStr];

                        var baoHanh = new BaoHanh
                        {
                            MaSerial = productIdStr,
                            UserId = userId,
                            NgayBaoHanh = DateTime.Now,
                            NgayKetThucBaoHanh = DateTime.Now.AddMonths(1),
                            TrangThai = "In Warranty",
                            LyDo = productRequest
                        };

                        _context.BaoHanhs.Add(baoHanh);
                    }
                }

                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // Không tìm thấy UserId trong session
                return RedirectToAction("Login", "Login"); // Chuyển hướng đến trang đăng nhập nếu không có UserId
            }
        }












        public IActionResult AdminIndex()
        {
            var wr = _context.BaoHanhs.Include(x=>x.MaSerialNavigation).OrderByDescending(x=>x.Id).ToList();
            return View(wr);
        }
        [HttpGet]
        public IActionResult UpdateWarrantyStatus(int warrantyId)
        {
            // Tìm bảo hành cần chỉnh sửa trong cơ sở dữ liệu
            var warranty = _context.BaoHanhs.FirstOrDefault(w => w.Id == warrantyId);

            if (warranty != null)
            {
                return View(warranty); // Trả về view chỉnh sửa trạng thái bảo hành với dữ liệu của bảo hành cần chỉnh sửa
            }

            // Trả về một trang hoặc thông báo lỗi nếu không tìm thấy bảo hành
            return View("Error");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateWarrantyStatus(BaoHanh baoHanh)
        {
            // Tìm bảo hành cần chỉnh sửa trong cơ sở dữ liệu
            var warranty = _context.BaoHanhs.FirstOrDefault(w => w.Id == baoHanh.Id);

            if (warranty != null)
            {
                warranty.TrangThai = baoHanh.TrangThai; // Update the warranty status
                _context.SaveChanges(); // Save changes to the database

                return RedirectToAction(nameof(Index)); // Redirect to the warranty list page after updating
            }

            // Return an error page or message if the warranty is not found
            return View("Error");
        }



        

    }
}
