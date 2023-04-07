using System.ComponentModel.DataAnnotations;

namespace CapstoneProject_BE.DTO
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string? UserCode { get; set; }
        public string? UserName { get; set; }
        public string? Address { get; set; }
        public string? Identity { get; set; }
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string? Phone { get; set; }
        public int RoleId { get; set; }
        public bool? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Image { get; set; }
        public string? Password { get; set; }
    }
}
