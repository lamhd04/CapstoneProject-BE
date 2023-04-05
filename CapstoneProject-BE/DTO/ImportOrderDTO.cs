using CapstoneProject_BE.Models;

namespace CapstoneProject_BE.DTO
{
    public class ImportOrderDTO
    {
        public int ImportId { get; set; }
        public string? ImportCode { get; set; }
        public int UserId { get; set; }
        public int SupplierId { get; set; }
        public int TotalAmount { get; set; }
        public float Total { get; set; }
        public float TotalCost { get; set; }
        public float Discount { get; set; }
        public float OtherExpense { get; set; }
        public float Paid { get; set; }
        public float InDebted { get; set; }
        public string? Note { get; set; }
        public int State { get; set; }
        public List<ImportDetailDTO> ImportOrderDetails { get; set; }
    }
}
