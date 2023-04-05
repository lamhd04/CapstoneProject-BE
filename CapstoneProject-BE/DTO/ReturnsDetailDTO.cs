namespace CapstoneProject_BE.DTO
{
    public class ReturnsDetailDTO
    {
        public int ReturnsId { get; set; }
        public int ProductId { get; set; }
        public int? MeasuredUnitId { get; set; }
        public int Amount { get; set; }
        public float Price { get; set; }
    }
}
