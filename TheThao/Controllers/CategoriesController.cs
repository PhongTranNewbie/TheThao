using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoaiSanPham>>> GetCategories()
        {
            return await _context.LoaiSanPhams.ToListAsync();
        }
    }
}
