using System;
using System.ComponentModel.DataAnnotations;
using AuthGold.Types;

namespace AuthGold.Models
{
    public class Customer
    {
        [Key]
        public Guid Id { get; set; }
        //public CPF Cpf { get;set; } = "00000000000";
        [Required(ErrorMessage="First field is required")]
        public string FirstName { get;set; }
        [Required(ErrorMessage="First field is required")]
        public string LastName { get;set; }
        public int BuyAmout { get; set; }
        public DateTime CreatedAt { get;set; }
        public DateTime UpdatedAt { get;set; }
        [Required(ErrorMessage="First field is required")]
        public string password { get;set; }
    }
}
