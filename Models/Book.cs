
using System.ComponentModel.DataAnnotations;
using AuthGold.Contracts;

namespace AuthGold.Models
{
    public class Book
    {
        [Key]
        public string ID { get; set; }
        [Required(ErrorMessage="This field must be fulled")]
        public string Name { get; set; }
        [Required(ErrorMessage="This field must be fulled")]
        public string Author { get; set; }
        [MaxLength(25)]
        [MinLength(25)]
        public string Secret { get; set; }
    }
}
