using AutoMapper;
using CapstoneProject_BE.AutoMapper;
using CapstoneProject_BE.DTO;
using CapstoneProject_BE.Helper;
using CapstoneProject_BE.Models;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Drawing;
using System.Text.Json;
using IronXL;

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
        public async Task<IActionResult> Get(int offset, int limit, int? catId = 0, int? supId = 0, string? search = "")
        {
            try
            {
                var result = await _context.Products.Include(x => x.Supplier).Include(x => x.Category)
                    .Where(x => (x.ProductCode.Contains(search) || x.ProductName.Contains(search) || x.Barcode.Contains(search))
                && (x.CategoryId == catId || catId == 0) && (x.SupplierId == supId || supId == 0)).ToListAsync();
                if (limit > result.Count() && offset >= 0)
                {
                    return Ok(new ResponseData<Models.Product>
                    {
                        Data = result.Skip(offset).Take(result.Count()).ToList(),
                        Offset = offset,
                        Limit = limit,
                        Total = result.Count()
                    });
                }
                else if (offset >= 0)
                {
                    return Ok(new ResponseData<Models.Product>
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
        public async Task<IActionResult> GetDetail(int prodId,string? barcode="")
        {
            try
            {
                var result = await _context.Products.Include(x => x.Supplier)
                    .Include(x => x.MeasuredUnits).Include(x => x.Category)
                    .SingleOrDefaultAsync(x => x.ProductId == prodId&&(x.Barcode==barcode||x.Barcode==""));
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
                    c.Created = DateTime.UtcNow;
                    c.ProductCode = GenerateProductCode(_context.Products.Count()+1);
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
        [HttpPut("PutProduct")]
        public async Task<IActionResult> PutProduct(ProductDTO productDTO)
        {
            try
            {
                var editProduct = await _context.Products.SingleOrDefaultAsync(x => x.ProductId == productDTO.ProductId);
                if (editProduct != null)
                {
                    _context.Entry(editProduct).State = EntityState.Detached;
                    var result=mapper.Map<Models.Product>(productDTO);
                    result.Created = editProduct.Created;
                    if (result.ProductCode == "")
                    {
                        result.ProductCode = GenerateProductCode(productDTO.ProductId);
                    }
                    _context.Update(result);
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

        [HttpPost("ImportExcelFile")]
        public async Task<IActionResult> ImportExcelFile(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                WorkBook workbook = WorkBook.Load(stream);
                WorkSheet worksheet = workbook.DefaultWorkSheet;

                int rowCount = worksheet.RowCount;
                int colCount = worksheet.ColumnCount;
                for (int row = 2; row <= rowCount; row++)
                {

                    ProductDTO p = new ProductDTO();
                    var c = mapper.Map<Models.Product>(p);
                    for (int col = 1; col <= colCount; col++)
                    {
                        c.ProductCode = worksheet["A" + row].ToString();
                        c.ProductName = worksheet["B" + row].ToString();
                        c.CategoryId = Convert.ToInt32(worksheet["C" + row].ToString());
                        c.Description = worksheet["D" + row].ToString();
                        c.SupplierId = Convert.ToInt32(worksheet["E" + row].ToString());
                        c.CostPrice = Convert.ToSingle(worksheet["F" + row].ToString());
                        c.SellingPrice = Convert.ToSingle(worksheet["G" + row].ToString());
                        c.DefaultMeasuredUnit = worksheet["H" + row].ToString();
                        c.InStock = Convert.ToInt32(worksheet["I" + row].ToString());
                        c.StockPrice = Convert.ToSingle(worksheet["J" + row].ToString());
                        c.Image = worksheet["K" + row].ToString();
                        c.Created = DateTime.UtcNow;
                        c.Status = Convert.ToBoolean(worksheet["L" + row].ToString());
                    }
                    _context.Add(c);
                    await _context.SaveChangesAsync();
                }
                return Ok("Thành công");
            }
        }
        private string GenerateProductCode(int proid)
        {
            var result = "SP" + proid;
            return result;
        }
    }
}
