using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TheThao.Models
{
    [Table("SanPham")]
    public partial class SanPham
    {
        public SanPham()
        {
            ChiTietGioHangs = new HashSet<ChiTietGioHang>();
            NhapHangs = new HashSet<NhapHang>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string? Name { get; set; }
        [Column(TypeName = "text")]
        public string? ChiTiet { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? Gia { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string? HinhAnh { get; set; }
        public int? TheLoaiId { get; set; }

        [ForeignKey(nameof(TheLoaiId))]
        [InverseProperty(nameof(LoaiSanPham.SanPhams))]
        public virtual LoaiSanPham? TheLoai { get; set; }
        [InverseProperty("MaSanPhamNavigation")]
        public virtual KhoHang KhoHang { get; set; } = null!;
        [InverseProperty(nameof(ChiTietGioHang.IdSanPhamNavigation))]
        public virtual ICollection<ChiTietGioHang> ChiTietGioHangs { get; set; }
        [InverseProperty(nameof(NhapHang.MaSanPhamNavigation))]
        public virtual ICollection<NhapHang> NhapHangs { get; set; }
    }
}
