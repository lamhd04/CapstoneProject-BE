using AutoMapper;
using CapstoneProject_BE.AutoMapper;
using CapstoneProject_BE.DTO;
using CapstoneProject_BE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CapstoneProject_BE.Controllers.Returns
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
                var storageid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "StorageId").Value);
                var userid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "UserId").Value);
                if (p != null)
                {
                    var result = mapper.Map<ReturnsOrder>(p);
                    result.Created = DateTime.Now;
                    result.State = 0;
                    var code = (_context.ReturnsOrders.Where(x => x.StorageId == storageid).Count() + 1);
                    if (result.ImportId == null)
                    {
                        result.ReturnsCode = "HOHA" + code;
                    }
                    else
                    {
                        result.ReturnsCode = "TAHA" + code;
                    }
                    result.StorageId = storageid;
                    
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
                                var order = _context.ExportOrderDetails.SingleOrDefault(x => x.ProductId == a.ProductId && x.ExportId == afr.ExportId);
                                afr.Available = order.Amount - a.Amount;
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
                                var order = _context.ImportOrderDetails.SingleOrDefault(x => x.ProductId == a.ProductId && x.ImportId == afr.ImportId);
                                afr.Available = order.Amount - a.Amount;
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
                            var product = _context.Products.SingleOrDefault(x => x.ProductId == a.ProductId);
                            if (a.MeasuredUnitId != null)
                            {
                                a.MeasuredUnit = await _context.MeasuredUnits.SingleOrDefaultAsync(x => x.MeasuredUnitId == a.MeasuredUnitId);
                                product.InStock -= a.Amount * a.MeasuredUnit.MeasuredUnitValue;
                            }
                            else
                            {
                                product.InStock -= a.Amount;
                            }
                            var history = new ProductHistory
                            {
                                ProductId=a.ProductId,
                                ActionCode = result.ReturnsCode,
                                ActionId=6,
                                Amount=product.InStock,
                                AmountDifferential=a.MeasuredUnitId!=null?$"-{a.Amount * a.MeasuredUnit.MeasuredUnitValue}":$"-{a.Amount}",
                                UserId=userid,
                                Note=result.Note,
                                Date=DateTime.Now
                            };
                            _context.Add(history);
                            await _context.SaveChangesAsync();
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
        public async Task<IActionResult> Get(int offset, int limit,string type, int? supId = 0, string? code = "")
        {
            try
            {
                var storageid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "StorageId").Value);
                List<ReturnsOrder> result = new List<ReturnsOrder>();
                if (type == "import")
                {
                    result = await _context.ReturnsOrders.Include(x => x.User).Include(x => x.Supplier)
                        .Where(x => (x.ReturnsCode.Contains(code) || x.User.UserName.Contains(code) || x.Supplier.SupplierName.Contains(code))
                    && (x.SupplierId == supId || supId == 0) && x.ExportId == null && x.StorageId== storageid
                     ).OrderByDescending(x => x.Created).ToListAsync();
                }
                else
                {
                    result = await _context.ReturnsOrders.Include(x => x.User)
    .Where(x => (x.ReturnsCode.Contains(code) || x.User.UserName.Contains(code)) && x.ImportId == null &&x.StorageId== storageid
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
                var storageid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "StorageId").Value);
                var result = await _context.ReturnsOrders
                    .Include(x => x.ReturnsOrderDetails).ThenInclude(x => x.MeasuredUnit).Include(x => x.Supplier).Include(x => x.User)
                    .SingleOrDefaultAsync(x => x.ReturnsId == returnid && x.StorageId == storageid);

                if (result != null)
                {
                    return Ok(new
                    {
                        ReturnId = result.ReturnsId,
                        ImportId = result.ImportId,
                        ExportId = result.ExportId,
                        SupplierId = result.SupplierId,
                        UserId = result.UserId,
                        Created = result.Created,
                        Imported = result.Imported,
                        Note = result.Note,
                        Media = result.Media,
                        Total = result.Total,
                        StorageId = result.StorageId,
                        ReturnsCode = result.ReturnsCode,
                        State = result.State,
                        Supplier = result.SupplierId != null ? new
                        {
                            SupplierId = result.SupplierId,
                            SupplierName = result.Supplier.SupplierName,
                            SupplierPhone = result.Supplier.SupplierPhone
                        } : null,
                        User = new
                        {
                            UserId = result.UserId,
                            UserName = result.User.UserName
                        },
                        ReturnsOrderDetail = from i in result.ReturnsOrderDetails
                                             join p in _context.Products.Include(x => x.MeasuredUnits)
                                             on i.ProductId equals p.ProductId
                                             select new
                                             {
                                                 ReturnsId = i.ReturnsId,
                                                 ProductId = i.ProductId,
                                                 Price = i.Price,
                                                 Amount = i.Amount,
                                                 DefaultMeasuredUnit = i.Product.DefaultMeasuredUnit,
                                                 Product = i.Product,
                                                 MeasuredUnit = i.MeasuredUnit,
                                                 MeasuredUnitId = i.MeasuredUnitId
                                             }
                    });
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
                var storageid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "StorageId").Value);
                var result = await _context.ReturnsOrders
                    .Include(x => x.ReturnsOrderDetails).ThenInclude(x=>x.MeasuredUnit)
                    .Include(x => x.Supplier).Include(x => x.User)
                    .SingleOrDefaultAsync(x => x.ReturnsCode == returncode && x.StorageId == storageid);

                if (result != null)
                {
                    return Ok(new
                    {
                        ReturnsId = result.ReturnsId,
                        ImportId = result.ImportId,
                        ExportId = result.ExportId,
                        SupplierId = result.SupplierId,
                        UserId = result.UserId,
                        Created = result.Created,
                        Imported = result.Imported,
                        Note = result.Note,
                        Media = result.Media,
                        Total = result.Total,
                        StorageId = result.StorageId,
                        ReturnsCode = result.ReturnsCode,
                        State = result.State,
                        Supplier = result.SupplierId != null ? new
                        {
                            SupplierId = result.SupplierId,
                            SupplierName = result.Supplier.SupplierName,
                            SupplierPhone = result.Supplier.SupplierPhone
                        } : null,
                        User = new
                        {
                            UserId = result.UserId,
                            UserName = result.User.UserName
                        },
                        ReturnsOrderDetail = from i in result.ReturnsOrderDetails
                                             join p in _context.Products.Include(x => x.MeasuredUnits)
                                             on i.ProductId equals p.ProductId
                                             select new
                                             {
                                                 ReturnsId = i.ReturnsId,
                                                 ProductId = i.ProductId,
                                                 Price=i.Price,
                                                 Amount=i.Amount,
                                                 DefaultMeasuredUnit = p.DefaultMeasuredUnit,
                                                 Product = p,
                                                 MeasuredUnit = i.MeasuredUnit,
                                                 MeasuredUnitId = i.MeasuredUnitId
                                             }
                    });
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
        [HttpPost("ReImport")]
        public async Task<IActionResult> ReImport(int returnid)
        {
            try
            {
                var userid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "UserId").Value);
                var storageid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "StorageId").Value);
                //var result = from s in _context.ReturnsOrderDetails
                //             join a in _context.MeasuredUnits on s.MeasuredUnitId equals a.MeasuredUnitId
                //             where s.ReturnsId==returnid
                //             select new
                //             {
                //                 MeasuredUnit=a,
                //                 MeasuredUnitId=a.MeasuredUnitId,
                //                 ProductId=s.ProductId,
                //                 Amount=s.Amount
                //             };
                var order = await _context.ReturnsOrders
                    .Include(x => x.ReturnsOrderDetails).ThenInclude(x => x.Product).ThenInclude(x => x.MeasuredUnits)
                    .SingleOrDefaultAsync(x => x.ReturnsId == returnid && x.StorageId == storageid);
                if (order != null&&order.State==0)
                {
                    order.State = 1;
                    order.Imported = DateTime.Now;
                    foreach (var a in order.ReturnsOrderDetails)
                    {
                        var product = _context.Products.SingleOrDefault(x => x.ProductId == a.ProductId);
                        if (a.MeasuredUnit != null)
                        {
                            product.InStock += a.Amount * a.MeasuredUnit.MeasuredUnitValue;
                        }
                        else
                        {
                            product.InStock += a.Amount;
                        }
                        var history = new ProductHistory
                        {
                            ProductId = a.ProductId,
                            ActionCode = order.ReturnsCode,
                            ActionId = 7,
                            Amount=product.InStock,
                            AmountDifferential = a.MeasuredUnitId != null ? $"+{a.Amount * a.MeasuredUnit.MeasuredUnitValue}" : $"+{a.Amount}",
                            UserId = userid,
                            Date = DateTime.Now
                        };
                        _context.Add(history);
                        await _context.SaveChangesAsync();
                    }
                    return Ok();
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
