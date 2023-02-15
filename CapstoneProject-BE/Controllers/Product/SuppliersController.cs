using AutoMapper;
using BitMiracle.LibTiff.Classic;
using CapstoneProject_BE.AutoMapper;
using CapstoneProject_BE.DTO;
using CapstoneProject_BE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CapstoneProject_BE.Controllers.Product
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly InventoryManagementContext _context;
        public IMapper mapper;
        public SuppliersController(InventoryManagementContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
            this.mapper = config.CreateMapper();
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
        public async Task<IActionResult> PostSupplier(SupplierDTO s)
        {
            try
            {
                if (s != null)
                {
                    var result=mapper.Map<Supplier>(s);
                    result.City = JsonSerializer.Serialize(s.City);
                    result.District = JsonSerializer.Serialize(s.District);
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
        [HttpPut("PutSupplier")]
        public async Task<IActionResult> PutSupplier(SupplierDTO s)
        {
            try
            {
                var editSupplier = await _context.Suppliers.SingleOrDefaultAsync(x => x.SupplierId == s.SupplierId);
                if (editSupplier != null)
                {
                    _context.Entry(editSupplier).State = EntityState.Detached;
                    editSupplier=mapper.Map<Supplier>(s);
                    editSupplier.City = JsonSerializer.Serialize(s.City);
                    editSupplier.District = JsonSerializer.Serialize(s.District);
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
                var supplier = await _context.Suppliers.SingleOrDefaultAsync(x => x.SupplierId == supId);
                if (supplier != null)
                {
                    var result = mapper.Map<SupplierDTO>(supplier);
                    result.City = JsonSerializer.Deserialize<City>(supplier.City);
                    result.District= JsonSerializer.Deserialize<District>(supplier.District);
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
