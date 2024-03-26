using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using TheThao.Models;
using TheThao.Services;

namespace TheThao.Controllers
{
    public class ImportController : Controller
    {
        private readonly TheThaoContext _context;
        private readonly IProductService _productService;
        public ImportController(TheThaoContext context, IProductService productService)
        {
            _context = context;
            _productService = productService;
        }

        // GET: NhapHang/Create
        public IActionResult Create()
        {
            ViewData["MaSanPham"] = new SelectList(_context.SanPhams, "Id", "Name");
            ViewData["MaNhapHang"] = new SelectList(_context.NhapHangs, "Id", "Id");
            ViewData["MaNhaCungCap"] = new SelectList(_context.NhaCungCaps, "Id", "TenNhaCungCap");
            
            ViewBag.DanhSachSanPham = new SelectList(_context.SanPhams, "Id", "Name");

            return View();
        }

        // POST: NhapHang/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(NhapHangViewModel model)
        {
            int? userId = null;
            var userIdString = HttpContext.Session.GetString("UserId");
            if (!string.IsNullOrEmpty(userIdString))
            {
                int parsedUserId;
                if (int.TryParse(userIdString, out parsedUserId))
                {
                    userId = parsedUserId;
                }
            }
                if (ModelState.IsValid)
            {
                var nhapHang = new NhapHang
                {
                    NgayNhapHang = DateTime.Now,
                    UserId = userId // Lưu ID của người dùng từ Session
                                    // Thêm các trường khác nếu cần
                };

                _context.Add(nhapHang);
                _context.SaveChanges();

                foreach (var chiTiet in model.ChiTietNhapHangs)
                {
                    var chiTietNhapHang = new ChiTietNhapHang
                    {
                        MaSanPham = chiTiet.MaSanPham,
                        MaNhaCungCap = chiTiet.MaNhaCungCap,
                        SoLuongNhap = chiTiet.SoLuongNhap,
                        Gia = chiTiet.Gia,
                        TongTien = chiTiet.SoLuongNhap * chiTiet.Gia, // Tính tổng tiền
                        MaNhapHang = nhapHang.Id
                    };

                    _context.Add(chiTietNhapHang);
                    _context.SaveChanges();
                }
                _productService.UpdateProductPrices();


                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }



        // GET: NhapHang/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhapHang = _context.NhapHangs
                .Include(n => n.ChiTietNhapHangs)
                .ThenInclude(c => c.MaNhaCungCapNavigation)
                .FirstOrDefault(m => m.Id == id);

            if (nhapHang == null)
            {
                return NotFound();
            }

            return View(nhapHang);
        }

        // GET: NhapHang/CreateChiTietNhapHang
        public IActionResult CreateChiTietNhapHang()
        {
            return View();
        }

        // POST: NhapHang/CreateChiTietNhapHang
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateChiTietNhapHang(ChiTietNhapHang chiTietNhapHang)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chiTietNhapHang);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(chiTietNhapHang);
        }

        // Add other actions such as Edit, Delete, etc. as needed
    }
}
