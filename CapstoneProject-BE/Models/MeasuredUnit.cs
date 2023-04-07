using System.Text.Json.Serialization;

namespace CapstoneProject_BE.Models
{
    public class MeasuredUnit
    {
        public int MeasuredUnitId { get; set; }
        public int ProductId { get; set; }
        public string MeasuredUnitName { get; set; }
        public int MeasuredUnitValue { get; set; }
        public float? SuggestedPrice { get; set; }
        [JsonIgnore]
        public virtual Product Product { get; set; }
        [JsonIgnore]
        public virtual ICollection<ImportOrderDetail> ImportOrderDetails { get; set; }
        [JsonIgnore]
        public virtual ICollection<ExportOrderDetail> ExportOrderDetails { get; set; }
        [JsonIgnore]
        public virtual ICollection<ReturnsOrderDetail> ReturnsOrderDetails { get; set; }
    }
}
