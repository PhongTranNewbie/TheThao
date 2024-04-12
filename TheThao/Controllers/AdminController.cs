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


        // GET: /Admin/CreateEmployee
        [HttpGet]
        public IActionResult CreateEmployee()
        {
            // Kiểm tra xem admin đã đăng nhập hay chưa
            if (HttpContext.Session.GetString("AdminId") == null)
            {
                return RedirectToAction("AdminLogin");
            }

            return View();
        }

        // POST: /Admin/CreateEmployee
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateEmployee(User model)
        {
            // Kiểm tra xem admin đã đăng nhập hay chưa
            if (HttpContext.Session.GetString("AdminId") == null)
            {
                return RedirectToAction("AdminLogin");
            }

            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
                {
                    ViewBag.ErrorMessage = "Username and password are required.";
                    return View(model);
                }
                if (_context.Users.Any(u => u.Username == model.Username))
                {
                    ModelState.AddModelError("Username", "Username already exists.");
                }
                else
                {
                    model.Role = "Staff"; // Set role to "Staff" for employee
                    _context.Users.Add(model);
                    _context.SaveChanges();
                    return RedirectToAction("EmployeeDashboard");
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult EmployeeLogin()
        {
            // Kiểm tra nếu đã đăng nhập thì chuyển hướng đến trang dashboard
            if (HttpContext.Session.GetString("EmployeeId") != null)
            {
                return RedirectToAction("EmployeeDashboard");
            }
            return View();
        }

        [HttpPost]
        public IActionResult EmployeeLogin(User model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password && u.Role == "Staff");
                if (user != null)
                {
                    // Lưu Id của nhân viên vào Session khi đăng nhập thành công
                    HttpContext.Session.SetString("EmployeeId", user.Id.ToString());
                    return RedirectToAction("EmployeeDashboard");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password");
                }
            }
            return View(model);
        }

        // Trang dashboard cho nhân viên
        public IActionResult EmployeeDashboard()
        {
            // Kiểm tra xem nhân viên đã đăng nhập hay chưa
            if (HttpContext.Session.GetString("EmployeeId") == null)
            {
                return RedirectToAction("EmployeeLogin");
            }

            return View("EmployeeDashboard");

        }

        // Đăng xuất nhân viên
        public IActionResult EmployeeLogout()
        {
            // Xóa Session khi nhân viên đăng xuất
            HttpContext.Session.Remove("EmployeeId");
            return RedirectToAction("EmployeeLogin");
        }
        public IActionResult Employee()
        {
            // Lấy danh sách nhân viên và khách hàng từ cơ sở dữ liệu
            var employees = _context.Users.Where(u => u.Role == "Staff").ToList();
            

            // Truyền danh sách nhân viên và khách hàng đến view
            ViewBag.Employees = employees;
            

            return View();
        }

        public IActionResult Customer()
        {
            // Lấy danh sách nhân viên và khách hàng từ cơ sở dữ liệu
            
            var customers = _context.Users.Where(u => u.Role == "Customer").ToList();

            // Truyền danh sách nhân viên và khách hàng đến view
            
            ViewBag.Customers = customers;

            return View();
        }
    }
}
