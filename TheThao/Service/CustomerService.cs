using TheThao.Models;

namespace TheThao.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly TheThaoContext _context;

        public CustomerService(TheThaoContext context)
        {
            _context = context;
        }

        public List<User> GetAllCustomers()
        {
            // Lấy danh sách khách hàng từ cơ sở dữ liệu
            return _context.Users.Where(u => u.Role == "Customer").ToList();
        }
    }
}
