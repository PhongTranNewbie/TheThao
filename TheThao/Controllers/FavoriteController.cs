using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheThao.Helpers;
using TheThao.Models;
using TheThao.ViewModels;
using X.PagedList;

namespace TheThao.Controllers
{
    public class FavoriteController : Controller
    {
        private readonly TheThaoContext _context;

        public FavoriteController(TheThaoContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> ToggleFavorite(int productId, int? page)
        {
            // Lấy thông tin người dùng hiện tại từ hệ thống xác thực (chú ý thay đổi nếu cần)
            var userIdString = HttpContext.Session.GetString("UserId");

            if (userIdString == null || !int.TryParse(userIdString, out int userId))
            {
                return RedirectToAction("Login", "Login");
            }
            if (productId != 0)
            {
                // Kiểm tra xem sản phẩm đã được yêu thích chưa
                var existingFavorite = await _context.Favorites.FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId);

                if (existingFavorite == null)
                {
                    // Nếu sản phẩm chưa được yêu thích, thêm vào danh sách yêu thích
                    _context.Favorites.Add(new Favorite { ProductId = productId, UserId = (int)userId, FavoriteDate = DateTime.Now });
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // Nếu sản phẩm đã được yêu thích, loại bỏ khỏi danh sách yêu thích
                    _context.Favorites.Remove(existingFavorite);
                    await _context.SaveChangesAsync();
                }
            }


            // Lấy danh sách sản phẩm từ bảng SanPhams
            int pageSize = 12;
            int pageNumber = page ?? 1;
            var products = _context.SanPhams.AsNoTracking().OrderBy(x => x.Id).ToList();

            // Lấy danh sách các sản phẩm yêu thích của người dùng từ bảng Favorite
            var favoriteProducts = await _context.Favorites
                .Where(f => f.UserId == userId)
                .Select(f => f.ProductId)
                .ToListAsync();

            // Tạo danh sách ViewModel từ danh sách sản phẩm và trạng thái yêu thích
            var favoriteProductViewModels = products.Select(p => new FavoriteProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                ChiTiet = p.ChiTiet,
                Gia = p.Gia,
                GiaTruocKhiGiamGia = p.GiaTruocKhiGiamGia,
                GiaSi = p.GiaSi,
                HinhAnh = p.HinhAnh,
                SoLuong = p.SoLuong,
                TheLoaiId = p.TheLoaiId,
                ThoiGianBaoHanh = p.ThoiGianBaoHanh,
                SoLuongDaBan = p.SoLuongDaBan,
                TrangThai = p.TrangThai,
                IsFavorite = favoriteProducts.Contains(p.Id) // Kiểm tra xem sản phẩm có trong danh sách yêu thích không
            }).ToList();
            PagedList<FavoriteProductViewModel> fv = new PagedList<FavoriteProductViewModel>(favoriteProductViewModels, pageNumber, pageSize);
            ViewBag.IsLoggedIn = false;
            var circles = _context.Circles.ToList();
            ViewBag.CircleImages = circles;
            // Trả về PartialView index với danh sách ViewModel và thông tin phân trang
            return PartialView("_Index", fv);
        }



        // Phương thức hỗ trợ để lấy ID của người dùng hiện tại từ hệ thống xác thực
        private int? GetCurrentUserId()
        {
            var userIdString = HttpContext.Session.GetString("UserId");

            if (userIdString == null || !int.TryParse(userIdString, out int userId))
            {
                // Hoặc thay đổi cách lấy ID của người dùng dựa trên hệ thống xác thực bạn đang sử dụng
                // Ví dụ: ASP.NET Core Identity
                // var user = await _userManager.GetUserAsync(User);
                // var userId = user?.Id;
                return null;
            }

            return userId;
        }



        public async Task<IActionResult> Index()
        {
            var userIdString = HttpContext.Session.GetString("UserId");

            if (userIdString == null || !int.TryParse(userIdString, out int userId))
            {
                return RedirectToAction("Login", "Login");
            }

            var favoriteProductIds = await _context.Favorites
                .Where(f => f.UserId == userId)
                .Select(f => f.ProductId)
                .ToListAsync();

            var products = await _context.SanPhams
                .Where(p => favoriteProductIds.Contains(p.Id))
                .ToListAsync();

            var promotionService = new PromotionService(_context);
            var favoriteViewModels = new List<FavoriteProductViewModel>();

            foreach (var product in products)
            {
                favoriteViewModels.Add(new FavoriteProductViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    ChiTiet = product.ChiTiet,
                    Gia = product.Gia,
                    GiaTruocKhiGiamGia = product.GiaTruocKhiGiamGia,
                    HinhAnh = product.HinhAnh,
                    SoLuong = product.SoLuong,
                    TheLoaiId = product.TheLoaiId,
                    ThoiGianBaoHanh = product.ThoiGianBaoHanh,
                    SoLuongDaBan = product.SoLuongDaBan,
                    TrangThai = product.TrangThai,
                    IsFavorite = true // As these are favorite products
                });
            }
            return View(favoriteViewModels);
        }
        public async Task<IActionResult> RemoveFavorite(int productId)
        {
            var userId = GetCurrentUserId();

            if (userId == null)
            {
                return RedirectToAction("Login", "Login");
            }

            if (productId != 0)
            {
                var existingFavorite = await _context.Favorites.FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId);

                if (existingFavorite != null)
                {
                    _context.Favorites.Remove(existingFavorite);
                    await _context.SaveChangesAsync();
                }
            }

            var favoriteProductIds = await _context.Favorites
                .Where(f => f.UserId == userId)
                .Select(f => f.ProductId)
                .ToListAsync();

            var products = await _context.SanPhams
                .Where(p => favoriteProductIds.Contains(p.Id))
                .ToListAsync();

            var favoriteViewModels = products.Select(p => new FavoriteProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                ChiTiet = p.ChiTiet,
                Gia = p.Gia,
                GiaTruocKhiGiamGia = p.GiaTruocKhiGiamGia,
                GiaSi = p.GiaSi,
                HinhAnh = p.HinhAnh,
                SoLuong = p.SoLuong,
                TheLoaiId = p.TheLoaiId,
                ThoiGianBaoHanh = p.ThoiGianBaoHanh,
                SoLuongDaBan = p.SoLuongDaBan,
                TrangThai = p.TrangThai,
                IsFavorite = true
            }).ToList();

            return View("Index", favoriteViewModels);
        }
    }
}
