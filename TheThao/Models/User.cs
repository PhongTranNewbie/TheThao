using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TheThao.Models
{
    public partial class User
    {
        public User()
        {
            DonHangs = new HashSet<DonHang>();
            GioHangs = new HashSet<GioHang>();
            NhapHangs = new HashSet<NhapHang>();
            Shippings = new HashSet<Shipping>();
            ThanhToans = new HashSet<ThanhToan>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string? Ten { get; set; }
        [Column(TypeName = "date")]
        public DateTime? NgayThangNamSinh { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string? DiaChi { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string? Email { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string? Phone { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string? Username { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string? Password { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string? Role { get; set; }

        [InverseProperty(nameof(DonHang.User))]
        public virtual ICollection<DonHang> DonHangs { get; set; }
        [InverseProperty(nameof(GioHang.User))]
        public virtual ICollection<GioHang> GioHangs { get; set; }
        [InverseProperty(nameof(NhapHang.User))]
        public virtual ICollection<NhapHang> NhapHangs { get; set; }
        [InverseProperty(nameof(Shipping.User))]
        public virtual ICollection<Shipping> Shippings { get; set; }
        [InverseProperty(nameof(ThanhToan.User))]
        public virtual ICollection<ThanhToan> ThanhToans { get; set; }
    }
}
