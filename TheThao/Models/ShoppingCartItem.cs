namespace TheThao.Models
{
    public class ShoppingCartItem
    {
        public int Id { get; set; }
        public SanPham SanPham { get; set; }
        public int Quantity { get; set; }
    }
}