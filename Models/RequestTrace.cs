using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace AuthGold.Models
{
    public class RequestTrace
    {
        [Required(ErrorMessage="This field is required")]
        public string id { get; set; }

        [Required(ErrorMessage="This field is required")]
        public string clientCode { get; set; }

        [Required(ErrorMessage="This field is required")]
        public string httpMethod { get; set; }

        [Required(ErrorMessage="This field is required")]
        public string address { get; set; }

        [Required(ErrorMessage="This field is required")]
        public int httpStatusCode { get; set; }

        [Required(ErrorMessage="This field is required")]
        [DataType(DataType.Duration)]
        public TimeSpan elapsedTime { get; set; }

        [Required(ErrorMessage="This field is required")]
        public DateTime CreatedAt { get; set; }
        
        [Required(ErrorMessage="This field is required")]
        public DateTime UpdatedAt { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize<RequestTrace>(this);
        }
    }
}
