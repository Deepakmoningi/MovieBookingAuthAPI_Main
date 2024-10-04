using System.ComponentModel.DataAnnotations;

namespace MovieBookingAuthApi.Models
{
    public class Login
    {
        [Required]
        public string LoginId { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
