namespace TheThao.Services
{
    public interface IPromotionService
    {
        Task ApplyPromotionAsync(int productId, int percentDiscount);
    }

}
