using System;
using AuthGold.Types;

namespace AuthGold.Models
{
    public class Customer
    {
        public Guid Id { get; set; }
        public CPF Cpf { get;set; } = "00000000000";
        public string FirstName { get;set; }
        public string LastName { get;set; }
        public int BuyAmout { get; set; }
        public Date CreatedAt { get;set; }
        public Date UpdatedAt { get;set; }
        public string password { get;set; }
    }
}
