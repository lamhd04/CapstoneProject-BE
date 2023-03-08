using CapstoneProject_BE.Models;

namespace CapstoneProject_BE.DTO
{
    public class ExportOrderDTO
    {
        public int ExportId { get; set; }
        public string ExportCode { get; set; }
        public int UserId { get; set; }
        public int TotalAmount { get; set; }
        public float Total { get; set; }
        public float TotalPrice { get; set; }
        public string? Note { get; set; }
        public int State { get; set; }
        public List<ExportDetailDTO> ExportOrderDetails { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public DateTime? DeniedDate { get; set; }
        public User? User { get; set; }

    }
}
