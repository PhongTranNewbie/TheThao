using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TheThao.Models
{
    [Table("NhaCungCap")]
    public partial class NhaCungCap
    {
        public NhaCungCap()
        {
            NhapHangs = new HashSet<NhapHang>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string? TenNhaCungCap { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string? DiaChi { get; set; }
        [Column("SDT")]
        [StringLength(20)]
        [Unicode(false)]
        public string? Sdt { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string? Email { get; set; }

        [InverseProperty(nameof(NhapHang.MaNhaCungCapNavigation))]
        public virtual ICollection<NhapHang> NhapHangs { get; set; }
    }
}
