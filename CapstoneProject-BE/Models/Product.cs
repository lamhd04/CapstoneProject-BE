using System.ComponentModel.DataAnnotations;

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
        public float? CostPrice { get; set; }
        public float? SellingPrice { get; set; }
        public string? DefaultMeasuredUnit { get; set; }
        public int? InStock { get; set; }
        public float? StockPrice { get; set; }
        
        public string? Image { get; set; }
        public DateTime Created { get; set; }
        public bool? Status { get; set; }
        public ICollection<MeasuredUnit> MeasuredUnits { get; set; }
        public Category Category { get; set; }
        public Supplier Supplier { get; set; }
        public string? Barcode { get; set; }

    }
}
