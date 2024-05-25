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
			var chiTietSanPhamGoiYs = _context.ChiTietSanPhamGoiYs.Select(c => c.MaSanPham).ToList();
			var sanPhams = await _context.SanPhams.Where(sp => !chiTietSanPhamGoiYs.Contains(sp.Id)).ToListAsync();
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
            var sanPhamGoiY = await _context.SanPhamGoiYs
                .Include(g => g.ChiTietSanPhamGoiYs)
                .ThenInclude(ct => ct.MaSanPhamNavigation)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (sanPhamGoiY == null)
            {
                return NotFound();
            }

            // Lấy danh sách các sản phẩm đã được chọn trong gợi ý này
            var selectedSanPhams = sanPhamGoiY.ChiTietSanPhamGoiYs.Select(ct => ct.MaSanPham).ToList();

            // Lấy danh sách các sản phẩm chưa được chọn trong bất kỳ gợi ý nào
            var chiTietSanPhamGoiYs = _context.ChiTietSanPhamGoiYs.Select(c => c.MaSanPham).ToList();
            var sanPhams = await _context.SanPhams
                .Where(sp => !chiTietSanPhamGoiYs.Contains(sp.Id) || selectedSanPhams.Contains(sp.Id))
                .ToListAsync();
            var model = new AddProductsToGoiYViewModel
            {
                MaGoiY = sanPhamGoiY.Id,
                TenGoiY = sanPhamGoiY.Ten,
                SanPhams = sanPhams,
                SelectedSanPhams = selectedSanPhams
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int maGoiY, string tenGoiY, List<int> maSanPhams)
        {
            if (string.IsNullOrEmpty(tenGoiY))
            {
                ModelState.AddModelError(string.Empty, "Vui lòng nhập tên gợi ý.");
                var sanPhams = await _context.SanPhams.ToListAsync();
                var model = new AddProductsToGoiYViewModel
                {
                    MaGoiY = maGoiY,
                    TenGoiY = tenGoiY,
                    SanPhams = sanPhams,
                    SelectedSanPhams = maSanPhams ?? new List<int>()
                };
                return View(model);
            }

            var sanPhamGoiY = await _context.SanPhamGoiYs
                .Include(g => g.ChiTietSanPhamGoiYs)
                .FirstOrDefaultAsync(g => g.Id == maGoiY);

            if (sanPhamGoiY == null)
            {
                ModelState.AddModelError(string.Empty, "Gợi ý không tồn tại.");
                return RedirectToAction(nameof(Edit), new { id = maGoiY });
            }

            // Update the name of the recommendation
            sanPhamGoiY.Ten = tenGoiY;

            // Get existing product recommendations
            var existingChiTiet = sanPhamGoiY.ChiTietSanPhamGoiYs.ToList();

            // Remove the products that are no longer selected
            var chiTietToRemove = existingChiTiet.Where(ct => !maSanPhams.Contains(ct.MaSanPham)).ToList();
            _context.ChiTietSanPhamGoiYs.RemoveRange(chiTietToRemove);

            // Add new product recommendations
            foreach (var maSanPham in maSanPhams)
            {
                if (!existingChiTiet.Any(ct => ct.MaSanPham == maSanPham))
                {
                    var chiTiet = new ChiTietSanPhamGoiY
                    {
                        MaSanPham = maSanPham,
                        MaGoiY = maGoiY
                    };

                    _context.ChiTietSanPhamGoiYs.Add(chiTiet);
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
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
