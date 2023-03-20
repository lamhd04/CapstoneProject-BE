namespace CapstoneProject_BE.DTO
{
    public class PasswordDTO
    {
        public int? UserId { get; set; }
        public string Password { get; set; }
        public string? OldPassword { get; set; }
    }
}
