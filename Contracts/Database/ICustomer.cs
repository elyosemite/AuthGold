using AuthGold.Models;

namespace AuthGold.Contracts.Database
{
    public interface ICustomer
    {
        void Save(Customer customer);
    }
}