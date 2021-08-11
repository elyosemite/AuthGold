using System;

namespace AuthGold.Models
{
    public class Order
    {
        public Guid Id { get; set; }  
        public string Number { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}