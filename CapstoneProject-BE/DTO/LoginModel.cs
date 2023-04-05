using System.ComponentModel.DataAnnotations;

namespace CapstoneProject_BE.DTO
{
    public class LoginModel
    {
        public string? Usercode { get; set; }
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
