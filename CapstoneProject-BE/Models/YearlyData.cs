namespace CapstoneProject_BE.Models
{
    public class YearlyData
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public float Profit { get; set; }
        public float InventoryValue { get; set; }
        public int StorageId { get; set; }
        public virtual Storage Storage { get; set; }
    }
}
