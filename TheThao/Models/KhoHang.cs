using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TheThao.Models
{
    [Table("KhoHang")]
    public partial class KhoHang
    {
        [Key]
        public int MaSanPham { get; set; }
        public int? SoLuong { get; set; }

        [ForeignKey(nameof(MaSanPham))]
        [InverseProperty(nameof(SanPham.KhoHang))]
        public virtual SanPham MaSanPhamNavigation { get; set; } = null!;
    }
}
