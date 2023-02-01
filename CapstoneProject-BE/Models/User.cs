namespace CapstoneProject_BE.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public long Phone { get; set; }
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
        public virtual RefreshToken RefreshToken { get; set; }  

    }
}
