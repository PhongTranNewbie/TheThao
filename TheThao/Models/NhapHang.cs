using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TheThao.Models
{
    [Table("NhapHang")]
    public partial class NhapHang
    {
        [Key]
        public int Id { get; set; }
        public int? MaSanPham { get; set; }
        public int? SoLuongNhap { get; set; }
        [Column(TypeName = "date")]
        public DateTime? NgayNhap { get; set; }
        public int? MaNhaCungCap { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? GiaNhap { get; set; }
        public int? UserId { get; set; }

        [ForeignKey(nameof(MaNhaCungCap))]
        [InverseProperty(nameof(NhaCungCap.NhapHangs))]
        public virtual NhaCungCap? MaNhaCungCapNavigation { get; set; }
        [ForeignKey(nameof(MaSanPham))]
        [InverseProperty(nameof(SanPham.NhapHangs))]
        public virtual SanPham? MaSanPhamNavigation { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("NhapHangs")]
        public virtual User? User { get; set; }
    }
}
