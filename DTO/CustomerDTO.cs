
using System;
using AuthGold.Types;

namespace AuthGold.DTO
{
    public class CustomerDTO
    {
        public Guid Id { get; set; }
        //public CPF Cpf { get; set; } = "00000000000";
        public string FirstName { get;set; }
        public string LastName { get;set; }
        public int BuyAmout { get; set; }
    }
}
