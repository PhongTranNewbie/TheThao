using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheThao.Models;

namespace TheThao.Controllers
{
    public class LoaiSanPhamsController : Controller
    {
        // GET: LoaiSanPhams
        private readonly TheThaoContext _context;
        public LoaiSanPhamsController(TheThaoContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var loaiSanPham = _context.LoaiSanPhams.ToList();
            return View(loaiSanPham);
        }

        // GET: LoaiSanPhams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null || _context.LoaiSanPhams == null)
            {
                return NotFound();
            }
            var Lsp = await _context.LoaiSanPhams.FirstOrDefaultAsync(m => m.Id == id);
            if (Lsp == null)
            {
                return NotFound();
            }
            return View(Lsp);
        }

        // GET: LoaiSanPhams/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NhaCungCaps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenLoaiSanPham")] LoaiSanPham loaiSanPham)
        {
            if (ModelState.IsValid)
            {
                _context.Add(loaiSanPham);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(loaiSanPham);
        }

        // GET: NhaCungCaps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LoaiSanPhams == null)
            {
                return NotFound();
            }

            var loaiSanPham = await _context.LoaiSanPhams.FindAsync(id);
            if (loaiSanPham == null)
            {
                return NotFound();
            }
            return View(loaiSanPham);
        }

        // POST: NhaCungCaps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenLoaiSanPham")] LoaiSanPham loaiSanPham)
        {
            if (id != loaiSanPham.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loaiSanPham);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NhaCungCapExists(loaiSanPham.Id))
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
            return View(loaiSanPham);
        }

        // GET: NhaCungCaps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LoaiSanPhams == null)
            {
                return NotFound();
            }

            var loaiSanPham = await _context.LoaiSanPhams
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loaiSanPham == null)
            {
                return NotFound();
            }

            return View(loaiSanPham);
        }

        // POST: NhaCungCaps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LoaiSanPhams == null)
            {
                return Problem("Entity set 'TheThaoContext.NhaCungCaps'  is null.");
            }
            var loaiSanPham = await _context.LoaiSanPhams.FindAsync(id);
            if (loaiSanPham != null)
            {
                _context.LoaiSanPhams.Remove(loaiSanPham);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NhaCungCapExists(int id)
        {
            return (_context.LoaiSanPhams?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
