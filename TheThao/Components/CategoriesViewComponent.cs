using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheThao.Models;

namespace TheThao.Component
{
	public class CategoriesViewComponent : ViewComponent
	{
		private readonly TheThaoContext _context;

		public CategoriesViewComponent(TheThaoContext context)
		{
			_context = context;
		}

		public async Task<IViewComponentResult> InvokeAsync() => View(await _context.LoaiSanPhams.ToListAsync());
	}
}
