using System;
using System.ComponentModel.DataAnnotations;

namespace AuthGold.Models
{
    public class Customer
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required(ErrorMessage="First field is required")]
        public string FirstName { get;set; }

        [Required(ErrorMessage="First field is required")]
        public string LastName { get;set; }

        public int BuyAmount { get; set; }

        public DateTime CreatedAt { get;set; }

        public DateTime UpdatedAt { get;set; }

        [Required(ErrorMessage="First field is required")]
        public string password { get;set; }
    }
}
