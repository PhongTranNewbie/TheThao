using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TheThao.Models;
using TheThao.Services;

namespace TheThao.Controllers
{
    public class ChiTietKhuyenMaisController : Controller
    {
        private readonly TheThaoContext _context;
        private readonly IProductService _productService;
        private readonly IPromotionService _promotionService;
        public ChiTietKhuyenMaisController(TheThaoContext context, IProductService productService, IPromotionService promotionService)
        {
            _context = context;
            _productService = productService;
            _promotionService = promotionService;
        }

        // GET: NhaCungCaps
        public async Task<IActionResult> Index()
        {
            var theThaoContext = _context.ChiTietKhuyenMais.Include(c => c.MaKmNavigation).Include(c => c.MaSanPhamNavigation);

            _productService.UpdateProductPrices();
            return View(await theThaoContext.ToListAsync());
        }

        // GET: NhaCungCaps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ChiTietKhuyenMais == null)
            {
                return NotFound();
            }

            var phanTram = await _context.ChiTietKhuyenMais
                .FirstOrDefaultAsync(m => m.Id == id);
            if (phanTram == null)
            {
                return NotFound();
            }

            return View(phanTram);
        }

        // GET: NhaCungCaps/Create
        public IActionResult Create()
        {
            ViewData["MaSanPham"] = new SelectList(_context.SanPhams, "Id", "Name");
            ViewBag.MaKM = new SelectList(_context.KhuyenMais, "Id", "TenKm");
            return View();
        }

        // POST: NhaCungCaps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MaSanPham,MaKm,NgayBatDau,NgayKetThuc")] ChiTietKhuyenMai phanTram)
        {

            _context.Add(phanTram);
            // Gọi phương thức ApplyPromotionAsync để áp dụng khuyến mãi cho sản phẩm
            
            await _context.SaveChangesAsync();
            _productService.UpdateProductPrices();
            return RedirectToAction(nameof(Index));
        }

        // GET: NhaCungCaps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ChiTietPhanTrams == null)
            {
                return NotFound();
            }

            var phanTram = await _context.ChiTietKhuyenMais.FindAsync(id);
            if (phanTram == null)
            {
                return NotFound();
            }
            ViewData["MaSanPham"] = new SelectList(_context.SanPhams, "Id", "Name", phanTram.MaSanPham);

            ViewData["MaKM"] = new SelectList(_context.PhanTramGiaCas, "Id", "TenKM", phanTram.MaKm);
            return View(phanTram);
        }

        // POST: NhaCungCaps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MaSanPham,MaKm,NgayBatDau,NgayKetThuc")] ChiTietKhuyenMai phanTram)
        {
            if (id != phanTram.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(phanTram);
                    await _context.SaveChangesAsync();
                    _promotionService.ApplyPromotion();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChiTietKMExists(phanTram.Id))
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
            ViewData["MaSanPham"] = new SelectList(_context.SanPhams, "Id", "Name", phanTram.MaSanPham);

            ViewData["MaKM"] = new SelectList(_context.KhuyenMais, "Id", "TenKM", phanTram.MaKm);
            return View(phanTram);
        }

        // GET: NhaCungCaps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ChiTietPhanTrams == null)
            {
                return NotFound();
            }

            var phanTram = await _context.ChiTietPhanTrams
                .FirstOrDefaultAsync(m => m.Id == id);
            if (phanTram == null)
            {
                return NotFound();
            }

            return View(phanTram);
        }

        // POST: NhaCungCaps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ChiTietPhanTrams == null)
            {
                return Problem("Entity set 'TheThaoContext.ChiTietPhanTrams'  is null.");
            }
            var phanTram = await _context.ChiTietKhuyenMais.FindAsync(id);
            if (phanTram != null)
            {
                _context.ChiTietKhuyenMais.Remove(phanTram);
            }

            await _context.SaveChangesAsync();
            _promotionService.ApplyPromotion();
            return RedirectToAction(nameof(Index));
        }

        private bool ChiTietKMExists(int id)
        {
            return (_context.ChiTietPhanTrams?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
