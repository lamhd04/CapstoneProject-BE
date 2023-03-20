namespace CapstoneProject_BE.Models
{
    public class ImportOrder
    {
        public int ImportId { get; set; }
        public string ImportCode { get; set; }
        public int UserId { get; set; }
        public int SupplierId { get; set; }
        public int TotalAmount { get; set; }
        public float Total { get; set; }
        public float TotalCost { get; set; }
        public float OtherExpense { get; set; }
        public float Paid { get; set; }
        public float InDebted { get; set; }
        public string? Note { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Approved { get; set; }
        public DateTime? Completed { get; set; }
        public DateTime? Denied { get; set; }
        public int State { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<ImportOrderDetail> ImportOrderDetails { get; set; }
        public virtual ICollection<ReturnsOrder> ReturnsOrders { get; set; }
        public virtual ICollection<AvailableForReturns> AvailableForReturns { get; set; }
        public virtual Supplier Supplier { get; set; }
        public int StorageId { get; set; }
        public Storage Storage { get; set; }
    }
}
