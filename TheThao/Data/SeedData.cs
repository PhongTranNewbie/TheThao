using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TheThao.Models;

namespace TheThao.Data
{
    public static class SeedData
    {
        public static void Initialize(TheThaoContext context)
        {
            // Kiểm tra xem cơ sở dữ liệu đã có dữ liệu chưa
            if (context.LoaiSanPhams.Any() || context.SanPhams.Any())
            {
                return;   // Cơ sở dữ liệu đã được khởi tạo
            }

            // Thêm loại sản phẩm
            var loaiSanPhams = new List<LoaiSanPham>
            {
                new LoaiSanPham
                {
                    TenLoaiSanPham = "Giày"
                },
                new LoaiSanPham
                {
                    TenLoaiSanPham = "Quần áo gym"
                },
                new LoaiSanPham
                {
                    TenLoaiSanPham = "Tạ"
                },
                new LoaiSanPham
                {
                    TenLoaiSanPham = "Máy chạy bộ"
                },
                // Thêm các loại sản phẩm khác tương tự ở đây
            };

            context.LoaiSanPhams.AddRange(loaiSanPhams);
            context.SaveChanges();

            // Thêm các sản phẩm tương ứng với mỗi loại sản phẩm
            var sanPhams = new List<SanPham>
            {
                // Sản phẩm giày
                new SanPham
                {
                    Name = "Giày chạy bộ Adidas",
                    ChiTiet = "Giày chạy bộ Adidas chính hãng",
                    Gia = 120.50m,
                    HinhAnh = "adidas.jpg",
                    TheLoaiId = loaiSanPhams[0].Id
                },
                // Sản phẩm quần áo gym
                new SanPham
                {
                    Name = "Quần áo tập gym Puma",
                    ChiTiet = "Bộ quần áo tập gym Puma cao cấp",
                    Gia = 65.75m,
                    HinhAnh = "puma.jpg",
                    TheLoaiId = loaiSanPhams[1].Id
                },
                // Sản phẩm tạ
                new SanPham
                {
                    Name = "Tạ 5kg",
                    ChiTiet = "Tạ luyện tập tay 5kg",
                    Gia = 15.20m,
                    HinhAnh = "dumbbell.jpg",
                    TheLoaiId = loaiSanPhams[2].Id
                },
                // Sản phẩm máy chạy bộ
                new SanPham
                {
                    Name = "Máy chạy bộ điện tử",
                    ChiTiet = "Máy chạy bộ điện tử tiện ích",
                    Gia = 899.99m,
                    HinhAnh = "treadmill.jpg",
                    TheLoaiId = loaiSanPhams[3].Id
                },
                // Thêm các sản phẩm khác tương tự ở đây
            };

            context.SanPhams.AddRange(sanPhams);
            context.SaveChanges();
        }
    }
}
