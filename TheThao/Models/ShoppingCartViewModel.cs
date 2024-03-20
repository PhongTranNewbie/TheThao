namespace TheThao.Models
{
    public class ShoppingCartViewModel
    {
        public List<ShoppingCartItem> CartItems { get; set; }
        public decimal? TotalPrice { get; set; }
        public int? TotalQuality { get; set; }
    }
}
