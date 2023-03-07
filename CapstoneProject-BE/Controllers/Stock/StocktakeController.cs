using AutoMapper;
using CapstoneProject_BE.AutoMapper;
using CapstoneProject_BE.DTO;
using CapstoneProject_BE.Helper;
using CapstoneProject_BE.Models;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Math;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CapstoneProject_BE.Controllers.Stock
{
    [Route("api/[controller]")]
    [ApiController]
    public class StocktakeController:ControllerBase
    {
        public IConfiguration _configuration;
        private readonly InventoryManagementContext _context;
        public IMapper mapper;
        public StocktakeController(InventoryManagementContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
            this.mapper = config.CreateMapper();
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] StocktakeDTO p)
        {
            try
            {
                //var storageid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "StorageId").Value);
                //var userid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "UserId").Value);
                if (p != null)
                {
                    var result = mapper.Map<StocktakeNote>(p);
                    result.Created = DateTime.Now;
                    result.State = 0;
                    var code = (_context.StocktakeNotes.Where(x => x.StorageId == 1).Count() + 1);
                    result.StocktakeCode = "KIHA" +code ;
                    result.StorageId = 1;
                    result.CreatedId = 1;
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
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody]StocktakeDTO p)
        {
            try
            {
                if (p != null)
                {
                    //var storageid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "StorageId").Value);
                    var dbimport = _context.StocktakeNotes.Include(a => a.StocktakeNoteDetails).SingleOrDefault(a => a.StocktakeId == p.StocktakeId && a.StorageId == 1);
                    var result = mapper.Map<StocktakeNote>(p);
                    if (dbimport.State == 0)
                    {
                        _context.RemoveRange(dbimport.StocktakeNoteDetails);
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
        [HttpPost("Stocktake")]
        public async Task<IActionResult> Stocktake(int stocktakeid)
        {
            try
            {
                //var updateid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "UserId").Value);
                var result = await _context.StocktakeNotes.Include(a => a.StocktakeNoteDetails).SingleOrDefaultAsync(x => x.StocktakeId == stocktakeid);
                if (result != null && result.State == 0)
                {
                    result.State = 1;
                    result.Updated = DateTime.Now;
                    result.UpdatedId = 1;
                    foreach (var detail in result.StocktakeNoteDetails)
                    {
                        var product = await _context.Products.SingleOrDefaultAsync(x => x.ProductId == detail.ProductId);
                        var history = new ProductHistory
                        {
                            ProductId = product.ProductId,
                            ActionId = 5
                        };
                        int total = 0;
                        if (detail.MeasuredUnitId != null)
                        {
                            detail.MeasuredUnit = await _context.MeasuredUnits.SingleOrDefaultAsync(x => x.MeasuredUnitId == detail.MeasuredUnitId);
                            total = detail.ActualStock * detail.MeasuredUnit.MeasuredUnitValue;
                        }
                        else
                        {
                            total = detail.ActualStock;
                        }
                        if (product.InStock != total)
                        {

                            history.AmountDifferential = product.InStock > total ? $"-{total}" : $"+{total}";
                                product.InStock = total;
                        }
                        else
                        {
                            return BadRequest("Số lượng không đổi");
                        }
                        history.ActionCode = result.StocktakeCode;
                        history.UserId = (int)result.UpdatedId;
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
        [HttpPost("Cancel")]
        public async Task<IActionResult> Cancel(int stocktakeid)
        {
            try
            {
                var result = await _context.StocktakeNotes.SingleOrDefaultAsync(x => x.StocktakeId == stocktakeid);
                if (result != null && result.State == 0)
                {
                    result.State = 2;
                    result.Canceled = DateTime.Now;
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
        [HttpGet("Get")]
        public async Task<IActionResult> Get(int offset, int limit, DateTime? created=null,DateTime? updated=null, int? state = -1, string? code = "")
        {
            try
            {
                var result = await _context.StocktakeNotes.Include(a => a.CreatedBy).Include(a=>a.UpdatedBy)
                    .Where(x => x.StocktakeCode.Contains(code)
                && (x.Created == created || created == null)&&(x.Updated==updated||updated==null) && (x.State == state || state == -1)
                 ).ToListAsync();
                if (limit > result.Count() && offset >= 0)
                {
                    return Ok(new ResponseData<StocktakeNote>
                    {
                        Data = result.Skip(offset).Take(result.Count()).OrderByDescending(x => x.Created).ToList(),
                        Offset = offset,
                        Limit = limit,
                        Total = result.Count()
                    });
                }
                else if (offset >= 0)
                {
                    return Ok(new ResponseData<StocktakeNote>
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
        [HttpGet("GetDetail")]
        public async Task<IActionResult> GetDetail(int stocktakeid)
        {
            try
            {
                //var storageid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "StorageId").Value);
                var result = await _context.StocktakeNotes
                    .Include(x => x.StocktakeNoteDetails).ThenInclude(x => x.Product).ThenInclude(x => x.MeasuredUnits).Include(x => x.UpdatedBy).Include(x => x.CreatedBy)
                    .SingleOrDefaultAsync(x => x.StocktakeId == stocktakeid && x.StorageId == 1);

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
        public async Task<IActionResult> GetDetail(string stocktakecode)
        {
            try
            {
                //var storageid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "StorageId").Value);
                var result = await _context.StocktakeNotes
                    .Include(x => x.StocktakeNoteDetails).ThenInclude(x => x.Product).ThenInclude(x => x.MeasuredUnits).Include(x => x.UpdatedBy).Include(x => x.CreatedBy)
                    .SingleOrDefaultAsync(x => x.StocktakeCode == stocktakecode && x.StorageId == 1);

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
