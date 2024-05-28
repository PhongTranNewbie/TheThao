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

    public virtual DbSet<BaoHanh> BaoHanhs { get; set; }

    public virtual DbSet<BinhLuan> BinhLuans { get; set; }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }

    public virtual DbSet<ChiTietHinhAnh> ChiTietHinhAnhs { get; set; }

    public virtual DbSet<ChiTietKhuyenMai> ChiTietKhuyenMais { get; set; }

    public virtual DbSet<ChiTietNhapHang> ChiTietNhapHangs { get; set; }

    public virtual DbSet<ChiTietPhanTram> ChiTietPhanTrams { get; set; }

    public virtual DbSet<ChiTietSanPhamGoiY> ChiTietSanPhamGoiYs { get; set; }

    public virtual DbSet<Circle> Circles { get; set; }

    public virtual DbSet<DonHang> DonHangs { get; set; }

    public virtual DbSet<Favorite> Favorites { get; set; }

    public virtual DbSet<Kho> Khos { get; set; }

    public virtual DbSet<KhuyenMai> KhuyenMais { get; set; }

    public virtual DbSet<LoaiSanPham> LoaiSanPhams { get; set; }

    public virtual DbSet<NhaCungCap> NhaCungCaps { get; set; }

    public virtual DbSet<NhapHang> NhapHangs { get; set; }

    public virtual DbSet<PhanTramGiaCa> PhanTramGiaCas { get; set; }

    public virtual DbSet<QuanLySerial> QuanLySerials { get; set; }

    public virtual DbSet<SanPham> SanPhams { get; set; }

    public virtual DbSet<SanPhamGoiY> SanPhamGoiYs { get; set; }

    public virtual DbSet<SoDiaChi> SoDiaChis { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=MSI\\SQLSEVER2022DEV;Database=TheThao;trusted_connection=yes;Encrypt=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BaoHanh>(entity =>
        {
            entity.ToTable("BaoHanh");

            entity.HasIndex(e => e.MaSerial, "IX_BaoHanh").IsUnique();

            entity.Property(e => e.NgayBaoHanh).HasColumnType("datetime");
            entity.Property(e => e.NgayKetThucBaoHanh).HasColumnType("datetime");
            entity.Property(e => e.TrangThai).HasMaxLength(50);

            entity.HasOne(d => d.MaSerialNavigation).WithOne(p => p.BaoHanh)
                .HasForeignKey<BaoHanh>(d => d.MaSerial)
                .HasConstraintName("FK_BaoHanh_QuanLySerial");
        });

        modelBuilder.Entity<BinhLuan>(entity =>
        {
            entity.ToTable("BinhLuan");

            entity.Property(e => e.BinhLuan1)
                .HasColumnType("text")
                .HasColumnName("BinhLuan");
            entity.Property(e => e.NgayCuoiCapNhat).HasColumnType("datetime");
            entity.Property(e => e.NgayTao).HasColumnType("datetime");

            entity.HasOne(d => d.MaSanPhamNavigation).WithMany(p => p.BinhLuans)
                .HasForeignKey(d => d.MaSanPham)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_BinhLuan_SanPham");

            entity.HasOne(d => d.User).WithMany(p => p.BinhLuans)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_BinhLuan_Users");
        });

        modelBuilder.Entity<Blog>(entity =>
        {
            entity.ToTable("Blog");

            entity.Property(e => e.Title).HasMaxLength(100);

            entity.HasOne(d => d.User).WithMany(p => p.Blogs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Blog_Users");
        });

        modelBuilder.Entity<ChiTietDonHang>(entity =>
        {
            entity.ToTable("ChiTietDonHang");

            entity.Property(e => e.Gia).HasColumnType("decimal(12, 0)");
            entity.Property(e => e.MaDonHang)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.MaDonHangNavigation).WithMany(p => p.ChiTietDonHangs)
                .HasForeignKey(d => d.MaDonHang)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ChiTietDonHang_DonHang");

            entity.HasOne(d => d.MaSanPhamNavigation).WithMany(p => p.ChiTietDonHangs)
                .HasForeignKey(d => d.MaSanPham)
                .HasConstraintName("FK_ChiTietDonHang_SanPham");
        });

        modelBuilder.Entity<ChiTietHinhAnh>(entity =>
        {
            entity.ToTable("ChiTietHinhAnh");

            entity.Property(e => e.HinhAnh).HasMaxLength(500);

            entity.HasOne(d => d.MaSanPhamNavigation).WithMany(p => p.ChiTietHinhAnhs)
                .HasForeignKey(d => d.MaSanPham)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ChiTietHinhAnh_SanPham");
        });

        modelBuilder.Entity<ChiTietKhuyenMai>(entity =>
        {
            entity.ToTable("ChiTietKhuyenMai");

            entity.Property(e => e.MaKm).HasColumnName("MaKM");
            entity.Property(e => e.NgayBatDau).HasColumnType("datetime");
            entity.Property(e => e.NgayKetThuc).HasColumnType("datetime");

            entity.HasOne(d => d.MaKmNavigation).WithMany(p => p.ChiTietKhuyenMais)
                .HasForeignKey(d => d.MaKm)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ChiTietKhuyenMai_KhuyenMai1");

            entity.HasOne(d => d.MaSanPhamNavigation).WithMany(p => p.ChiTietKhuyenMais)
                .HasForeignKey(d => d.MaSanPham)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ChiTietKhuyenMai_SanPham");
        });

        modelBuilder.Entity<ChiTietNhapHang>(entity =>
        {
            entity.ToTable("ChiTietNhapHang");

            entity.Property(e => e.Gia).HasColumnType("decimal(12, 0)");
            entity.Property(e => e.TongTien).HasColumnType("decimal(12, 0)");

            entity.HasOne(d => d.MaNhaCungCapNavigation).WithMany(p => p.ChiTietNhapHangs)
                .HasForeignKey(d => d.MaNhaCungCap)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ChiTietNhapHang_NhaCungCap");

            entity.HasOne(d => d.MaNhapHangNavigation).WithMany(p => p.ChiTietNhapHangs)
                .HasForeignKey(d => d.MaNhapHang)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ChiTietNhapHang_NhapHang");

            entity.HasOne(d => d.MaSanPhamNavigation).WithMany(p => p.ChiTietNhapHangs)
                .HasForeignKey(d => d.MaSanPham)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ChiTietNhapHang_SanPham");
        });

        modelBuilder.Entity<ChiTietPhanTram>(entity =>
        {
            entity.ToTable("ChiTietPhanTram");

            entity.Property(e => e.NgayBatDau).HasColumnType("date");
            entity.Property(e => e.NgayKetThuc).HasColumnType("date");

            entity.HasOne(d => d.MaPhanTramNavigation).WithMany(p => p.ChiTietPhanTrams)
                .HasForeignKey(d => d.MaPhanTram)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ChiTietPhanTram_PhanTramGiaCa");

            entity.HasOne(d => d.MaSanPhamNavigation).WithMany(p => p.ChiTietPhanTrams)
                .HasForeignKey(d => d.MaSanPham)
                .HasConstraintName("FK_ChiTietPhanTram_SanPham");
        });

        modelBuilder.Entity<ChiTietSanPhamGoiY>(entity =>
        {
            entity.HasKey(e => new { e.MaSanPham, e.MaGoiY });

            entity.ToTable("ChiTietSanPhamGoiY");

            entity.HasIndex(e => e.Id, "IX_ChiTietSanPhamGoiY").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.HasOne(d => d.MaGoiYNavigation).WithMany(p => p.ChiTietSanPhamGoiYs)
                .HasForeignKey(d => d.MaGoiY)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChiTietSanPhamGoiY_SanPhamGoiY");

            entity.HasOne(d => d.MaSanPhamNavigation).WithMany(p => p.ChiTietSanPhamGoiYs)
                .HasForeignKey(d => d.MaSanPham)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChiTietSanPhamGoiY_SanPham");
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

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.DiaChi).HasMaxLength(200);
            entity.Property(e => e.NgayTao).HasColumnType("date");
            entity.Property(e => e.PhuongPhapThanhToan).HasMaxLength(70);

            entity.HasOne(d => d.User).WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__DonHang__UserId__5441852A");
        });

        modelBuilder.Entity<Favorite>(entity =>
        {
            entity.HasKey(e => new { e.ProductId, e.UserId });

            entity.ToTable("Favorite");

            entity.Property(e => e.FavoriteDate).HasColumnType("datetime");

            entity.HasOne(d => d.Product).WithMany(p => p.Favorites)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Favorite_SanPham");

            entity.HasOne(d => d.User).WithMany(p => p.Favorites)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Favorite_Users");
        });

        modelBuilder.Entity<Kho>(entity =>
        {
            entity.HasKey(e => e.Serial);

            entity.ToTable("Kho");

            entity.Property(e => e.TrangThai).HasMaxLength(50);

            entity.HasOne(d => d.MaSanPhamNavigation).WithMany(p => p.Khos)
                .HasForeignKey(d => d.MaSanPham)
                .HasConstraintName("FK_Kho_SanPham");
        });

        modelBuilder.Entity<KhuyenMai>(entity =>
        {
            entity.ToTable("KhuyenMai");

            entity.Property(e => e.TenKm)
                .HasMaxLength(50)
                .HasColumnName("TenKM");

            entity.HasOne(d => d.User).WithMany(p => p.KhuyenMais)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_KhuyenMai_Users");
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

            entity.Property(e => e.NgayNhapHang).HasColumnType("date");

            entity.HasOne(d => d.User).WithMany(p => p.NhapHangs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__NhapHang__UserId__54CB950F");
        });

        modelBuilder.Entity<PhanTramGiaCa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_PhanTramGiaCa_1");

            entity.ToTable("PhanTramGiaCa");

            entity.HasOne(d => d.User).WithMany(p => p.PhanTramGiaCas)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_PhanTramGiaCa_Users");
        });

        modelBuilder.Entity<QuanLySerial>(entity =>
        {
            entity.HasKey(e => e.SerialId);

            entity.ToTable("QuanLySerial");

            entity.Property(e => e.SerialId).ValueGeneratedNever();

            entity.HasOne(d => d.MaChiTietDonHangNavigation).WithMany(p => p.QuanLySerials)
                .HasForeignKey(d => d.MaChiTietDonHang)
                .HasConstraintName("FK_QuanLySerial_ChiTietDonHang");

            entity.HasOne(d => d.Serial).WithOne(p => p.QuanLySerial)
                .HasForeignKey<QuanLySerial>(d => d.SerialId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuanLySerial_Kho");
        });

        modelBuilder.Entity<SanPham>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SanPham__3214EC07A00E0847");

            entity.ToTable("SanPham");

            entity.Property(e => e.Gia).HasColumnType("decimal(12, 0)");
            entity.Property(e => e.GiaSi).HasColumnType("decimal(12, 0)");
            entity.Property(e => e.GiaTruocKhiGiamGia).HasColumnType("decimal(12, 0)");
            entity.Property(e => e.HinhAnh).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.TrangThai)
                .HasMaxLength(50)
                .IsFixedLength();

            entity.HasOne(d => d.TheLoai).WithMany(p => p.SanPhams)
                .HasForeignKey(d => d.TheLoaiId)
                .HasConstraintName("FK_SanPham_LoaiSanPham");
        });

        modelBuilder.Entity<SanPhamGoiY>(entity =>
        {
            entity.ToTable("SanPhamGoiY");

            entity.Property(e => e.NgayTao).HasColumnType("datetime");
            entity.Property(e => e.Ten).HasMaxLength(100);
            entity.Property(e => e.TrangThai).HasMaxLength(100);
        });

        modelBuilder.Entity<SoDiaChi>(entity =>
        {
            entity.ToTable("SoDiaChi");

            entity.Property(e => e.QuanHuyen).HasMaxLength(50);
            entity.Property(e => e.ThanhPho).HasMaxLength(50);
            entity.Property(e => e.Xa).HasMaxLength(50);

            entity.HasOne(d => d.User).WithMany(p => p.SoDiaChis)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_SoDiaChi_Users");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC078F80E094");

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
