using Microsoft.AspNetCore.Mvc;
using TheThao.Models;
using TheThao.Models.Authentication;

namespace TheThao.Controllers
{
    public class HomeController : Controller
    {
        private readonly TheThaoContext _context;

        public HomeController(TheThaoContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var products = _context.SanPhams.ToList();
            ViewBag.Products = products;
            ViewBag.IsLoggedIn = false;
            return View();
        }
        public IActionResult Category(int theLoaiId)
        {
            
            var products = _context.SanPhams.Where(p => p.TheLoaiId == theLoaiId).ToList();
            ViewBag.Products = products;
            return View();
        }


    }
}
