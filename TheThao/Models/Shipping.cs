using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TheThao.Models
{
    [Table("Shipping")]
    public partial class Shipping
    {
        public Shipping()
        {
            DonHangs = new HashSet<DonHang>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string? DiaChi { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Ngay { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? TongTien { get; set; }
        public int? UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty("Shippings")]
        public virtual User? User { get; set; }
        [InverseProperty(nameof(DonHang.Shipping))]
        public virtual ICollection<DonHang> DonHangs { get; set; }
    }
}
