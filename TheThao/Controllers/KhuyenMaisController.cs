using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TheThao.Models;

namespace TheThao.Controllers
{
    public class KhuyenMaisController : Controller
    {
        private readonly TheThaoContext _context;

        public KhuyenMaisController(TheThaoContext context)
        {
            _context = context;
        }

        // GET: NhaCungCaps
        public async Task<IActionResult> Index()
        {
            return _context.KhuyenMais != null ?
                        View(await _context.KhuyenMais.ToListAsync()) :
                        Problem("Entity set 'TheThaoContext.PhanTramGiaCas'  is null.");
        }

        // GET: NhaCungCaps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.KhuyenMais == null)
            {
                return NotFound();
            }

            var phanTram = await _context.KhuyenMais
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
            return View();
        }

        // POST: NhaCungCaps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenKm,PhanTramKhuyenMai,UserId")] KhuyenMai Pt)
        {

            _context.Add(Pt);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        // GET: NhaCungCaps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.KhuyenMais == null)
            {
                return NotFound();
            }

            var phanTram = await _context.KhuyenMais.FindAsync(id);
            if (phanTram == null)
            {
                return NotFound();
            }
            return View(phanTram);
        }

        // POST: NhaCungCaps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenKm,PhanTramKhuyenMai,UserId")] KhuyenMai Pt)
        {
            if (id != Pt.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Pt);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KhuyenMaiExists(Pt.Id))
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
            return View(Pt);
        }

        // GET: NhaCungCaps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.KhuyenMais == null)
            {
                return NotFound();
            }

            var phanTram = await _context.KhuyenMais
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
            if (_context.KhuyenMais == null)
            {
                return Problem("Entity set 'TheThaoContext.NhaCungCaps'  is null.");
            }
            var phanTram = await _context.KhuyenMais.FindAsync(id);
            if (phanTram != null)
            {
                _context.KhuyenMais.Remove(phanTram);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KhuyenMaiExists(int id)
        {
            return (_context.KhuyenMais?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
