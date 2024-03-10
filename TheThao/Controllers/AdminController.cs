using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;
using TheThao.Models;
using TheThao.Services;

namespace TheThao.Controllers
{
    public class AdminController : Controller
    {
        private readonly TheThaoContext _context;
        private readonly ICustomerService _customerService;

        public AdminController(TheThaoContext context, ICustomerService customerService)
        {
            _context = context;
            _customerService = customerService;
        }

        [HttpGet]
        public IActionResult AdminLogin()
        {
            // Kiểm tra nếu đã đăng nhập thì chuyển hướng đến trang dashboard
            if (HttpContext.Session.GetString("AdminId") != null)
            {
                return RedirectToAction("AdminDashboard");
            }
            return View();
        }

        [HttpPost]
        public IActionResult AdminLogin(User model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password && u.Role == "admin");
                if (user != null)
                {
                    // Lưu Id của admin vào Session khi đăng nhập thành công
                    HttpContext.Session.SetString("AdminId", user.Id.ToString());
                    return RedirectToAction("AdminDashboard");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password");
                }
            }
            return View(model);
        }

        // Trang dashboard cho admin
        public IActionResult AdminDashboard()
        {
            // Kiểm tra xem admin đã đăng nhập hay chưa
            if (HttpContext.Session.GetString("AdminId") == null)
            {
                return RedirectToAction("AdminLogin");
            }

            // Lấy danh sách khách hàng từ cơ sở dữ liệu hoặc từ dịch vụ phù hợp
            //var customers = _customerService.GetAllCustomers();

            // Truyền danh sách khách hàng vào view
            // return View(customers);
            return View();
        }

        // Đăng xuất
        public IActionResult AdminLogout()
        {
            // Xóa Session khi admin đăng xuất
            HttpContext.Session.Remove("AdminId");
            return RedirectToAction("AdminLogin");
        }

        public IActionResult Profile()
        {
            var adminId = HttpContext.Session.GetString("AdminId");

            if (!string.IsNullOrEmpty(adminId))
            {
                var user = _context.Users.FirstOrDefault(u => u.Id == int.Parse(adminId));
                if (user != null)
                {
                    return View(user);
                }
            }

            return RedirectToAction("AdminLogin");
        }


        // Các action khác
    }
}
