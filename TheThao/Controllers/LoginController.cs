using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheThao.Models;


namespace TheThao.Controllers
{
    public class LoginController : Controller
        {
            private readonly TheThaoContext _context;
        private IHttpContextAccessor _httpContextAccessor;

        public LoginController(TheThaoContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
            public IActionResult Login()
            {
                return View();
            }

            [HttpPost]
        // Trong phương thức Login của LoginController
        public IActionResult Login(User model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);
                if (user != null)
                {
                    // Đăng nhập thành công, lấy UserId
                    HttpContext.Session.SetString("UserId", user.Id.ToString());
                    HttpContext.Session.SetString("Role", user.Role.ToString());
                    HttpContext.Session.SetInt32("UserId_Int", user.Id);
                    // Tạo một instance của CartController
                    var cartController = new CartController(_context, _httpContextAccessor);

                    // Lấy UserId từ session
                    var userId = HttpContext.Session.GetString("UserId");

                   
                    ViewBag.IsLoggedIn = true;
					return RedirectToAction("ToggleFavorite", "Favorite");

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
                        var circles = _context.Circles.ToList();
                        ViewBag.CircleImages = circles;
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
                            user.Email = model.Email;
                            user.Phone = model.Phone;
                            _context.SaveChanges();
                            return RedirectToAction("Profile");
                        }
                    }
                }
                return View(model);
            }

        
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            ViewBag.IsLoggedIn = false;

			return RedirectToAction("Index", "Home");
		}

        [HttpGet]
        public IActionResult DiaChi()
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (!string.IsNullOrEmpty(userIdString) && int.TryParse(userIdString, out int userId))
            {
                var user = _context.Users
                                   .Include(u => u.SoDiaChis)
                                   .FirstOrDefault(u => u.Id == userId);
				if (user != null)
				{
					// Lấy lại dữ liệu từ cơ sở dữ liệu
					user.SoDiaChis = _context.SoDiaChis.Where(a => a.UserId == userId).ToList();

					return View(user);
				}
			}
            return RedirectToAction("Login", "Login"); // Sửa lại controller nếu cần
        }

        [HttpPost]
        public IActionResult AddDiaChi([FromBody] SoDiaChi newAddress)
        {
            var userIdString = HttpContext.Session.GetString("UserId");

            if (!string.IsNullOrEmpty(userIdString) && int.TryParse(userIdString, out int userId))
            {
                newAddress.UserId = userId;

                var user = _context.Users
                    .Include(u => u.SoDiaChis)
                    .FirstOrDefault(u => u.Id == userId);

                if (user != null)
                {
                    

                    _context.SoDiaChis.Add(newAddress);
                    _context.SaveChanges();
                    // Chờ 100 milliseconds trước khi chuyển hướng đến trang mới
                    System.Threading.Thread.Sleep(100);
                    return RedirectToAction(nameof(DiaChi));
				}
			}

            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public IActionResult SetDefaultDiaChi(int addressId)
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (!string.IsNullOrEmpty(userIdString) && int.TryParse(userIdString, out int userId))
            {
                var user = _context.Users
                                   .Include(u => u.SoDiaChis)
                                   .FirstOrDefault(u => u.Id == userId);

                if (user != null)
                {
                    
                    _context.SaveChanges();
                }
                return RedirectToAction("DiaChi");
            }
            return RedirectToAction("Login", "Login");
        }



    }
}
