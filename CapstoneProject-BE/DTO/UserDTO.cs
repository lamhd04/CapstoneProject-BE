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
        public string? Email { get; set; }
        public string Password { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string? Phone { get; set; }
        public int RoleId { get; set; }
        public bool Status { get; set; }
    }
}
