using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TheThao.Models;

public partial class TheThaoContext : DbContext
{
    public TheThaoContext()
    {
    }

    public TheThaoContext(DbContextOptions<TheThaoContext> options)
        : base(options)
    {
    }



    public virtual DbSet<ChiTietGioHang> ChiTietGioHangs { get; set; }

    public virtual DbSet<ChiTietHinhAnh> ChiTietHinhAnhs { get; set; }

    public virtual DbSet<ChiTietNhapHang> ChiTietNhapHangs { get; set; }

    public virtual DbSet<Circle> Circles { get; set; }

    public virtual DbSet<DonHang> DonHangs { get; set; }

    public virtual DbSet<GioHang> GioHangs { get; set; }

    public virtual DbSet<LoaiSanPham> LoaiSanPhams { get; set; }

    public virtual DbSet<NhaCungCap> NhaCungCaps { get; set; }

    public virtual DbSet<NhapHang> NhapHangs { get; set; }

    public virtual DbSet<SanPham> SanPhams { get; set; }

    public virtual DbSet<Shipping> Shippings { get; set; }

    public virtual DbSet<ThanhToan> ThanhToans { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=MSI\\SQLSEVER2022DEV;Database=TheThao;trusted_connection=yes;Encrypt=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        

        modelBuilder.Entity<ChiTietGioHang>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ChiTietG__3214EC071459AEE0");

            entity.ToTable("ChiTietGioHang");

            entity.HasOne(d => d.IdGioHangNavigation).WithMany(p => p.ChiTietGioHangs)
                .HasForeignKey(d => d.IdGioHang)
                .HasConstraintName("FK__ChiTietGi__IdGio__5070F446");

            entity.HasOne(d => d.IdSanPhamNavigation).WithMany(p => p.ChiTietGioHangs)
                .HasForeignKey(d => d.IdSanPham)
                .HasConstraintName("FK__ChiTietGi__IdSan__4F7CD00D");
        });

        modelBuilder.Entity<ChiTietHinhAnh>(entity =>
        {
            entity.ToTable("ChiTietHinhAnh");

            entity.Property(e => e.HinhAnh).HasMaxLength(500);

            entity.HasOne(d => d.MaSanPhamNavigation).WithMany(p => p.ChiTietHinhAnhs)
                .HasForeignKey(d => d.MaSanPham)
                .HasConstraintName("FK_ChiTietHinhAnh_SanPham");
        });

        modelBuilder.Entity<ChiTietNhapHang>(entity =>
        {
            entity.ToTable("ChiTietNhapHang");

            entity.Property(e => e.Gia)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.SoLuongNhap)
                .HasMaxLength(10)
                .IsFixedLength();

            entity.HasOne(d => d.MaNhaCungCapNavigation).WithMany(p => p.ChiTietNhapHangs)
                .HasForeignKey(d => d.MaNhaCungCap)
                .HasConstraintName("FK_ChiTietNhapHang_NhaCungCap");

            entity.HasOne(d => d.MaNhapHangNavigation).WithMany(p => p.ChiTietNhapHangs)
                .HasForeignKey(d => d.MaNhapHang)
                .HasConstraintName("FK_ChiTietNhapHang_NhapHang");

            entity.HasOne(d => d.MaSanPhamNavigation).WithMany(p => p.ChiTietNhapHangs)
                .HasForeignKey(d => d.MaSanPham)
                .HasConstraintName("FK_ChiTietNhapHang_SanPham");
        });

        modelBuilder.Entity<Circle>(entity =>
        {
            entity.ToTable("Circle");

            entity.Property(e => e.HinhAnh).HasMaxLength(500);
        });

        modelBuilder.Entity<DonHang>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DonHang__3214EC07A8B60365");

            entity.ToTable("DonHang");

            entity.Property(e => e.DiaChi)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Ngay).HasColumnType("date");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Shipping).WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.ShippingId)
                .HasConstraintName("FK__DonHang__Shippin__5535A963");

            entity.HasOne(d => d.User).WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__DonHang__UserId__5441852A");
        });

        modelBuilder.Entity<GioHang>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GioHang__3214EC07F7135696");

            entity.ToTable("GioHang");

            entity.Property(e => e.MaGiaoDichMerchant)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.MaGiaoDichUser)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.MaDonHangNavigation).WithMany(p => p.GioHangs)
                .HasForeignKey(d => d.MaDonHang)
                .HasConstraintName("FK_GioHang_DonHang");

            entity.HasOne(d => d.User).WithMany(p => p.GioHangs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_GioHang_Users");
        });

        modelBuilder.Entity<LoaiSanPham>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LoaiSanP__3214EC075426E094");

            entity.ToTable("LoaiSanPham");

            entity.Property(e => e.TenLoaiSanPham).HasMaxLength(255);
        });

        modelBuilder.Entity<NhaCungCap>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__NhaCungC__3214EC077D1EE1BD");

            entity.ToTable("NhaCungCap");

            entity.Property(e => e.DiaChi)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Sdt)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("SDT");
            entity.Property(e => e.TenNhaCungCap)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<NhapHang>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__NhapHang__3214EC079A8D94B9");

            entity.ToTable("NhapHang");

            entity.HasOne(d => d.User).WithMany(p => p.NhapHangs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__NhapHang__UserId__54CB950F");
        });

        modelBuilder.Entity<SanPham>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SanPham__3214EC07A00E0847");

            entity.ToTable("SanPham");

            entity.Property(e => e.ChiTiet).HasMaxLength(500);
            entity.Property(e => e.HinhAnh).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(255);

            entity.HasOne(d => d.TheLoai).WithMany(p => p.SanPhams)
                .HasForeignKey(d => d.TheLoaiId)
                .HasConstraintName("FK_SanPham_LoaiSanPham");
        });

        modelBuilder.Entity<Shipping>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Shipping__3214EC07937ABA17");

            entity.ToTable("Shipping");

            entity.Property(e => e.DiaChi)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Ngay).HasColumnType("date");

            entity.HasOne(d => d.User).WithMany(p => p.Shippings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Shipping__UserId__45F365D3");
        });

        modelBuilder.Entity<ThanhToan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ThanhToa__3214EC07E2EB90B7");

            entity.ToTable("ThanhToan");

            entity.Property(e => e.Ngay).HasColumnType("date");
            entity.Property(e => e.PhuongPhapThanhToan)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.MaGioHangNavigation).WithMany(p => p.ThanhToans)
                .HasForeignKey(d => d.MaGioHang)
                .HasConstraintName("FK__ThanhToan__MaGio__3C69FB99");

            entity.HasOne(d => d.User).WithMany(p => p.ThanhToans)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_ThanhToan_Users");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC078F80E094");

            entity.Property(e => e.DiaChi)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.NgayThangNamSinh).HasColumnType("date");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Role)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Ten)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
