using System.ComponentModel.DataAnnotations;

namespace E_Invoice_API.Core.DTO.Request
{
    public class LoginRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
