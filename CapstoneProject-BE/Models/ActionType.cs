using System.Text.Json.Serialization;

namespace CapstoneProject_BE.Models
{
    public class ActionType
    {
        public int ActionId { get; set; }
        public string Action { get; set; }
        public string? Description { get; set; }
        [JsonIgnore]
        public virtual ICollection<ProductHistory> ProductHistories { get; set; }
    }
}
