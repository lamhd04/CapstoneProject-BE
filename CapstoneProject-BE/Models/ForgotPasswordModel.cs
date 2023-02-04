using System.ComponentModel.DataAnnotations;

namespace CapstoneProject_BE.Models
{
    public class ForgotPasswordModel
    {
        public int Id { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        public bool EmailSent { get; set; }
        public string Token { get; set; }
    }
}
