using System;
using AuthGold.DTO;
using AuthGold.Models;

namespace AuthGold.Controllers
{
    public class Converters
    {
        public static BookDTO BookItemDTO(Book bookItem) =>
            new BookDTO
            {
                ID = bookItem.ID,
                Name = bookItem.Name,
                Author = bookItem.Author
            };

        public static CustomerDTO CustomerItemDTO(Customer customerItem) =>
            new CustomerDTO
            {
                Id = customerItem.Id,
                FirstName = customerItem.FirstName,
                LastName = customerItem.LastName,
                BuyAmout = customerItem.BuyAmount
            };

    }
}