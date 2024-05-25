using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TheThao.Models;

namespace TheThao.Controllers
{
    public class BlogController : Controller
    {
        private readonly TheThaoContext _context;
        public BlogController(TheThaoContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return _context.Blogs != null ?
                        View(await _context.Blogs.ToListAsync()) :
                        Problem("Entity set 'TheThaoContext.Blogs'  is null.");
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Blog blog)
        {
            _context.Add(blog);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = _context.Blogs.FirstOrDefault(m => m.Id == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = _context.Blogs.Find(id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Title,NoiDung")] Blog blog)
        {
            if (id != blog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(blog);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(blog);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = _context.Blogs.FirstOrDefault(m => m.Id == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var blog = _context.Blogs.Find(id);
            _context.Blogs.Remove(blog);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult TrangChu()
        {
            var blog = _context.Blogs.ToList();
			var circles = _context.Circles.ToList();
			ViewBag.CircleImages = circles;
			return View(blog);
        }

        public IActionResult ChiTiet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = _context.Blogs.FirstOrDefault(m => m.Id == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        private int? GetCurrentUserId()
        {
            var userIdString = HttpContext.Session.GetString("UserId");

            if (userIdString == null || !int.TryParse(userIdString, out int userId))
            {
                // Hoặc thay đổi cách lấy ID của người dùng dựa trên hệ thống xác thực bạn đang sử dụng
                // Ví dụ: ASP.NET Core Identity
                // var user = await _userManager.GetUserAsync(User);
                // var userId = user?.Id;
                return null;
            }

            return userId;
        }
    }
}


