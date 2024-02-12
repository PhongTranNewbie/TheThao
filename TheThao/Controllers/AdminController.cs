// AdminController
using Microsoft.AspNetCore.Mvc;
using TheThao.Data;
using TheThao.Models;
using TheThao.Services;

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
        return View();
    }

    [HttpPost]
    public IActionResult AdminLogin(User model)
    {
        if (ModelState.IsValid)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);
            if (user != null && user.Role == "admin")
            {
                // Đăng nhập thành công, chuyển hướng đến trang admin dashboard
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
        // Lấy danh sách khách hàng từ cơ sở dữ liệu hoặc từ dịch vụ phù hợp
        var customers = _customerService.GetAllCustomers();

        // Truyền danh sách khách hàng vào view
        return View(customers);
    }

    public List<User> GetAllCustomers()
    {
        // Lấy danh sách khách hàng từ cơ sở dữ liệu và trả về
        return _context.Users.ToList();
    }

}
