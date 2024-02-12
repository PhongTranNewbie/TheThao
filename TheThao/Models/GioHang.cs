using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TheThao.Models
{
    [Table("GioHang")]
    public partial class GioHang
    {
        public GioHang()
        {
            ChiTietGioHangs = new HashSet<ChiTietGioHang>();
            ThanhToans = new HashSet<ThanhToan>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string? MaGiaoDichUser { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string? MaGiaoDichMerchant { get; set; }
        public int? MaDonHang { get; set; }
        public int? UserId { get; set; }

        [ForeignKey(nameof(MaDonHang))]
        [InverseProperty(nameof(DonHang.GioHangs))]
        public virtual DonHang? MaDonHangNavigation { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("GioHangs")]
        public virtual User? User { get; set; }
        [InverseProperty(nameof(ChiTietGioHang.IdGioHangNavigation))]
        public virtual ICollection<ChiTietGioHang> ChiTietGioHangs { get; set; }
        [InverseProperty(nameof(ThanhToan.MaGioHangNavigation))]
        public virtual ICollection<ThanhToan> ThanhToans { get; set; }
    }
}
