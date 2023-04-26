using AutoMapper;
using CapstoneProject_BE.AutoMapper;
using CapstoneProject_BE.DTO;
using CapstoneProject_BE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CapstoneProject_BE.Controllers.Product
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        public IMapper mapper;
        public IConfiguration _configuration;
        private readonly InventoryManagementContext _context;
        public CategoriesController(InventoryManagementContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
            this.mapper = config.CreateMapper();
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int catId)
        {
            try
            {
                var storageid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "StorageId").Value);
                var result = await _context.Categories.SingleOrDefaultAsync(x => x.CategoryId == catId && x.StorageId == storageid);
                if (result != null)
                {
                    _context.Remove(result);
                    await _context.SaveChangesAsync();
                    return Ok("Thành công");

                }
                else
                {
                    return NotFound("Loại sản phẩm không tồn tại");
                }
            }
            catch
            {
                return StatusCode(500);
            }
        }
        [HttpPost("PostCategory")]
        public async Task<IActionResult> PostCategory(CategoryDTO c)
        {
            try
            {
                var storageid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "StorageId").Value);
                if (c != null)
                {
                    var result = mapper.Map<Category>(c);
                    result.StorageId = storageid;
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
        [HttpPut("PutCategory")]
        public async Task<IActionResult> PutCategory(CategoryDTO c)
        {
            try
            {
                var storageid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "StorageId").Value);
                var editProduct = await _context.Categories.SingleOrDefaultAsync(x => x.CategoryId == c.CategoryId && x.StorageId == storageid);
                if (editProduct != null)
                {
                    editProduct.CategoryName = c.CategoryName;
                    editProduct.Description = c.Description;
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
        public async Task<IActionResult> Get(int offset, int limit, string? search = "")
        {
            try
            {
                var storageid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "StorageId").Value);
                var result = await _context.Categories
                    .Where(x =>x.CategoryName.Contains(search)
                &&x.StorageId==storageid).OrderByDescending(x=>x.CategoryId).ToListAsync();
                if (limit > result.Count() && offset >= 0)
                {
                    return Ok(new ResponseData<Category>
                    {
                        Data = result.Skip(offset).Take(result.Count()).ToList(),
                        Offset = offset,
                        Limit = limit,
                        Total = result.Count()
                    });
                }
                else if (offset >= 0)
                {
                    return Ok(new ResponseData<Category>
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
        public async Task<IActionResult> GetDetail(int catId)
        {
            try
            {
                var storageid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "StorageId").Value);
                var result = await _context.Categories.SingleOrDefaultAsync(x => x.CategoryId == catId && x.StorageId == storageid);
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
    }
}
