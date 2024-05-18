using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheThao.Models;
using TheThao.ViewModel;

namespace TheThao.Controllers
{
    public class GoiYController : Controller
    {
        private readonly TheThaoContext _context;

        public GoiYController(TheThaoContext context)
        {
            _context = context;
        }

        // GET: GoiY/Create
        public async Task<IActionResult> Create()
        {
            var sanPhams = await _context.SanPhams.ToListAsync();
            var model = new AddProductsToGoiYViewModel
            {
                SanPhams = sanPhams
            };
            return View(model);
        }

        // POST: GoiY/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string tenGoiY, List<int> maSanPhams)
        {
            if (string.IsNullOrEmpty(tenGoiY))
            {
                ModelState.AddModelError(string.Empty, "Vui lòng nhập tên gợi ý.");
                return View(new AddProductsToGoiYViewModel { SanPhams = await _context.SanPhams.ToListAsync() });
            }

            if (maSanPhams == null || !maSanPhams.Any())
            {
                ModelState.AddModelError(string.Empty, "Vui lòng chọn ít nhất một sản phẩm để thêm vào gợi ý.");
                return View(new AddProductsToGoiYViewModel { SanPhams = await _context.SanPhams.ToListAsync() });
            }

            var sanPhamGoiY = new SanPhamGoiY
            {
                Ten = tenGoiY
            };

            _context.SanPhamGoiYs.Add(sanPhamGoiY);
            await _context.SaveChangesAsync();

            foreach (var maSanPham in maSanPhams)
            {
                var chiTiet = new ChiTietSanPhamGoiY
                {
                    MaSanPham = maSanPham,
                    MaGoiY = sanPhamGoiY.Id
                };

                _context.ChiTietSanPhamGoiYs.Add(chiTiet);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: GoiY/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var sanPhamGoiY = await _context.SanPhamGoiYs.FindAsync(id);
            if (sanPhamGoiY == null)
            {
                return NotFound();
            }

            var sanPhams = await _context.SanPhams.ToListAsync();
            var model = new AddProductsToGoiYViewModel
            {
                MaGoiY = sanPhamGoiY.Id,
                TenGoiY = sanPhamGoiY.Ten,
                SanPhams = sanPhams
            };

            return View(model);
        }

        // POST: GoiY/AddProductsToGoiY
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProductsToGoiY(int maGoiY, List<int> maSanPhams)
        {
            if (maSanPhams == null || !maSanPhams.Any())
            {
                ModelState.AddModelError(string.Empty, "Vui lòng chọn ít nhất một sản phẩm để thêm vào gợi ý.");
                return RedirectToAction(nameof(Edit), new { id = maGoiY });
            }

            var sanPhamGoiY = await _context.SanPhamGoiYs.FindAsync(maGoiY);
            if (sanPhamGoiY == null)
            {
                ModelState.AddModelError(string.Empty, "Gợi ý không tồn tại.");
                return RedirectToAction(nameof(Create));
            }

            foreach (var maSanPham in maSanPhams)
            {
                var chiTietExist = await _context.ChiTietSanPhamGoiYs
                    .AnyAsync(x => x.MaSanPham == maSanPham && x.MaGoiY == maGoiY);

                if (chiTietExist)
                {
                    ModelState.AddModelError(string.Empty, $"Sản phẩm {maSanPham} đã tồn tại trong gợi ý.");
                    return RedirectToAction(nameof(Edit), new { id = maGoiY });
                }

                var chiTiet = new ChiTietSanPhamGoiY
                {
                    MaSanPham = maSanPham,
                    MaGoiY = maGoiY
                };

                _context.ChiTietSanPhamGoiYs.Add(chiTiet);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: GoiY/Index
        public async Task<IActionResult> Index()
        {
            var goiYs = await _context.SanPhamGoiYs
                .Include(g => g.ChiTietSanPhamGoiYs)
                .ThenInclude(ct => ct.MaSanPhamNavigation)
                .Select(g => new SanPhamGoiYViewModel
                {
                    MaGoiY = g.Id,
                    TenGoiY = g.Ten,
                    SanPhams = g.ChiTietSanPhamGoiYs.Select(ct => ct.MaSanPhamNavigation).ToList()
                })
                .ToListAsync();

            return View(goiYs);
        }


        private int? GetCurrentUserId()
        {
            var userIdString = HttpContext.Session.GetString("AdminId");

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
