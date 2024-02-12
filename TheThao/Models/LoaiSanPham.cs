using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TheThao.Models
{
    [Table("LoaiSanPham")]
    public partial class LoaiSanPham
    {
        public LoaiSanPham()
        {
            SanPhams = new HashSet<SanPham>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string? TenLoaiSanPham { get; set; }

        [InverseProperty(nameof(SanPham.TheLoai))]
        public virtual ICollection<SanPham> SanPhams { get; set; }
    }
}
