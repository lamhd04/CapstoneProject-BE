namespace CapstoneProject_BE.Models
{
    public class StocktakeNote
    {
        public int StocktakeId { get; set; }
        public string StocktakeCode { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Canceled { get; set; }
        public DateTime? Updated { get; set; }
        public int State { get; set; }
        public int CreatedId { get; set; }
        public int? UpdatedId { get; set; }
        public int StorageId { get; set; }
        public string? Note { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual User UpdatedBy { get; set; }
        public virtual Storage Storage { get; set; }
        public virtual ICollection<StocktakeNoteDetail> StocktakeNoteDetails { get; set; }
    }
}
