using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheThao.Models;
using TheThao.Services;

namespace TheThao.Controllers
{
    public class CircleController : Controller
    {
        private readonly TheThaoContext _context;
        public CircleController(TheThaoContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return _context.Circles != null ?
                        View(await _context.Circles.ToListAsync()) :
                        Problem("Entity set 'TheThaoContext.NhaCungCaps'  is null.");
        }
        public IActionResult Create()
        {
            return View();
        }

        // POST: NhaCungCaps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,HinhAnh,IsVisible")] Circle circle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(circle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(circle);
        }

        // GET: NhaCungCaps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Circles == null)
            {
                return NotFound();
            }

            var circle = await _context.Circles.FindAsync(id);
            if (circle == null)
            {
                return NotFound();
            }
            return View(circle);
        }

        // POST: NhaCungCaps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,HinhAnh,IsVisible")] Circle circle)
        {
            if (id != circle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(circle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CircleExists(circle.Id))
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
            return View(circle);
        }
        private bool CircleExists(int id)
        {
            return (_context.Circles?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
