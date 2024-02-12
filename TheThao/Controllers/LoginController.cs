using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheThao.Data;
using TheThao.Models;

namespace TheThao.Controllers
{
    public class LoginController : Controller
    {

        private readonly TheThaoContext _context;

        public LoginController(TheThaoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);
                if (user != null)
                {
                    // Tạo session hoặc lưu thông tin người dùng vào cookie
                    HttpContext.Session.SetString("UserId", user.Id.ToString());

                    // Thực hiện đăng nhập thành công và chuyển hướng đến trang Profile
                    return RedirectToAction("Profile");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password");
                }
            }
            return RedirectToAction("Profile");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User model)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
                {
                    ViewBag.ErrorMessage = "Username and password are required.";
                    return View(model);
                }
                // Kiểm tra xem người dùng đã tồn tại chưa
                if (_context.Users.Any(u => u.Username == model.Username))
                {
                    ModelState.AddModelError("Username", "Username already exists.");
                }
                else
                {
                    // Lưu người dùng mới vào cơ sở dữ liệu
                    model.Role = "Customer";
                    _context.Users.Add(model);
                    _context.SaveChanges();

                    // Thực hiện đăng ký thành công, có thể thực hiện hành động khác (ví dụ: đăng nhập)
                    return RedirectToAction("Login");
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Profile()
        {
            // Kiểm tra xem người dùng đã đăng nhập chưa
            var userId = HttpContext.Session.GetString("UserId");
            if (userId != null)
            {
                // Lấy thông tin người dùng từ cơ sở dữ liệu dựa trên userId
                var user = _context.Users.FirstOrDefault(u => u.Id == int.Parse(userId));
                if (user != null)
                {
                    // Hiển thị trang Profile và truyền thông tin người dùng qua model
                    return View(user);
                }
            }

            // Nếu người dùng chưa đăng nhập, chuyển hướng đến trang đăng nhập
            return RedirectToAction("Login");
        }

        [HttpPost]
        public IActionResult Profile(User model)
        {
            if (ModelState.IsValid)
            {
                // Lấy userId từ session
                var userId = HttpContext.Session.GetString("UserId");
                if (userId != null)
                {
                    // Lấy thông tin người dùng từ cơ sở dữ liệu
                    var user = _context.Users.FirstOrDefault(u => u.Id == int.Parse(userId));
                    if (user != null)
                    {
                        // Cập nhật thông tin người dùng
                        user.Ten = model.Ten;
                        user.NgayThangNamSinh = model.NgayThangNamSinh;
                        user.DiaChi = model.DiaChi;
                        user.Email = model.Email;
                        user.Phone = model.Phone;

                        // Lưu thay đổi vào cơ sở dữ liệu
                        _context.SaveChanges();

                        // Chuyển hướng đến trang chủ sau khi hoàn thành
                        return View("Client");
                    }
                }
            }

            // Nếu có lỗi xảy ra hoặc người dùng chưa đăng nhập, hiển thị lại trang Profile
            return RedirectToAction("Client");
        }

        public IActionResult Client()
        {
            var products = _context.SanPhams.ToList();
            ViewBag.Products = products;
            return View();
        }
    }
}
