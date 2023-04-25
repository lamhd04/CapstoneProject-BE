using AutoMapper;
using CapstoneProject_BE.AutoMapper;
using CapstoneProject_BE.Models;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CapstoneProject_BE.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]

    public class DashboardController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly InventoryManagementContext _context;
        public IMapper mapper;
        public DashboardController(InventoryManagementContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
            mapper = config.CreateMapper();
        }
        [Authorize(Policy = "Manager")]
        [HttpGet("GetData")]
        public async Task<IActionResult> GetData()
        {
            try
            {
                var spent = 0f;
                var gain = 0f;
                var storageid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "StorageId").Value);
                var staff = _context.Users.Where(x => x.StorageId== storageid && x.RoleId!=1).Count();
                var product = _context.Products.Where(x => x.StorageId == storageid).Count();
                var import = _context.ImportOrders.Where(x => x.StorageId == storageid && x.State==2);
                var export = _context.ExportOrders.Where(x => x.StorageId == storageid && x.State ==2);
                var importReturn = _context.ReturnsOrders.Where(x => x.StorageId == storageid && x.SupplierId!=null);
                var exportReturn = _context.ReturnsOrders.Where(x => x.StorageId == storageid && x.SupplierId == null);
                foreach(var a in import)
                {
                    spent += a.Paid;
                }
                foreach(var a in exportReturn)
                {
                    spent += a.Total;
                }
                foreach(var a in export)
                {
                    gain += a.TotalPrice;
                }
                return Ok(new {
                    spent = spent,
                    gain = gain,
                    staff = staff,
                    import = import.Count(),
                    exportReturn = exportReturn.Count(),
                    importReturn = importReturn.Count(),
                    export=export.Count(),
                    product=product
                });
            }
            catch
            {
                return StatusCode(500);
            }
        }
        [Authorize(Policy = "Manager")]
        [HttpGet("GetChartData")]
        public async Task<IActionResult> GetChartData(int year)
        {
            try
            {
                var storageid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "StorageId").Value);
                var chartdata = await _context.YearlyDatas.Where(x => x.Year == year && x.StorageId == 1).ToListAsync();
                List<YearlyData> result = new List<YearlyData>();
                for(int i = 1; i <= 12; i++)
                {
                    var month = chartdata.SingleOrDefault(x=>x.Month==i);
                    if (month != null)
                    {
                        result.Add(month);
                    }
                    else if (i==DateTime.Now.Month)
                    {
                        YearlyData yd = new YearlyData();
                       var import = await _context.ImportOrders.Where(x => x.StorageId == storageid && x.Completed.Value.Month == DateTime.Today.Month &&x.Completed.Value.Year==year).ToListAsync();
                       var export = await _context.ExportOrders.Where(x => x.StorageId == storageid && x.Completed.Value.Month == DateTime.Today.Month && x.Completed.Value.Year == year).ToListAsync();
                       var exportReturn = await _context.ReturnsOrders.Where(x => x.StorageId == storageid && x.SupplierId == null && x.Created.Month == DateTime.Today.Month && x.Created.Year == year).ToListAsync();
                        var product = await _context.Products.Where(x => x.StorageId == 1).ToListAsync();
                        foreach(var a in import)
                        {
                            yd.Profit -= a.Total;
                        }
                        foreach(var a in export)
                        {
                            yd.Profit += a.TotalPrice;
                        }
                        foreach(var a in exportReturn)
                        {
                            yd.Profit -= a.Total;
                        }
                        if(DateTime.Now.Year==year)
                        foreach (var a in product)
                        {
                                yd.InventoryValue += (float)(a.InStock * a.CostPrice);
                        }
                        yd.Month = i;
                        yd.Year = year;
                        result.Add(yd);
                    }
                    else
                    {
                        result.Add(new YearlyData
                        {
                            Month = i,
                            Year = year
                        }); ;

                    }
                }
                return Ok(result);
            }
            catch
            {
                return StatusCode(500);
            }
        }
        [Authorize(Policy = "Manager")]
        [HttpGet("GetDataByTimePeriod")]
        public async Task<IActionResult> GetDataByTimePeriod(string timeperiod)
        {
            try
            {
                var profit = 0f;
                var newOrders = 0f;
                var newReturns = 0f;
                var cancelOrders = 0f;
                List<ImportOrder> import = null;
                List<ExportOrder> export = null;
                List<ReturnsOrder> exportReturn = null;
                var storageid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "StorageId").Value);
                switch (timeperiod)
                {
                    case "thisweek":
                        var monday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
                        import = await _context.ImportOrders.Where(x => x.StorageId == storageid && x.Completed>monday && x.Completed.Value.Year == DateTime.Today.Year).ToListAsync();
                        export = await _context.ExportOrders.Where(x => x.StorageId == storageid && x.Completed > monday && x.Completed.Value.Year == DateTime.Today.Year).ToListAsync();
                        exportReturn = await _context.ReturnsOrders.Where(x => x.StorageId == storageid && x.SupplierId == null && x.Created > monday && x.Created.Year == DateTime.Today.Year).ToListAsync();
                        break;
                    case "yesterday":
                        import = await _context.ImportOrders.Where(x => x.StorageId == storageid && x.Completed.Value.Day== DateTime.Today.AddDays(-1).Day && x.Completed.Value.Year == DateTime.Today.Year).ToListAsync();
                        export = await _context.ExportOrders.Where(x => x.StorageId == storageid && x.Completed.Value.Day == DateTime.Today.AddDays(-1).Day && x.Completed.Value.Year == DateTime.Today.Year).ToListAsync();
                        exportReturn = await _context.ReturnsOrders.Where(x => x.StorageId == storageid && x.SupplierId == null && x.Created.Day == DateTime.Today.AddDays(-1).Day && x.Created.Year == DateTime.Today.Year).ToListAsync();
                        break;
                    case "thismonth":
                        import = await _context.ImportOrders.Where(x => x.StorageId == storageid && x.Completed.Value.Month == DateTime.Today.Month && x.Completed.Value.Year == DateTime.Today.Year).ToListAsync();
                        export = await _context.ExportOrders.Where(x => x.StorageId == storageid && x.Completed.Value.Month == DateTime.Today.Month && x.Completed.Value.Year == DateTime.Today.Year).ToListAsync();
                        exportReturn = await _context.ReturnsOrders.Where(x => x.StorageId == storageid && x.SupplierId == null && x.Created.Month == DateTime.Today.Month).ToListAsync();
                        break;
                    default:
                        import = await _context.ImportOrders.Where(x => x.StorageId == storageid && x.Completed.Value.Day == DateTime.Today.Day && x.Completed.Value.Year == DateTime.Today.Year).ToListAsync();
                        export = await _context.ExportOrders.Where(x => x.StorageId == storageid && x.Completed.Value.Day == DateTime.Today.Day && x.Completed.Value.Year == DateTime.Today.Year).ToListAsync();
                        exportReturn = await _context.ReturnsOrders.Where(x => x.StorageId == storageid && x.SupplierId == null && x.Created.Day == DateTime.Today.Day && x.Created.Year == DateTime.Today.Year).ToListAsync();
                        break;
                }
                foreach (var a in import)
                {
                    if (a.State == 2)
                    {
                        profit -= a.Paid;
                        newOrders++;
                    }
                    else if (a.State == 3)
                    {
                        cancelOrders++;
                    }
                }
                foreach (var a in export)
                {
                    if (a.State == 2)
                    {
                        profit += a.TotalPrice;
                        newOrders++;
                    }
                    else if (a.State == 3)
                    {
                        cancelOrders++;
                    }
                }
                foreach (var a in exportReturn)
                {
                    profit-=a.Total;
                    newReturns++;
                }
                return Ok(new { 
                    profit,newOrders,cancelOrders,newReturns
                });
            }
            catch
            {
                return StatusCode(500);
            }
        }

    }
}
