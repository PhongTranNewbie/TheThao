using TheThao.Models;
using X.PagedList;

namespace TheThao.ViewModel
{
    public class OrdersByStatusViewModel
    {
        public List<DonHang> WaitingForConfirmationOrders { get; set; }
        public List<DonHang> OnDeliveryOrders { get; set; }
        public List<DonHang> PaidOrders { get; set; }
        public List<DonHang> PaidAndAwaitingDeliveryOrders { get; set; }
        public List<DonHang> CancelledOrders { get; set; }
    }

}
