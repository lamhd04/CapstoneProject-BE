using CapstoneProject_BE.Models;

namespace CapstoneProject_BE.DTO
{
    public class ProductList
    {
        public List<Product> Data { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; }
        public int Total { get; set; }
    }
}
