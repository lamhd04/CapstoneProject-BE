using AutoMapper;
using CapstoneProject_BE.AutoMapper;
using CapstoneProject_BE.DTO;
using CapstoneProject_BE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CapstoneProject_BE.Controllers.Returns
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReturnsController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly InventoryManagementContext _context;
        public IMapper mapper;
        public ReturnsController(InventoryManagementContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
            mapper = config.CreateMapper();
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] ReturnsDTO p)
        {
            try
            {
                //var storageid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "StorageId").Value);
                //var userid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "UserId").Value);
                if (p != null)
                {
                    var result = mapper.Map<ReturnsOrder>(p);
                    result.Created = DateTime.Now;
                    var code = (_context.ReturnsOrders.Where(x => x.StorageId == 1).Count() + 1);
                    if (result.ImportId == null)
                    {
                        result.ReturnsCode = "HOHA" + code;
                    }
                    else
                    {
                        result.ReturnsCode = "TAHA" + code;
                    }
                    result.StorageId = 1;
                    
                    foreach(var a in result.ReturnsOrderDetails)
                    {
                        result.Total += a.Price * a.Amount;
                        var afr=new AvailableForReturns
                        {
                            ProductId=a.ProductId,
                            ImportId=result.ImportId,
                            ExportId=result.ExportId
                        };
                        if (result.ImportId == null)
                        {
                            if (_context.AvailableForReturns.SingleOrDefault(x => x.ExportId == afr.ExportId&&x.ProductId==a.ProductId) == null)
                            {
                                afr.Available = _context.ExportOrderDetails.SingleOrDefault(x => x.ProductId == a.ProductId && x.ExportId == afr.ExportId).Amount - a.Amount;
                                _context.Add(afr);
                            }

                            else
                            {
                                var b = _context.AvailableForReturns.SingleOrDefault(x => x.ProductId == a.ProductId && x.ExportId == afr.ExportId);
                                afr.Available = b.Available - a.Amount;
                                afr.Id = b.Id;
                                _context.ChangeTracker.Clear();
                                _context.Update(afr);
                            }
                                
                        }
                        else
                        {
                            if (_context.AvailableForReturns.SingleOrDefault(x => x.ImportId == afr.ImportId && x.ProductId == a.ProductId) == null)
                            {
                                afr.Available = _context.ImportOrderDetails.SingleOrDefault(x => x.ProductId == a.ProductId && x.ImportId == afr.ImportId).Amount - a.Amount;
                                _context.Add(afr);
                            }
                            else
                            {
                                var b = _context.AvailableForReturns.SingleOrDefault(x => x.ProductId == a.ProductId && x.ImportId == afr.ImportId);
                                afr.Available = b.Available - a.Amount;
                                afr.Id = b.Id;
                                _context.ChangeTracker.Clear();
                                _context.Update(afr);
                            }
                        }
                    }
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
        [HttpGet("GetAvailable")]
        public async Task<IActionResult> GetAvailable(int importid=0,int exportid=0)
        {
            try
            {
                //var storageid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "StorageId").Value);
                if (importid == 0)
                {
                    var a = await _context.ExportOrderDetails.Include(x=>x.MeasuredUnit).Include(x=>x.Product).Where(x => x.ExportId == exportid).ToListAsync();
                    var result = new List<AvailableDTO>();
                    foreach (var b in a)
                    {
                        var c = new AvailableDTO
                        {
                            Product = b.Product,
                            ProductId = b.ProductId,
                            Price = b.Price,
                            MeasuredUnitId = b.MeasuredUnitId,
                            MeasuredUnit = b.MeasuredUnit,
                            ExportId = b.ExportId,
                            DefaultMeasuredUnit = b.Product.DefaultMeasuredUnit
                        };
                        var d = _context.AvailableForReturns.SingleOrDefault(x => x.ExportId == exportid && x.ProductId == b.ProductId);
                        if ( d== null)
                        {
                            c.Available = b.Amount;
                        }
                        else
                        {
                            c.Available = d.Available;
                        }
                        result.Add(c);
                    }
                    return Ok(result);
                }
                else
                {
                    var a = await _context.ImportOrderDetails.Include(x => x.MeasuredUnit).Include(x => x.Product).Where(x => x.ImportId == importid).ToListAsync();
                    var result = new List<AvailableDTO>();
                    foreach (var b in a)
                    {
                        var c = new AvailableDTO
                        {
                            Product = b.Product,
                            ProductId = b.ProductId,
                            Price = b.CostPrice,
                            MeasuredUnitId = b.MeasuredUnitId,
                            MeasuredUnit = b.MeasuredUnit,
                            ImportId = b.ImportId,
                            DefaultMeasuredUnit = b.Product.DefaultMeasuredUnit
                        };
                        var d = _context.AvailableForReturns.SingleOrDefault(x => x.ImportId == importid && x.ProductId == b.ProductId);
                        if (d == null)
                        {
                            c.Available = b.Amount;
                        }
                        else
                        {
                            c.Available = d.Available;
                        }
                        result.Add(c);
                    }
                    return Ok(result);
                }
                return BadRequest("Không có dữ liệu");
            }
            catch
            {
                return StatusCode(500);
            }
        }
        [HttpGet("Get")]
        public async Task<IActionResult> Get(int offset, int limit,string type, int? suppid = 0, string? code = "")
        {
            try
            {
                List<ReturnsOrder> result = new List<ReturnsOrder>();
                if (type == "import")
                {
                    result = await _context.ReturnsOrders.Include(x => x.User).Include(x => x.Supplier)
                        .Where(x => (x.ReturnsCode.Contains(code) || x.User.UserName.Contains(code) || x.Supplier.SupplierName.Contains(code))
                    && (x.SupplierId == suppid || suppid == 0) && x.ExportId == null
                     ).OrderByDescending(x => x.Created).ToListAsync();
                }
                else
                {
                    result = await _context.ReturnsOrders.Include(x => x.User)
    .Where(x => (x.ReturnsCode.Contains(code) || x.User.UserName.Contains(code)) && x.ImportId == null
 ).OrderByDescending(x => x.Created).ToListAsync();
                }

                if (limit > result.Count() && offset >= 0)
                {
                    return Ok(new ResponseData<ReturnsOrder>
                    {
                        Data = result.Skip(offset).Take(result.Count()).ToList(),
                        Offset = offset,
                        Limit = limit,
                        Total = result.Count()
                    });
                }
                else if (offset >= 0)
                {
                    return Ok(new ResponseData<ReturnsOrder>
                    {
                        Data = result.Skip(offset).Take(limit).ToList(),
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
        [HttpGet("GetReturnsDetail")]
        public async Task<IActionResult> GetDetail(int returnid)
        {
            try
            {
                //var storageid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "StorageId").Value);
                var result = await _context.ReturnsOrders
                    .Include(x => x.ReturnsOrderDetails).ThenInclude(x => x.Product).ThenInclude(x => x.MeasuredUnits)
                    .Include(x=>x.Supplier).Include(x=>x.User)
                    .SingleOrDefaultAsync(x => x.ReturnsId == returnid && x.StorageId == 1);

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
        [HttpGet("GetDetail")]
        public async Task<IActionResult> GetDetail(string returncode)
        {
            try
            {
                //var storageid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "StorageId").Value);
                var result = await _context.ReturnsOrders
                    .Include(x => x.ReturnsOrderDetails).ThenInclude(x => x.Product).ThenInclude(x => x.MeasuredUnits)
                    .Include(x => x.Supplier).Include(x => x.User)
                    .SingleOrDefaultAsync(x => x.ReturnsCode == returncode && x.StorageId == 1);

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
    
    }
}
