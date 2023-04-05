namespace CapstoneProject_BE.DTO
{
    public class ReturnsDTO
    {
        public int? ImportId { get; set; }
        public int? ExportId { get; set; }
        public int? SupplierId { get; set; }
        public int UserId { get; set; }
        public string? Note { get; set; }
        public string? Media { get; set; }
        public List<ReturnsDetailDTO> ReturnsOrderDetails { get; set; }
    }
}
