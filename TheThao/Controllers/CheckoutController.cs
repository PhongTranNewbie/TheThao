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

		public async Task<IActionResult> Checkout(string paymentSelection)
		{
			// Lấy userId từ Session
			var userIdString = HttpContext.Session.GetString("UserId");
			var userIdStringAd = _context.Users.FirstOrDefault(u => u.Role == "admin");
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
					orderItem.PhuongPhapThanhToan = paymentSelection;
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
                        var sp = _context.SanPhams.FirstOrDefault(x => x.Id == cart.SanPham.Id);
						sp.SoLuong -= cart.Quantity;
						sp.SoLuongDaBan += cart.Quantity;
						if(sp.SoLuong <= 0)
						{
							sp.TrangThai = "Out of Stock";
						}
                        _context.Add(orderdetails);
                        await _context.SaveChangesAsync(); // Lưu đơn hàng vào cơ sở dữ liệu

                        // Tạo mục QuanLySerial và cập nhật trạng thái trong Kho
                        var khoItems = _context.Khos
                            .Where(k => k.MaSanPham == cart.SanPham.Id && k.TrangThai != "Sold")
                            .OrderBy(k => Guid.NewGuid()) // Sắp xếp ngẫu nhiên
                            .Take(cart.Quantity)
                            .ToList();

                        if (khoItems.Count < cart.Quantity)
                        {
                            // Xử lý trường hợp không đủ số lượng trong kho
                            // Có thể trả về thông báo lỗi hoặc thông báo cho người dùng
                            return BadRequest("Không đủ số lượng sản phẩm trong kho.");
                        }

                        foreach (var khoItem in khoItems)
                        {
                            khoItem.TrangThai = "Sold";
                            var quanLySerial = new QuanLySerial
                            {
                                MaChiTietDonHang = orderdetails.Id,
                                SerialId = khoItem.Serial,
                            };
                            _context.Add(quanLySerial);
                        }
                    }
					
					await _context.SaveChangesAsync(); // Lưu các chi tiết đơn hàng vào cơ sở dữ liệu
					await _emailSender.SendEmailAsync(user.Email, "Đơn hàng mới", $"Đơn hàng của bạn có mã {orderCode} đã được tạo thành công.");
					if (userIdStringAd != null)
					{
						await _emailSender.SendEmailAsync(user.Email, "Đơn hàng mới", $"Đơn hàng mới có mã {orderCode}");
					}

					HttpContext.Session.Remove("Cart");
					// Tính tổng tiền
					decimal? tongTien = _context.ChiTietDonHangs
						.Where(ct => ct.MaDonHang == orderCode)
						.Sum(ct => ct.Gia * ct.SoLuong);

					var viewModel = new CheckoutSuccessViewModel
					{
						DonHang = orderItem,
						ChiTietDonHangs = _context.ChiTietDonHangs
							.Include(ct => ct.MaSanPhamNavigation)
							.Where(ct => ct.MaDonHang == orderCode)
							.ToList(),
						TongTien = (decimal)tongTien // Gán giá trị tổng tiền vào ViewModel
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
