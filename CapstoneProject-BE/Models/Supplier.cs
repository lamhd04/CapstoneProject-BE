
using System.Text.Json.Serialization;

namespace CapstoneProject_BE.Models
{
    public class Supplier
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string SupplierPhone { get; set; }
        public bool Status { get; set; }
        public string SupplierAddress { get; set; }
        public string SupplierEmail { get; set; }
        [JsonIgnore]
        public ICollection<Product> Products { get; set; }
    }
}
