using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using TheThao.Models;

namespace TheThao.Data
{
    public partial class TheThaoContext : DbContext
    {
        public TheThaoContext()
        {
        }

        public TheThaoContext(DbContextOptions<TheThaoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ChiTietGioHang> ChiTietGioHangs { get; set; } = null!;
        public virtual DbSet<DonHang> DonHangs { get; set; } = null!;
        public virtual DbSet<GioHang> GioHangs { get; set; } = null!;
        public virtual DbSet<KhoHang> KhoHangs { get; set; } = null!;
        public virtual DbSet<LoaiSanPham> LoaiSanPhams { get; set; } = null!;
        public virtual DbSet<NhaCungCap> NhaCungCaps { get; set; } = null!;
        public virtual DbSet<NhapHang> NhapHangs { get; set; } = null!;
        public virtual DbSet<SanPham> SanPhams { get; set; } = null!;
        public virtual DbSet<Shipping> Shippings { get; set; } = null!;
        public virtual DbSet<ThanhToan> ThanhToans { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=MSI\\SQLSEVER2022DEV;Database=TheThao;trusted_connection=yes;Encrypt=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChiTietGioHang>(entity =>
            {
                entity.HasOne(d => d.IdGioHangNavigation)
                    .WithMany(p => p.ChiTietGioHangs)
                    .HasForeignKey(d => d.IdGioHang)
                    .HasConstraintName("FK__ChiTietGi__IdGio__5070F446");

                entity.HasOne(d => d.IdSanPhamNavigation)
                    .WithMany(p => p.ChiTietGioHangs)
                    .HasForeignKey(d => d.IdSanPham)
                    .HasConstraintName("FK__ChiTietGi__IdSan__4F7CD00D");
            });

            modelBuilder.Entity<DonHang>(entity =>
            {
                entity.HasOne(d => d.Shipping)
                    .WithMany(p => p.DonHangs)
                    .HasForeignKey(d => d.ShippingId)
                    .HasConstraintName("FK__DonHang__Shippin__5535A963");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.DonHangs)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__DonHang__UserId__5441852A");
            });

            modelBuilder.Entity<GioHang>(entity =>
            {
                entity.HasOne(d => d.MaDonHangNavigation)
                    .WithMany(p => p.GioHangs)
                    .HasForeignKey(d => d.MaDonHang)
                    .HasConstraintName("FK_GioHang_DonHang");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.GioHangs)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_GioHang_Users");
            });

            modelBuilder.Entity<KhoHang>(entity =>
            {
                entity.HasKey(e => e.MaSanPham)
                    .HasName("PK__KhoHang__FAC7442D6BC76C32");

                entity.Property(e => e.MaSanPham).ValueGeneratedNever();

                entity.HasOne(d => d.MaSanPhamNavigation)
                    .WithOne(p => p.KhoHang)
                    .HasForeignKey<KhoHang>(d => d.MaSanPham)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__KhoHang__MaSanPh__4CA06362");
            });

            modelBuilder.Entity<NhapHang>(entity =>
            {
                entity.HasOne(d => d.MaNhaCungCapNavigation)
                    .WithMany(p => p.NhapHangs)
                    .HasForeignKey(d => d.MaNhaCungCap)
                    .HasConstraintName("FK_NhapHang_NhaCungCap");

                entity.HasOne(d => d.MaSanPhamNavigation)
                    .WithMany(p => p.NhapHangs)
                    .HasForeignKey(d => d.MaSanPham)
                    .HasConstraintName("FK_NhapHang_SanPham");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.NhapHangs)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__NhapHang__UserId__412EB0B6");
            });

            modelBuilder.Entity<SanPham>(entity =>
            {
                entity.HasOne(d => d.TheLoai)
                    .WithMany(p => p.SanPhams)
                    .HasForeignKey(d => d.TheLoaiId)
                    .HasConstraintName("FK_SanPham_LoaiSanPham");
            });

            modelBuilder.Entity<Shipping>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.Shippings)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Shipping__UserId__45F365D3");
            });

            modelBuilder.Entity<ThanhToan>(entity =>
            {
                entity.HasOne(d => d.MaGioHangNavigation)
                    .WithMany(p => p.ThanhToans)
                    .HasForeignKey(d => d.MaGioHang)
                    .HasConstraintName("FK__ThanhToan__MaGio__3C69FB99");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ThanhToans)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_ThanhToan_Users");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
