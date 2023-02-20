namespace CapstoneProject_BE.DTO
{
    public class ImportDetailDTO
    {
        public int ProductId { get; set; }
        public int? MeasuredUnitId { get; set; }
        public int Amount { get; set; }
        public float CostPrice { get; set; }
        public float Discount { get; set; }
        public float Price { get; set; }
    }
}
