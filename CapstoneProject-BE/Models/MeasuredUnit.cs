using System.Text.Json.Serialization;

namespace CapstoneProject_BE.Models
{
    public class MeasuredUnit
    {
        public int MeasuredUnitId { get; set; }
        public int ProductId { get; set; }
        public string MeasuredUnitName { get; set; }
        public int MeasuredUnitValue { get; set; }
        [JsonIgnore]
        public Product Product { get; set; }
    }
}
