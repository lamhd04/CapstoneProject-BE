using System.Text.Json.Serialization;

namespace CapstoneProject_BE.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string? Description { get; set; } 
        [JsonIgnore]
        public ICollection<Product>? Products { get; set; }
        public int StorageId { get; set; }
        public Storage Storage { get; set; }
    }
}
