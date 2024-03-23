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
    public class ChiTietPhanTramsController : Controller
    {
        private readonly TheThaoContext _context;
        private readonly IProductService _productService;
        public ChiTietPhanTramsController(TheThaoContext context, IProductService productService)
        {
            _context = context;
            _productService = productService;
        }

        // GET: NhaCungCaps
        public async Task<IActionResult> Index()
        {
            var theThaoContext = _context.ChiTietPhanTrams.Include(c => c.MaPhanTramNavigation).Include(c => c.MaSanPhamNavigation);
            return View(await theThaoContext.ToListAsync());
        }

        // GET: NhaCungCaps/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: NhaCungCaps/Create
        public IActionResult Create()
        {
            ViewData["MaSanPham"] = new SelectList(_context.SanPhams, "Id", "Name");
            ViewData["MaPhanTram"] = new SelectList(_context.PhanTramGiaCas, "Id", "PhanTram");
            return View();
        }

        // POST: NhaCungCaps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MaSanPham,MaPhanTram,NgayBatDau,NgayKetThuc")] ChiTietPhanTram phanTram)
        {
            
                _context.Add(phanTram);
                await _context.SaveChangesAsync();

            _productService.UpdateProductPrices();

            // Lưu các thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
            
        }

        // GET: NhaCungCaps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ChiTietPhanTrams == null)
            {
                return NotFound();
            }

            var phanTram = await _context.ChiTietPhanTrams.FindAsync(id);
            if (phanTram == null)
            {
                return NotFound();
            }
            ViewData["MaSanPham"] = new SelectList(_context.SanPhams, "Id", "Name", phanTram.MaSanPham);

            ViewData["MaPhanTram"] = new SelectList(_context.PhanTramGiaCas, "Id", "PhanTram", phanTram.MaPhanTram);
            return View(phanTram);
        }

        // POST: NhaCungCaps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MaSanPham,MaPhanTram,NgayBatDau,NgayKetThuc")] ChiTietPhanTram phanTram)
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
                    _productService.UpdateProductPrices();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChiTietPhanTramExists(phanTram.Id))
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

            ViewData["MaPhanTram"] = new SelectList(_context.PhanTramGiaCas, "Id", "PhanTram", phanTram.MaPhanTram);
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
            var phanTram = await _context.ChiTietPhanTrams.FindAsync(id);
            if (phanTram != null)
            {
                _context.ChiTietPhanTrams.Remove(phanTram);
            }

            await _context.SaveChangesAsync();
            _productService.UpdateProductPrices();
            return RedirectToAction(nameof(Index));
        }

        private bool ChiTietPhanTramExists(int id)
        {
            return (_context.ChiTietPhanTrams?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
