using CapstoneProject_BE.Models;

namespace CapstoneProject_BE.DTO
{
    public class StocktakeDTO
    {
        public int StocktakeId { get; set; }
        public string StocktakeCode { get; set; }
        public int StorageId { get; set; }
        public int CreatedId { get; set; }
        public string? Note { get; set; }
        public List<StocktakeDetailDTO> StocktakeNoteDetails { get; set; }
    }
}
