using AutoMapper;
using CapstoneProject_BE.AutoMapper;
using CapstoneProject_BE.DTO;
using CapstoneProject_BE.Helper;
using CapstoneProject_BE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CapstoneProject_BE.Controllers.Export
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExportController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly InventoryManagementContext _context;
        public IMapper mapper;
        public ExportController(InventoryManagementContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
            mapper = config.CreateMapper();
        }
        [HttpPut("UpdateExportOrder")]
        public async Task<IActionResult> UpdateExportOrder(ExportOrderDTO p)
        {
            try
            {

                if (p != null)
                {
                    //var storageid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "StorageId").Value);
                    var dbimport = _context.ExportOrders.Include(a => a.ExportOrderDetails).SingleOrDefault(a => a.ExportId == p.ExportId && a.StorageId == 1);
                    var result = mapper.Map<ExportOrder>(p);
                    if (dbimport.State == 0)
                    {
                        result.StorageId = 1;
                        result.Created = dbimport.Created;
                        _context.RemoveRange(dbimport.ExportOrderDetails);
                        await _context.SaveChangesAsync();
                        _context.ChangeTracker.Clear();
                        _context.Update(result);
                        await _context.SaveChangesAsync();
                        return Ok("Thành công");
                    }
                    else
                    {
                        return BadRequest("Không thể chỉnh sửa phiếu này");
                    }
                }
                else
                {
                    return BadRequest("Không có dữ liệu");
                }
            }
            catch
            {
                return StatusCode(500);
            }
        }
        [HttpPost("CreateExportOrder")]
        public async Task<IActionResult> CreateExportOrder(ExportOrderDTO p)
        {
            try
            {
                if (p != null)
                {
                    //var storageid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "StorageId").Value);
                    var result = mapper.Map<ExportOrder>(p);
                    result.Created = DateTime.Now;
                    result.State = 0;
                    var code = _context.ImportOrders.Where(x => x.StorageId == 1).Count() + 1;
                    result.ExportCode ="XAHA"+code;
                    result.StorageId = 1;
                    _context.Add(result);
                    await _context.SaveChangesAsync();
                    return Ok("Thành công");
                }
                else
                {
                    return BadRequest("Không có dữ liệu");
                }
            }
            catch
            {
                return StatusCode(500);
            }
        }
        [HttpPost("ApproveExport")]
        public async Task<IActionResult> ApproveExport(int exportId)
        {
            try
            {
                var result = await _context.ExportOrders.SingleOrDefaultAsync(x => x.ExportId == exportId);
                if (result != null && result.State == 0)
                {
                    result.State = 1;
                    result.Approved = DateTime.Now;
                    await _context.SaveChangesAsync();
                    return Ok("Thành công");
                }
                else
                {
                    return BadRequest("Không có dữ liệu");
                }
            }
            catch
            {
                return StatusCode(500);
            }
        }
        [HttpGet("GetExportOrder")]
        public async Task<IActionResult> GetExport(int offset, int limit, int? supId = 0, int? state = -1, string? code = "")
        {
            try
            {
                //var storageid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "StorageId").Value);
                var result = await _context.ExportOrders
                    .Where(x => (x.ExportCode.Contains(code) || code == "")
                 && (x.State == state || state == -1)
                 ).ToListAsync();
                if (limit > result.Count() && offset >= 0)
                {
                    return Ok(new ResponseData<ExportOrder>
                    {
                        Data = result.Skip(offset).Take(result.Count()).OrderByDescending(x => x.Created).ToList(),
                        Offset = offset,
                        Limit = limit,
                        Total = result.Count()
                    });
                }
                else if (offset >= 0)
                {
                    return Ok(new ResponseData<ExportOrder>
                    {
                        Data = result.Skip(offset).Take(limit).OrderByDescending(x => x.Created).ToList(),
                        Offset = offset,
                        Limit = limit,
                        Total = result.Count()
                    });
                }
                else
                {
                    return NotFound("Không kết quả");
                }
            }
            catch
            {
                return StatusCode(500);
            }
        }
        [HttpGet("GetExportDetail")]
        public async Task<IActionResult> GetExportDetail(int exportId)
        {
            try
            {
                //var storageid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "StorageId").Value);
                var result = await _context.ExportOrders
                    .Include(x => x.ExportOrderDetails).ThenInclude(x => x.Product).ThenInclude(x=>x.MeasuredUnits).Include(x => x.User)
                    .SingleOrDefaultAsync(x => x.ExportId == exportId);
                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest("Không có dữ liệu");
                }
            }
            catch
            {
                return StatusCode(500);
            }
        }
        [HttpGet("GetExportDetail")]
        public async Task<IActionResult> GetExportDetail(string exportcode)
        {
            try
            {
                //var storageid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "StorageId").Value);
                var result = await _context.ExportOrders
                    .Include(x => x.ExportOrderDetails).ThenInclude(x => x.Product).ThenInclude(x => x.MeasuredUnits).Include(x => x.User)
                    .SingleOrDefaultAsync(x => x.ExportCode == exportcode&&x.StorageId==1);
                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest("Không có dữ liệu");
                }
            }
            catch
            {
                return StatusCode(500);
            }
        }
        [HttpPost("DenyImport")]
        public async Task<IActionResult> DenyExport(int exportId)
        {
            try
            {
                //var storageid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "StorageId").Value);
                var result = await _context.ExportOrders.SingleOrDefaultAsync(x => x.ExportId == exportId);
                if (result != null && result.State == 0)
                {
                    result.State = 3;
                    result.Denied = DateTime.Now;
                    await _context.SaveChangesAsync();
                    return Ok("Thành công");
                }
                else
                {
                    return BadRequest("Không có dữ liệu");
                }
            }
            catch
            {
                return StatusCode(500);
            }
        }
        [HttpPost("Export")]
        public async Task<IActionResult> Export(int exportId)
        {
            try
            {
                //var storageid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "StorageId").Value);
                var result = await _context.ExportOrders.Include(a => a.ExportOrderDetails).SingleOrDefaultAsync(x => x.ExportId == exportId);
                if (result != null && result.State == 1)
                {
                    result.State = 2;
                    result.Completed = DateTime.Now;
                    foreach (var detail in result.ExportOrderDetails)
                    {
                        var product = await _context.Products.SingleOrDefaultAsync(x => x.ProductId == detail.ProductId);
                        var history = new ProductHistory
                        {
                            ProductId = product.ProductId,
                            ActionId = 2
                        };
                        int total = 0;
                        if (detail.MeasuredUnitId != null)
                        {
                            detail.MeasuredUnit = await _context.MeasuredUnits.SingleOrDefaultAsync(x => x.MeasuredUnitId == detail.MeasuredUnitId);
                            total = detail.Amount * detail.MeasuredUnit.MeasuredUnitValue;
                            if (total > product.InStock)
                                return BadRequest("Số lượng xuất lớn hơn tồn kho");
                            product.InStock -= total;
                        }
                        else
                        {
                            total = detail.Amount;
                            history.AmountDifferential = $"-{detail.Amount}";
                            if (total > product.InStock)
                                return BadRequest("Số lượng xuất lớn hơn tồn kho");
                            product.InStock -= total;
                        }
                        history.AmountDifferential = $"-{total}";
                        history.Price = product.SellingPrice;
                        product.SellingPrice = (detail.Amount * detail.Price + product.InStock * product.SellingPrice) / (total + product.InStock);
                        var pricedifferential = product.SellingPrice - history.Price;
                        if (pricedifferential > 0)
                            history.PriceDifferential = $"+{pricedifferential}";
                        else if (pricedifferential < 0)
                            history.PriceDifferential = $"-{pricedifferential}";
                        else
                            history.PriceDifferential = null;
                        history.ActionCode = result.ExportCode;
                        history.UserId = result.UserId;
                        history.Amount = product.InStock;
                        history.Date = DateTime.Now;
                        _context.Add(history);
                    }
                    await _context.SaveChangesAsync();
                    return Ok("Thành công");
                }
                else
                {
                    return BadRequest("Không có dữ liệu");
                }
            }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}
