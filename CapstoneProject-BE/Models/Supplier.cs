
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CapstoneProject_BE.Models
{
    public class Supplier
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string SupplierPhone { get; set; }
        public bool Status { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public string? Address { get; set; }
        public string? Note { get; set; }
        [DataType(DataType.EmailAddress)]
        public string? SupplierEmail { get; set; }
        [JsonIgnore]
        public virtual ICollection<Product>? Products { get; set; }
        [JsonIgnore]
        public virtual ICollection<ImportOrder> ImportOrders { get; set; }
        public int StorageId { get; set; }
        public Storage Storage { get; set; }
        [JsonIgnore]
        public virtual ICollection<ReturnsOrder> ReturnsOrders { get; set; }
    }
}
