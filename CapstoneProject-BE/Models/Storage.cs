using System.Text.Json.Serialization;

namespace CapstoneProject_BE.Models
{
    public class Storage
    {
        public int StorageId { get; set; }
        public string StorageName { get; set; }
        [JsonIgnore]
        public virtual ICollection<Supplier> Suppliers { get; set; }
        [JsonIgnore]
        public virtual ICollection<User> Users { get; set; }
        [JsonIgnore]
        public virtual ICollection<Product> Products { get; set; }
        [JsonIgnore]
        public virtual ICollection<ImportOrder> ImportOrders { get; set; }
        [JsonIgnore]
        public virtual ICollection<ExportOrder> ExportOrders { get; set; }
        [JsonIgnore]
        public virtual ICollection<Category> Categories { get; set; }
        [JsonIgnore]
        public virtual ICollection<StocktakeNote> StocktakeNotes { get; set; }
        public virtual ICollection<ReturnsOrder> ReturnsOrders { get; set; }
        public virtual ICollection<YearlyData> YearlyDatas { get; set; }
    }
}
