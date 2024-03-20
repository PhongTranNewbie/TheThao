namespace TheThao.Models
{
    public class Purchase
    {
        public int Id { get; set; }
        public int SanPhamId { get; set; }
        public SanPham SanPham { get; set; }
        public int? Quantity { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public decimal? Total { get; set; }

    }
}
