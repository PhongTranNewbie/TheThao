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
    public class NhapHangsController : Controller
    {
        private readonly TheThaoContext _context;

        public NhapHangsController(TheThaoContext context)
        {
            _context = context;
        }

        // GET: NhapHangs
        public async Task<IActionResult> Index()
        {
            var theThaoContext = _context.NhapHangs.Include(n => n.User);
            return View(await theThaoContext.ToListAsync());
        }

        // GET: NhapHangs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.NhapHangs == null)
            {
                return NotFound();
            }

            var nhapHang = await _context.NhapHangs
                .Include(n => n.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (nhapHang == null)
            {
                return NotFound();
            }

            return View(nhapHang);
        }

        // GET: NhapHangs/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: NhapHangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NgayNhapHang,UserId")] NhapHang nhapHang)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nhapHang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", nhapHang.UserId);
            return View(nhapHang);
        }

        // GET: NhapHangs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.NhapHangs == null)
            {
                return NotFound();
            }

            var nhapHang = await _context.NhapHangs.FindAsync(id);
            if (nhapHang == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", nhapHang.UserId);
            return View(nhapHang);
        }

        // POST: NhapHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NgayNhapHang,UserId")] NhapHang nhapHang)
        {
            if (id != nhapHang.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nhapHang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NhapHangExists(nhapHang.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", nhapHang.UserId);
            return View(nhapHang);
        }

        // GET: NhapHangs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.NhapHangs == null)
            {
                return NotFound();
            }

            var nhapHang = await _context.NhapHangs
                .Include(n => n.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (nhapHang == null)
            {
                return NotFound();
            }

            return View(nhapHang);
        }

        // POST: NhapHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.NhapHangs == null)
            {
                return Problem("Entity set 'TheThaoContext.NhapHangs'  is null.");
            }
            var nhapHang = await _context.NhapHangs.FindAsync(id);
            if (nhapHang != null)
            {
                _context.NhapHangs.Remove(nhapHang);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NhapHangExists(int id)
        {
          return (_context.NhapHangs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
