using AutoMapper;
using CapstoneProject_BE.Models;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace CapstoneProject_BE.Scheduler
{
    public class InventoryFinalization : IJob
    {
        public IConfiguration _configuration;
        private readonly InventoryManagementContext _context;
        public InventoryFinalization(InventoryManagementContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public Task Execute(IJobExecutionContext context)
        {
            var product = _context.Products.Where(x => x.StorageId == 1).ToList();
            var import = _context.ImportOrders.Where(x => x.StorageId == 1 && x.State == 2 && x.Completed.Value.Month == DateTime.Today.Month).ToList();
            var export = _context.ExportOrders.Where(x => x.StorageId == 1 && x.State == 2 && x.Completed.Value.Month == DateTime.Today.Month).ToList();
            var exportReturns = _context.ReturnsOrders.Where(x => x.StorageId == 1 && x.Created.Month==DateTime.Today.Month).ToList();
            YearlyData yd = new YearlyData();
            foreach(var a in product)
            {
                yd.InventoryValue += (float)(a.InStock * a.CostPrice);
            }
            foreach(var a in import)
            {
                yd.Profit -= a.Paid;
            }
            foreach(var a in export)
            {
                yd.Profit += a.TotalPrice;
            }
            foreach(var a in exportReturns)
            {
                yd.Profit -= a.Total;
            }
            yd.Year = DateTime.Today.Year;
            yd.Month = DateTime.Today.Month;
            yd.StorageId = 1;
            _context.Add(yd);
            _context.SaveChanges();
            return Task.FromResult(true);
        }
    }
}
