using System.ComponentModel.DataAnnotations;

namespace CapstoneProject_BE.DTO
{
    public class LoginModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
