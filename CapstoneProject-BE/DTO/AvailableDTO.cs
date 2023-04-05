using CapstoneProject_BE.Models;

namespace CapstoneProject_BE.DTO
{
    public class AvailableDTO
    {
        public int ProductId { get; set; }
        public int? ExportId { get; set; }
        public int? ImportId { get; set; }
        public int Available { get; set; }
        public float Price { get; set; }
        public string DefaultMeasuredUnit { get; set; }
        public int? MeasuredUnitId { get; set; }
        public Product Product { get; set; }
        public MeasuredUnit? MeasuredUnit { get; set; }
    }
}
