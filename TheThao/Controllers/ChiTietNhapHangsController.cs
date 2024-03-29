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
    public class ChiTietNhapHangsController : Controller
    {
        private readonly TheThaoContext _context;
        private readonly IProductService _productService;
        public ChiTietNhapHangsController(TheThaoContext context, IProductService productService)
        {
            _context = context;
            _productService = productService;
        }

        // GET: ChiTietNhapHangs
        public async Task<IActionResult> Index()
        {
            var theThaoContext = _context.ChiTietNhapHangs.Include(c => c.MaNhaCungCapNavigation).Include(c => c.MaNhapHangNavigation).Include(c => c.MaSanPhamNavigation);
            _productService.UpdateProductPrices();
            return View(await theThaoContext.ToListAsync());
        }

        // GET: ChiTietNhapHangs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chiTietNhapHang = await _context.ChiTietNhapHangs
                .Include(c => c.MaNhaCungCapNavigation)
                .Include(c => c.MaNhapHangNavigation)
                .Include(c => c.MaSanPhamNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chiTietNhapHang == null)
            {
                return NotFound();
            }

            return View(chiTietNhapHang);
        }

        // GET: ChiTietNhapHangs/Create
        public IActionResult Create()
        {
            ViewData["MaSanPham"] = new SelectList(_context.SanPhams, "Id", "Name");
            ViewData["MaNhapHang"] = new SelectList(_context.NhapHangs, "Id", "Id");
            ViewData["MaNhaCungCap"] = new SelectList(_context.NhaCungCaps, "Id", "TenNhaCungCap");
            return View();
        }


        // POST: ChiTietNhapHangs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MaSanPham,MaNhapHang,MaNhaCungCap,SoLuongNhap,Gia,TongTien")] ChiTietNhapHang chiTietNhapHang)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chiTietNhapHang);
                
                var product = _context.SanPhams.FirstOrDefault(p => p.Id == chiTietNhapHang.MaSanPham);
                var sl = product.SoLuong;
                sl += chiTietNhapHang.SoLuongNhap;
                product.SoLuong = sl;
                await _context.SaveChangesAsync();

                // Tìm sản phẩm tương ứng trong cơ sở dữ liệu


                // Lưu các thay đổi vào cơ sở dữ liệu

                _productService.UpdateProductPrices();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaNhaCungCap"] = new SelectList(_context.NhaCungCaps, "Id", "TenNhaCungCap", chiTietNhapHang.MaNhaCungCap);
            ViewData["MaNhapHang"] = new SelectList(_context.NhapHangs, "Id", "Id", chiTietNhapHang.MaNhapHang);
            ViewData["MaSanPham"] = new SelectList(_context.SanPhams, "Id", "TenSanPham", chiTietNhapHang.MaSanPham);
            return View(chiTietNhapHang);
        }


        // GET: ChiTietNhapHangs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chiTietNhapHang = await _context.ChiTietNhapHangs.FindAsync(id);
            if (chiTietNhapHang == null)
            {
                return NotFound();
            }
            ViewData["MaNhaCungCap"] = new SelectList(_context.NhaCungCaps, "Id", "TenNhaCungCap", chiTietNhapHang.MaNhaCungCap);
            ViewData["MaNhapHang"] = new SelectList(_context.NhapHangs, "Id", "Id", chiTietNhapHang.MaNhapHang);
            ViewData["MaSanPham"] = new SelectList(_context.SanPhams, "Id", "Name", chiTietNhapHang.MaSanPham);
            return View(chiTietNhapHang);
        }

        // POST: ChiTietNhapHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MaSanPham,MaNhapHang,MaNhaCungCap,SoLuongNhap,Gia,TongTien")] ChiTietNhapHang chiTietNhapHang)
        {
            if (id != chiTietNhapHang.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chiTietNhapHang);
                    await _context.SaveChangesAsync();
                    _productService.UpdateProductPrices();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChiTietNhapHangExists(chiTietNhapHang.Id))
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
            ViewData["MaNhaCungCap"] = new SelectList(_context.NhaCungCaps, "Id", "Id", chiTietNhapHang.MaNhaCungCap);
            ViewData["MaNhapHang"] = new SelectList(_context.NhapHangs, "Id", "Id", chiTietNhapHang.MaNhapHang);
            ViewData["MaSanPham"] = new SelectList(_context.SanPhams, "Id", "Id", chiTietNhapHang.MaSanPham);
            return View(chiTietNhapHang);
        }

        // GET: ChiTietNhapHangs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ChiTietNhapHangs == null)
            {
                return NotFound();
            }

            var chiTietNhapHang = await _context.ChiTietNhapHangs
                .Include(c => c.MaNhaCungCapNavigation)
                .Include(c => c.MaNhapHangNavigation)
                .Include(c => c.MaSanPhamNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chiTietNhapHang == null)
            {
                return NotFound();
            }

            return View(chiTietNhapHang);
        }

        // POST: ChiTietNhapHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ChiTietNhapHangs == null)
            {
                return Problem("Entity set 'TheThaoContext.ChiTietNhapHangs'  is null.");
            }
            var chiTietNhapHang = await _context.ChiTietNhapHangs.FindAsync(id);
            if (chiTietNhapHang != null)
            {
                _context.ChiTietNhapHangs.Remove(chiTietNhapHang);
            }
            
            await _context.SaveChangesAsync();
            _productService.UpdateProductPrices();
            return RedirectToAction(nameof(Index));
        }

        private bool ChiTietNhapHangExists(int id)
        {
          return (_context.ChiTietNhapHangs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
