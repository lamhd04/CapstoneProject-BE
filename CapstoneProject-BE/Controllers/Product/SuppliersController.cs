using CapstoneProject_BE.DTO;
using CapstoneProject_BE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CapstoneProject_BE.Controllers.Product
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly InventoryManagementContext _context;
        public SuppliersController(InventoryManagementContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int supId)
        {
            try
            {
                var result = await _context.Suppliers.SingleOrDefaultAsync(x => x.SupplierId == supId);
                if (result != null)
                {
                    _context.Remove(result);
                    await _context.SaveChangesAsync();
                    return Ok("Thành công");

                }
                else
                {
                    return NotFound("Nhà cung cấp không tồn tại");
                }
            }
            catch
            {
                return StatusCode(500);
            }
        }
        [HttpPost("PostSupplier")]
        public async Task<IActionResult> PostSupplier(Supplier s)
        {
            try
            {

                if (s != null)
                {
                    _context.Add(s);
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
        [HttpPut("PutSupplier")]
        public async Task<IActionResult> PutSupplier(Supplier s)
        {
            try
            {
                var editProduct = await _context.Suppliers.SingleOrDefaultAsync(x => x.SupplierId == s.SupplierId);
                if (editProduct != null)
                {
                    _context.Entry(editProduct).State = EntityState.Detached;
                    _context.Update(s);
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
                var result = await _context.Suppliers
                    .Where(x => x.SupplierName.Contains(search)
                ).ToListAsync();
                if (limit > result.Count() && offset >= 0)
                {
                    return Ok(new ResponseData<Supplier>
                    {
                        Data = result.Skip(offset).Take(result.Count()).ToList(),
                        Offset = offset,
                        Limit = limit,
                        Total = result.Count()
                    });
                }
                else if (offset >= 0)
                {
                    return Ok(new ResponseData<Supplier>
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
        public async Task<IActionResult> GetDetail(int supId)
        {
            try
            {
                var result = await _context.Suppliers.SingleOrDefaultAsync(x => x.SupplierId == supId);
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
