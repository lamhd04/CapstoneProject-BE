using System.Text.Json.Serialization;

namespace CapstoneProject_BE.Models
{
    public class ImportOrderDetail
    {
        public int DetailId { get; set; }
        public int ImportId { get; set; }
        public int ProductId { get; set; }
        public int? MeasuredUnitId { get; set; }
        public int Amount { get; set; }
        public float CostPrice { get; set; }
        public float Discount { get; set; }
        [JsonIgnore]
        public virtual ImportOrder ImportOrder { get; set; }
        public virtual MeasuredUnit? MeasuredUnit { get; set; }
        public virtual Product Product { get; set; }
    }
}
