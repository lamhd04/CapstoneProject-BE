using CapstoneProject_BE.Models;
using System.ComponentModel.DataAnnotations;

namespace CapstoneProject_BE.DTO
{
    public class ProductDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string? ProductCode { get; set; }
        public int? CategoryId { get; set; }
        public string? Barcode { get; set; }
        public string? Description { get; set; }
        public int? SupplierId { get; set; }
        public float? CostPrice { get; set; }
        public float? SellingPrice { get; set; }
        public string? DefaultMeasuredUnit { get; set; }
        public int? MinStock { get; set; }
        public int? MaxStock { get; set; }
        public int? InStock { get; set; }
        public float? StockPrice { get; set; }
        [DataType(DataType.ImageUrl)]
        public string? Image { get; set; }
        public bool? Status { get; set; }
        public List<MeasuredUnitDTO>? MeasuredUnits { get; set; }
    }
}
