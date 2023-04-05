namespace CapstoneProject_BE.Models
{
    public class ExportOrder
    {
        public int ExportId { get; set; }
        public string ExportCode { get; set; }
        public int UserId { get; set; }
        public int TotalAmount { get; set; }
        public float Total { get; set; }
        public float TotalPrice { get; set; }
        public string? Note { get; set; }
        public int State { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Approved { get; set; }
        public DateTime? Completed { get; set; }
        public DateTime? Denied { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<ExportOrderDetail> ExportOrderDetails { get; set; }
        public virtual ICollection<ReturnsOrder> ReturnsOrders { get; set; }
        public virtual ICollection<AvailableForReturns> AvailableForReturns { get; set; }
        public int StorageId { get; set; }
        public Storage Storage { get; set; }
    }
}
