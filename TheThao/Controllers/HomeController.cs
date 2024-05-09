using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TheThao.Models;
using TheThao.Models.Authentication;
using X.PagedList;

namespace TheThao.Controllers
{
    public class HomeController : Controller
    {
        private readonly TheThaoContext _context;

        public HomeController(TheThaoContext context)
        {
            _context = context;
        }
        public IActionResult Index(int? page)
        {
            int pageSize = 12;
            int pageNumber = page == null || page < 0 ? 1: page.Value;
            var products = _context.SanPhams.AsNoTracking().OrderBy(x=>x.Id);
            PagedList<SanPham> productsList = new PagedList<SanPham>(products, pageNumber, pageSize);
            
            ViewBag.IsLoggedIn = false;
            var circles = _context.Circles.ToList();
            ViewBag.CircleImages = circles;
            
            return View(productsList);
        }
        public IActionResult Category(int theLoaiId, string priceRange)
        {
            IQueryable<SanPham> productsQuery = _context.SanPhams.Where(p => p.TheLoaiId == theLoaiId);

            if (!string.IsNullOrEmpty(priceRange))
            {
                var priceRanges = priceRange.Split('-').Select(int.Parse).ToArray();
                if (priceRanges.Length == 2)
                {
                    // Tách phân khúc giá thành giá tối thiểu và tối đa
                    var priceSegments = priceRange.Split('-');
                    var minPrice = Convert.ToDecimal(priceSegments[0]); // Chuyển đổi sang decimal
                    var maxPrice = priceSegments.Length == 2 ? Convert.ToDecimal(priceSegments[1]) : 999999999999; // Set a reasonable maximum value

                    // Lọc sản phẩm theo phân khúc giá
                    productsQuery = productsQuery.Where(p => p.Gia >= minPrice && p.Gia <= maxPrice);
                }
            }

            var products = productsQuery.ToList();
            ViewBag.Products = products;

            var circles = _context.Circles.ToList();
            ViewBag.CircleImages = circles;

            return View();
        }

        public IActionResult GetCircleImages()
        {
            var circles = _context.Circles.ToList();
            return PartialView("_CircleImagesPartial", circles);
        }
        public async Task<IActionResult> HAIndex()
        {
            var theThaoContext = _context.ChiTietHinhAnhs.Include(c => c.MaSanPhamNavigation);
            return View(await theThaoContext.ToListAsync());
        }
        public IActionResult CreateHA()
        {
            ViewData["MaSanPham"] = new SelectList(_context.SanPhams, "Id", "Name");
            return View();
        }

        // POST: NhaCungCaps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateHA([Bind("Id,MaSanPham,HinhAnh")] ChiTietHinhAnh HA)
        {
            if (ModelState.IsValid)
            {
                _context.Add(HA);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(HAIndex));
            }
            return View(HA);
        }
        // GET: ChiTietNhapHangs/Edit/5
        public async Task<IActionResult> EditHA(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chiTietHinhAnh = await _context.ChiTietHinhAnhs.FindAsync(id);
            if (chiTietHinhAnh == null)
            {
                return NotFound();
            }

            ViewData["MaSanPham"] = new SelectList(_context.SanPhams, "Id", "Name", chiTietHinhAnh.MaSanPham);
            return View(chiTietHinhAnh);
        }

        // POST: ChiTietNhapHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditHA(int id, [Bind("Id,MaSanPham,HinhAnh")] ChiTietHinhAnh chiTietHinhAnh)
        {
            if (id != chiTietHinhAnh.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chiTietHinhAnh);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChiTietHinhAnhExists(chiTietHinhAnh.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(HAIndex));
            }

            ViewData["MaSanPham"] = new SelectList(_context.SanPhams, "Id", "Id", chiTietHinhAnh.MaSanPham);
            return View(chiTietHinhAnh);
        }
        private bool ChiTietHinhAnhExists(int id)
        {
            return (_context.ChiTietHinhAnhs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        public bool HasPromotion(int productId)
        {
            // Kiểm tra xem có bất kỳ chi tiết khuyến mãi nào cho sản phẩm không
            return _context.ChiTietKhuyenMais.Any(ctkm => ctkm.MaSanPham == productId);
        }


        [HttpGet]
        public ActionResult Search(string keyword, decimal? minPrice, decimal? maxPrice, string status)
        {
            if (string.IsNullOrEmpty(keyword) && !minPrice.HasValue && !maxPrice.HasValue && string.IsNullOrEmpty(status))
            {
                // Trả về kết quả mặc định nếu không có tham số nào được cung cấp
                return PartialView("SearchResults", _context.SanPhams.ToList());
            }

            // Chuyển đổi từ khóa tìm kiếm sang chữ thường
            var lowerKeyword = keyword?.ToLower();

            // Lọc sản phẩm dựa trên từ khóa (nếu có)
            var products = _context.SanPhams.AsQueryable();
            if (!string.IsNullOrEmpty(lowerKeyword))
            {
                products = products.Where(p => p.Name.ToLower().Contains(lowerKeyword));
            }

            // Lọc sản phẩm theo phân khúc giá (nếu được chỉ định)
            if (minPrice.HasValue && maxPrice.HasValue)
            {
                products = products.Where(p => p.Gia >= minPrice && p.Gia <= maxPrice);
            }

            // Lọc sản phẩm theo trạng thái (nếu được chỉ định)
            if (!string.IsNullOrEmpty(status))
            {
                products = products.Where(p => p.TrangThai == status);
            }

            var productList = products.ToList();
            var circles = _context.Circles.ToList();
            ViewBag.CircleImages = circles;

            // Trả về view kết quả tìm kiếm
            return View("SearchResults", productList);
        }











        public IActionResult CheckoutSuccess()
        {
            return View();
        }

    }
}
