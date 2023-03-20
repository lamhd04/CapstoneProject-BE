namespace CapstoneProject_BE.Models
{
    public class AvailableForReturns
    {
        public int Id { get; set; }
        public int? ImportId { get; set; }
        public int? ExportId { get; set; }
        public int ProductId { get; set; }
        public int Available { get; set; }
        public virtual ImportOrder ImportOrder { get; set; }
        public virtual ExportOrder ExportOrder { get; set; }
        public virtual Product Product { get; set; }
    }
}
