using AutoMapper;
using CapstoneProject_BE.AutoMapper;
using CapstoneProject_BE.DTO;
using CapstoneProject_BE.Helper;
using CapstoneProject_BE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CapstoneProject_BE.Controllers.Product
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly InventoryManagementContext _context;
        public IMapper mapper;
        public ProductsController(InventoryManagementContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
            this.mapper = config.CreateMapper();
        }
        [HttpGet("Get")]
        public async Task<IActionResult> Get(int offset, int limit,int? catId=0,int? supId=0, string? search = "")
        {
            try
            {
                var result = _context.Products.Include(x=>x.Supplier).Include(x=>x.Category)
                    .Where(x => x.ProductCode.Contains(search)|| x.ProductName.Contains(search)
                && x.CategoryId == catId || catId==0 && x.SupplierId == supId || supId==0);
                if (limit > result.Count()&&offset>=0)
                {
                   return Ok(new ProductList { Data= result.Skip(offset).Take(result.Count()).ToList(),
                   Offset=offset,
                   Limit=limit,
                   Total=result.Count()});
                }
                else if(offset >= 0)
                {
                    return Ok(new ProductList
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
        [HttpGet("GetDetail")]
        public async Task<IActionResult> GetDetail(int prodId)
        {
            try
            {
                var result = await _context.Products.Include(x => x.Supplier).Include(x=>x.MeasuredUnits).Include(x => x.Category).SingleOrDefaultAsync(x=>x.ProductId==prodId);
                if (result != null)
                {
                    return Ok(result);

                }
                else
                {
                    return NotFound("sản phẩm không tồn tại");
                }
            }
            catch
            {
                return StatusCode(500);
            }

        }
        [HttpPost("PostProduct")]
        public async Task<IActionResult> PostProduct(ProductDTO p)
        {
            try
            {
                
                if (p != null)
                {
                    var c = mapper.Map<Models.Product>(p);
                    c.Created=DateTime.UtcNow;
                    _context.Add(c);
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
