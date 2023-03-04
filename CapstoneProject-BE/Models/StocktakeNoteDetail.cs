﻿namespace CapstoneProject_BE.Models
{
    public class StocktakeNoteDetail
    {
        public int DetailId { get; set; }
        public int StocktakeId { get; set; }
        public int ProductId { get; set; }
        public int? MeasuredUnitId { get; set; }
        public int CurrentStock { get; set; }
        public int ActualStock { get; set; }
        public int AmountDifferential { get; set; }
        public string? Note { get; set; }
        public virtual StocktakeNote StocktakeNote { get; set; }
        public virtual Product Product { get; set; }
        public virtual MeasuredUnit MeasuredUnit { get; set; }
    }
}