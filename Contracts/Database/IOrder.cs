using AuthGold.Models;

namespace AuthGold.Contracts.Database
{
    public interface IOrder
    {
        void Save(Order order);
    }
}