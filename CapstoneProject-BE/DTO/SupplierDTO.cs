using System.ComponentModel.DataAnnotations;

namespace CapstoneProject_BE.DTO
{
    public class SupplierDTO
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string SupplierPhone { get; set; }
        public bool? Status { get; set; }
        public City? City { get; set; }
        public District? District { get; set; }
        public Ward? Ward { get; set; }
        public string? Address { get; set; }
        public string? Note { get; set; }
        [DataType(DataType.EmailAddress)]
        public string? SupplierEmail { get; set; }
    }
}
