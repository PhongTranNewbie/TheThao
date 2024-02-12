using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TheThao.Models
{
    [Table("ChiTietGioHang")]
    public partial class ChiTietGioHang
    {
        [Key]
        public int Id { get; set; }
        public int? IdSanPham { get; set; }
        public int? IdGioHang { get; set; }
        public int? Quantity { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? Gia { get; set; }

        [ForeignKey(nameof(IdGioHang))]
        [InverseProperty(nameof(GioHang.ChiTietGioHangs))]
        public virtual GioHang? IdGioHangNavigation { get; set; }
        [ForeignKey(nameof(IdSanPham))]
        [InverseProperty(nameof(SanPham.ChiTietGioHangs))]
        public virtual SanPham? IdSanPhamNavigation { get; set; }
    }
}
