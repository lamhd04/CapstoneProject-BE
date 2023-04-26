using AutoMapper;
using BitMiracle.LibTiff.Classic;
using CapstoneProject_BE.AutoMapper;
using CapstoneProject_BE.DTO;
using CapstoneProject_BE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CapstoneProject_BE.Controllers.Product
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
                var storageid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "StorageId").Value);
                var result = await _context.Suppliers.SingleOrDefaultAsync(x => x.SupplierId == supId&&x.StorageId== storageid);
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
                var storageid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "StorageId").Value);
                if (s != null)
                {
                    var result=mapper.Map<Supplier>(s);
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
        [HttpPut("PutSupplier")]
        public async Task<IActionResult> PutSupplier(SupplierDTO s)
        {
            try
            {
                var storageid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "StorageId").Value);
                var editSupplier = await _context.Suppliers.SingleOrDefaultAsync(x => x.SupplierId == s.SupplierId&&x.StorageId== storageid);
                if (editSupplier != null)
                {
                    editSupplier=mapper.Map<Supplier>(s);
                    editSupplier.StorageId = storageid;
                    _context.ChangeTracker.Clear();
                    _context.Update(editSupplier);
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
        public async Task<IActionResult> Get(int offset, int limit,bool? status=null, string? search = "")
        {
            try
            {
                var storageid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "StorageId").Value);
                var result = await _context.Suppliers
                    .Where(x => (x.SupplierName.Contains(search)||x.SupplierPhone.Contains(search))
                &&x.StorageId== storageid && (x.Status==status||status==null)).OrderByDescending(x=>x.SupplierId).ToListAsync();
                if (limit > result.Count() && offset >= 0)
                {
                    return Ok(new 
                    {
                        Data = mapper.Map<List<SupplierDTO>>(result.Skip(offset).Take(result.Count())),
                        Offset = offset,
                        Limit = limit,
                        Total = result.Count()
                    });
                }
                else if (offset >= 0)
                {
                    return Ok(new 
                    {
                        Data = mapper.Map<List<SupplierDTO>>(result.Skip(offset).Take(limit)),
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
                var storageid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "StorageId").Value);
                var supplier = await _context.Suppliers.SingleOrDefaultAsync(x=>x.SupplierId==supId&&x.StorageId== storageid);
                if (supplier != null)
                {
                    var result = mapper.Map<SupplierDTO>(supplier);                
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
