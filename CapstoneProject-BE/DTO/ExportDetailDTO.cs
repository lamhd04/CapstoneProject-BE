using CapstoneProject_BE.Models;

namespace CapstoneProject_BE.DTO
{
    public class ExportDetailDTO
    {
        public int ExportId { get; set; }
        public int ProductId { get; set; }
        public int? MeasuredUnitId { get; set; }
        public int Amount { get; set; }
        public float Discount { get; set; }
        public float Price { get; set; }
        public string? DefaultMeasuredUnit { get; set; }
        public virtual MeasuredUnit? MeasuredUnit { get; set; }
        public virtual Product? Product { get; set; }
    }
}
