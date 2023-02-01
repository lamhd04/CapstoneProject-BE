namespace CapstoneProject_BE.Models
{
    public class RefreshToken
    {
        public int TokenId { get; set; }
        public int UserId { get; set; }
        public string JwtId { get; set; }
        public string Token { get; set; }
        public DateTime Created { get; set; }
        public DateTime ExpiredAt { get; set; }
        public bool IsRevoked { get; set; }
        public virtual User User { get; set; }


    }
}
