using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TheThao.Models
{
    [Table("ThanhToan")]
    public partial class ThanhToan
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Ngay { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string? PhuongPhapThanhToan { get; set; }
        public int? MaGioHang { get; set; }
        public int? UserId { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? TongTien { get; set; }

        [ForeignKey(nameof(MaGioHang))]
        [InverseProperty(nameof(GioHang.ThanhToans))]
        public virtual GioHang? MaGioHangNavigation { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("ThanhToans")]
        public virtual User? User { get; set; }
    }
}
