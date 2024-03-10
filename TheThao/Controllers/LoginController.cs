using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                        // Đăng nhập thành công, tạo session hoặc lưu thông tin vào cookie
                        HttpContext.Session.SetString("UserId", user.Id.ToString());
                    ViewBag.IsLoggedIn = true;
                    return RedirectToAction("Profile");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid username or password");
                    }
                }
                return View(model);
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
                    if (_context.Users.Any(u => u.Username == model.Username))
                    {
                        ModelState.AddModelError("Username", "Username already exists.");
                    }
                    else
                    {
                        model.Role = "Customer";
                        _context.Users.Add(model);
                        _context.SaveChanges();
                        return RedirectToAction("Login");
                    }
                }
                return View(model);
            }

            [HttpGet]
            public IActionResult Profile()
            {
                var userId = HttpContext.Session.GetString("UserId");
                if (userId != null)
                {
                    var user = _context.Users.FirstOrDefault(u => u.Id == int.Parse(userId));
                    if (user != null)
                    {
                        return View(user);
                    }
                }
                return RedirectToAction("Login");
            }

            [HttpPost]
            public IActionResult Profile(User model)
            {
                if (ModelState.IsValid)
                {
                    var userId = HttpContext.Session.GetString("UserId");
                    if (userId != null)
                    {
                        var user = _context.Users.FirstOrDefault(u => u.Id == int.Parse(userId));
                        if (user != null)
                        {
                            user.Ten = model.Ten;
                            user.NgayThangNamSinh = model.NgayThangNamSinh;
                            user.DiaChi = model.DiaChi;
                            user.Email = model.Email;
                            user.Phone = model.Phone;
                            _context.SaveChanges();
                            return RedirectToAction("Client");
                        }
                    }
                }
                return View("Client");
            }

        
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            ViewBag.IsLoggedIn = false;

            return RedirectToAction("Login"); // Redirect to home or any other page after logout
        }


    }
}
