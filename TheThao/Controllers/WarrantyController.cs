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
                var baohanhs = _context.BaoHanhs.Include(x => x.MaSanPhamNavigation).Where(x => x.UserId == userId).ToList();
                return View(baohanhs);
            }
            return RedirectToAction("Login","Login");
		}

        [HttpGet]
        public IActionResult Create()
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            int userId;
            if (!string.IsNullOrEmpty(userIdString) && int.TryParse(userIdString, out userId))
            {
                // Lấy danh sách các đơn hàng đã thanh toán của người dùng hiện tại
                var orders = _context.DonHangs
                    .Where(x => x.UserId == userId && x.TrangThai == 3)
                    .ToList();

                // Tạo danh sách sản phẩm từ các đơn hàng đã thanh toán
                var products = new List<ChiTietDonHang>();
                foreach (var order in orders)
                {
                    var date = order.NgayTao;
                    var orderProducts = _context.ChiTietDonHangs
                        .Include(x => x.MaSanPhamNavigation)
                        .Where(x => x.MaDonHang == order.Id)
                        .ToList();

                    // Loại bỏ các sản phẩm đã được bảo hành
                    // Loại bỏ các sản phẩm đã được bảo hành và chỉ giữ lại các sản phẩm còn thời gian bảo hành
                    orderProducts = orderProducts.Where(p =>
                        !_context.BaoHanhs.Any(b => b.MaSanPham == p.MaSanPham) && // Chưa được bảo hành
                        (DateTime.Now < (order.NgayTao ?? DateTime.Now).AddYears((int)p.MaSanPhamNavigation.NamBaoHanh))
                    ).ToList();
                    products.AddRange(orderProducts);
                }

                return View(products);
            }

            return View(new List<ChiTietDonHang>()); // Trả về view với danh sách rỗng nếu không tìm thấy UserId trong session
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int[] selectedProducts)
        {
            if (selectedProducts == null || selectedProducts.Length == 0)
            {
                ModelState.AddModelError(string.Empty, "Vui lòng chọn ít nhất một sản phẩm để tạo bảo hành.");
                return RedirectToAction(nameof(Create));
            }

            var userIdString = HttpContext.Session.GetString("UserId");
            int userId;
            if (!string.IsNullOrEmpty(userIdString) && int.TryParse(userIdString, out userId))
            {
                // Lấy danh sách các đơn hàng đã thanh toán của người dùng hiện tại
                var orders = _context.DonHangs
                    .Where(x => x.UserId == userId && x.TrangThai == 3)
                    .ToList();

                foreach (var productId in selectedProducts)
                {
                    var productExistsInOrder = false;
                    foreach (var order in orders)
                    {
                        // Kiểm tra xem sản phẩm được chọn có thuộc đơn hàng đã thanh toán của người dùng hay không
                        var orderDetail = _context.ChiTietDonHangs.Where( x=> x.MaDonHang == order.Id && x.MaSanPham == productId).FirstOrDefault();
                        if (orderDetail != null)
                        {
                            productExistsInOrder = true;
                            var baoHanh = new BaoHanh
                            {
                                MaSanPham = productId,
                                UserId = userId,
                                MaDonHang = orderDetail.Id,
                                NgayBaoHanh = DateTime.Now,
                                NgayKetThucBaoHanh = DateTime.Now.AddMonths(1),
                                TrangThai = "Đang bảo hành"
                            };
                            _context.BaoHanhs.Add(baoHanh);
                            break; // Thoát khỏi vòng lặp nếu tìm thấy sản phẩm trong đơn hàng
                        }
                    }

                    if (!productExistsInOrder)
                    {
                        ModelState.AddModelError(string.Empty, $"Sản phẩm có ID {productId} không thuộc đơn hàng đã thanh toán của bạn.");
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
            var wr = _context.BaoHanhs.Include(x=>x.MaSanPhamNavigation).OrderByDescending(x=>x.Id).ToList();
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



        [HttpGet]
        public IActionResult BaoHanh(string id)
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            int userId;
            if (!string.IsNullOrEmpty(userIdString) && int.TryParse(userIdString, out userId))
            {
                // Lấy duy nhất đơn hàng đã thanh toán của người dùng hiện tại
                var order = _context.DonHangs
                    .FirstOrDefault(x => x.UserId == userId && x.TrangThai == 3 && x.Id == id);

                if (order != null)
                {
                    // Tạo danh sách chi tiết đơn hàng từ đơn hàng đã thanh toán
                    var products = _context.ChiTietDonHangs
                        .Include(x => x.MaSanPhamNavigation)
                        .Where(x => x.MaDonHang == order.Id)
                        .ToList();

                    // Loại bỏ các sản phẩm đã được bảo hành và chỉ giữ lại các sản phẩm còn thời gian bảo hành
                    products = products.Where(p =>
                        !_context.BaoHanhs.Any(b => b.MaSanPham == p.MaSanPham) && // Chưa được bảo hành
                        (DateTime.Now < (order.NgayTao ?? DateTime.Now).AddYears((int)p.MaSanPhamNavigation.NamBaoHanh))
                    ).ToList();

                    return View(products);
                }
            }

            return View(new List<ChiTietDonHang>()); // Trả về view với danh sách rỗng nếu không tìm thấy UserId trong session hoặc không tìm thấy đơn hàng
        }

    }
}
