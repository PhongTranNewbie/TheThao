using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TheThao.Models
{
    [Table("DonHang")]
    public partial class DonHang
    {
        public DonHang()
        {
            GioHangs = new HashSet<GioHang>();
        }

        [Key]
        public int Id { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Ngay { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string? TrangThai { get; set; }
        public int? UserId { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? TongTien { get; set; }
        public int? ShippingId { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string? DiaChi { get; set; }

        [ForeignKey(nameof(ShippingId))]
        [InverseProperty("DonHangs")]
        public virtual Shipping? Shipping { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("DonHangs")]
        public virtual User? User { get; set; }
        [InverseProperty(nameof(GioHang.MaDonHangNavigation))]
        public virtual ICollection<GioHang> GioHangs { get; set; }
    }
}
