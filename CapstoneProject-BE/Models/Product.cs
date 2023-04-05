using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CapstoneProject_BE.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string? ProductCode { get; set; }
        public int? CategoryId { get; set; }
        public string? Description { get; set; }
        public int? SupplierId { get; set; }
        public float CostPrice { get; set; }
        public float SellingPrice { get; set; }
        public string? DefaultMeasuredUnit { get; set; }
        public int InStock { get; set; }
        public int? MinStock { get; set; }
        public int? MaxStock { get; set; }
        public float StockPrice { get; set; }
        
        public string? Image { get; set; }
        public DateTime Created { get; set; }
        public bool? Status { get; set; }
        [JsonIgnore]
        public virtual ICollection<ImportOrderDetail> ImportOrderDetails { get; set; }
        public virtual ICollection<MeasuredUnit> MeasuredUnits { get; set; }
        public virtual ICollection<ProductHistory> ProductHistories { get; set; }
        [JsonIgnore]
        public virtual ICollection<ExportOrderDetail> ExportOrderDetails { get; set; }
        public virtual ICollection<AvailableForReturns> AvailableForReturns { get; set; }
        public Category Category { get; set; }
        public Supplier Supplier { get; set; }
        public string? Barcode { get; set; }
        public int StorageId { get; set; }
        public Storage Storage { get; set; }

    }
}
