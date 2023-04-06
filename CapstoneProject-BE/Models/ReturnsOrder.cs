namespace CapstoneProject_BE.Models
{
    public class ReturnsOrder
    {
        public int ReturnsId { get; set; }
        public int? ImportId { get; set; }
        public int? ExportId { get; set; }
        public int? SupplierId { get; set; }
        public int UserId { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Imported { get; set; }
        public string? Note { get; set; }
        public string? Media { get; set; }
        public float Total { get; set; }
        public int StorageId { get; set; }
        public string ReturnsCode { get; set; }
        public int State { get; set; }
        public virtual ImportOrder ImportOrder { get; set; }
        public virtual Storage Storage { get; set; }
        public virtual ExportOrder ExportOrder { get; set; }
        public virtual Supplier? Supplier { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<ReturnsOrderDetail> ReturnsOrderDetails { get; set; }
    }
}
