using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TheThao.Models;

namespace TheThao.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoriesApiController : ControllerBase
    {
        private readonly TheThaoContext _context;

        public CategoriesApiController(TheThaoContext context)
        {
            _context = context;
        }
/*
        public async Task<IActionResult> Index(int Id)
        {
            LoaiSanPham category = _context.LoaiSanPhams.Where(c=>c.Id == Id).FirstOrDefault();
            if(category == null)
            {
                return RedirectToAction("Index");
            }

            var productByCategory = _context.SanPhams.Where(c => c.TheLoaiId == category.Id);

            return View(await productByCategory.OrderByDescending(c => c.Id).ToListAsync());
        }
*/
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoaiSanPham>>> GetCategories()
        {
            return await _context.LoaiSanPhams.ToListAsync();
        }
    }
}
