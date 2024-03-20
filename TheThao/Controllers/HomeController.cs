using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
            List<SanPham> products = _context.SanPhams.ToList();
            ViewBag.Products = products;
            ViewBag.IsLoggedIn = false;
            var circles = _context.Circles.ToList();
            ViewBag.CircleImages = circles;
            return View();
        }
        public IActionResult Category(int theLoaiId)
        {
            
            var products = _context.SanPhams.Where(p => p.TheLoaiId == theLoaiId).ToList();
            ViewBag.Products = products;
            return View();
        }
        public IActionResult GetCircleImages()
        {
            var circles = _context.Circles.ToList();
            return PartialView("_CircleImagesPartial", circles);
        }
        public async Task<IActionResult> HAIndex()
        {
            var theThaoContext = _context.ChiTietHinhAnhs.Include(c => c.MaSanPhamNavigation);
            return View(await theThaoContext.ToListAsync());
        }
        public IActionResult CreateHA()
        {
            ViewData["MaSanPham"] = new SelectList(_context.SanPhams, "Id", "Name");
            return View();
        }

        // POST: NhaCungCaps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateHA([Bind("Id,MaSanPham,HinhAnh")] ChiTietHinhAnh HA)
        {
            if (ModelState.IsValid)
            {
                _context.Add(HA);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(HAIndex));
            }
            return View(HA);
        }
        // GET: ChiTietNhapHangs/Edit/5
        public async Task<IActionResult> EditHA(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chiTietHinhAnh = await _context.ChiTietHinhAnhs.FindAsync(id);
            if (chiTietHinhAnh == null)
            {
                return NotFound();
            }

            ViewData["MaSanPham"] = new SelectList(_context.SanPhams, "Id", "Name", chiTietHinhAnh.MaSanPham);
            return View(chiTietHinhAnh);
        }

        // POST: ChiTietNhapHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditHA(int id, [Bind("Id,MaSanPham,HinhAnh")] ChiTietHinhAnh chiTietHinhAnh)
        {
            if (id != chiTietHinhAnh.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chiTietHinhAnh);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChiTietHinhAnhExists(chiTietHinhAnh.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(HAIndex));
            }

            ViewData["MaSanPham"] = new SelectList(_context.SanPhams, "Id", "Id", chiTietHinhAnh.MaSanPham);
            return View(chiTietHinhAnh);
        }
        private bool ChiTietHinhAnhExists(int id)
        {
            return (_context.ChiTietHinhAnhs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
