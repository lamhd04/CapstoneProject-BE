using System.Text.Json.Serialization;

namespace CapstoneProject_BE.Models
{
    public class ExportOrderDetail
    {
        public int DetailId { get; set; }
        public int ExportId { get; set; }
        public int ProductId { get; set; }
        public int? MeasuredUnitId { get; set; }
        public int Amount { get; set; }
        public float Discount { get; set; }
        public float Price { get; set; }
        [JsonIgnore]
        public virtual ExportOrder ExportOrder { get; set; }
        public virtual MeasuredUnit? MeasuredUnit { get; set; }
        public virtual Product Product { get; set; }
    }
}
