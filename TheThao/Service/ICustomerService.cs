using TheThao.Models;

namespace TheThao.Services
{
    public interface ICustomerService
    {
        List<User> GetAllCustomers();
    }
}