using AutoMapper;
using CapstoneProject_BE.DTO;
using CapstoneProject_BE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CapstoneProject_BE.Controllers.Product
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly InventoryManagementContext _context;
        public CategoriesController(InventoryManagementContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int catId)
        {
            try
            {
                var result = await _context.Categories.SingleOrDefaultAsync(x => x.CategoryId == catId);
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
        public async Task<IActionResult> PostCategory(Category c)
        {
            try
            {

                if (c != null)
                {
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
        [HttpPut("PutCategory")]
        public async Task<IActionResult> PutCategory(Category c)
        {
            try
            {
                var editProduct = await _context.Categories.SingleOrDefaultAsync(x => x.CategoryId == c.CategoryId);
                if (editProduct != null)
                {
                    editProduct.CategoryName = c.CategoryName;
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
                var result = await _context.Categories
                    .Where(x =>x.CategoryName.Contains(search)
                ).ToListAsync();
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
    }
}
