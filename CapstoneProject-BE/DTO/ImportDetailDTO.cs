using CapstoneProject_BE.Models;

namespace CapstoneProject_BE.DTO
{
    public class ImportDetailDTO
    {
        public int ImportId { get; set; }
        public int ProductId { get; set; }
        public int? MeasuredUnitId { get; set; }
        public int Amount { get; set; }
        public float CostPrice { get; set; }
        public float Discount { get; set; }
    }
}
