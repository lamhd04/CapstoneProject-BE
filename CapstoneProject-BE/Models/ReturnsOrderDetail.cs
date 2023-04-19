using System.Text.Json.Serialization;

namespace CapstoneProject_BE.Models
{
    public class ReturnsOrderDetail
    {
        public int ReturnsId { get; set; }
        public int DetailId { get; set; }
        public int ProductId { get; set; }
        public int? MeasuredUnitId { get; set; }
        public float Price { get; set; }
        public int Amount { get; set; }
        [JsonIgnore]
        public virtual ReturnsOrder ReturnsOrder { get; set; }
        public virtual Product Product { get; set; }
        public virtual MeasuredUnit? MeasuredUnit { get; set; }
    }
}
