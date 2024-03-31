using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TheThao.Models;
using TheThao.Services;

namespace TheThao.Controllers
{
    public class SanPhamsController : Controller
    {
        private readonly TheThaoContext _context;
        private readonly IProductService _productService;
        public SanPhamsController(TheThaoContext context, IProductService productService)
        {
            _context = context;
            _productService = productService;
        }

        // GET: SanPhams
        public async Task<IActionResult> Index()
        {
            var theThaoContext = _context.SanPhams.Include(s => s.TheLoai);
            _productService.UpdateProductPrices();
            return View(await theThaoContext.ToListAsync());
        }

        // GET: SanPhams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SanPhams == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPhams
                .Include(s => s.TheLoai)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }

        // GET: SanPhams/Create
        public IActionResult Create()
        {
            ViewData["TheLoaiId"] = new SelectList(_context.LoaiSanPhams, "Id", "TenLoaiSanPham");
            return View();
        }

        // POST: SanPhams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ChiTiet,Gia,GiaTruocKhiGiamGia,HinhAnh,SoLuong,TheLoaiId")] SanPham sanPham)
        {
           
                _context.Add(sanPham);
            _productService.UpdateProductPrices();
            await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            
        }

        // GET: SanPhams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SanPhams == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPhams.FindAsync(id);
            if (sanPham == null)
            {
                return NotFound();
            }
            ViewData["TheLoaiId"] = new SelectList(_context.LoaiSanPhams, "Id", "Id", sanPham.TheLoaiId);
            return View(sanPham);
        }

        // POST: SanPhams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ChiTiet,Gia,GiaTruocKhiGiamGia,HinhAnh,SoLuong,TheLoaiId")] SanPham sanPham)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    _context.Update(sanPham);
                    _productService.UpdateProductPrices();
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SanPhamExists(sanPham.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TheLoaiId"] = new SelectList(_context.LoaiSanPhams, "Id", "Id", sanPham.TheLoaiId);
            return View(sanPham);
        }



        // GET: SanPhams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SanPhams == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPhams
                .Include(s => s.TheLoai)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }

        // POST: SanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SanPhams == null)
            {
                return Problem("Entity set 'TheThaoContext.SanPhams'  is null.");
            }
            var sanPham = await _context.SanPhams.FindAsync(id);
            if (sanPham != null)
            {
                _context.SanPhams.Remove(sanPham);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SanPhamExists(int id)
        {
          return (_context.SanPhams?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        public IActionResult ChiTietSanPham(int id)
        {
            var users = _context.Users.ToList();

            // Truyền dữ liệu sang view
            ViewData["Users"] = users;
            ViewBag.SanPham = _context.SanPhams.FirstOrDefault(e => e.Id == id);
            ViewBag.ChiTietHinhAnhs = _context.ChiTietHinhAnhs.ToList();
            var listOfCommentViewModels = _context.BinhLuans
                .Where(b => b.MaSanPham == id) // Thay vì b.Id == id, sử dụng b.MaSanPham == id để lấy tất cả bình luận cho sản phẩm này
                .Select(a => new BinhLuanViewModel
                {
                    Id = a.Id,
                    MaSanPham = a.MaSanPham,
                    UserId = a.UserId,
                    BinhLuan1 = a.BinhLuan1,
                    NgayTao = a.NgayTao,
                    NgayCuoiCapNhat = a.NgayCuoiCapNhat,
                    Rate = a.Rate
                })
                .ToList();
            return View(listOfCommentViewModels);
        }



        [HttpPost]
        public ActionResult AddComment(int id, int rate, string productComment)
        {
            BinhLuan bl = new BinhLuan();
            bl.MaSanPham = id; // Sử dụng id được truyền từ form
            bl.BinhLuan1 = productComment;
            bl.NgayTao = DateTime.Now;
            bl.Rate = rate;
            var userIdString = HttpContext.Session.GetString("UserId");
            if (!string.IsNullOrEmpty(userIdString) && int.TryParse(userIdString, out int userId))
            {
                bl.UserId = userId;
            }
            _context.BinhLuans.Add(bl);
            _context.SaveChanges();
            return RedirectToAction("ChiTietSanPham", new { id = id });
        }



    }


}
