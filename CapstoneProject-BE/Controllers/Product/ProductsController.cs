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
                var result = _context.Products.Include(x => x.Supplier).Include(x => x.Category)
                    .Where(x => (x.ProductCode.Contains(search) || x.ProductName.Contains(search))
                && (x.CategoryId == catId || catId == 0) && (x.SupplierId == supId || supId == 0));
                if (limit > result.Count() && offset >= 0)
                {
                    return Ok(new ProductList
                    {
                        Data = result.Skip(offset).Take(result.Count()).ToList(),
                        Offset = offset,
                        Limit = limit,
                        Total = result.Count()
                    });
                }
                else if (offset >= 0)
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
                var result = await _context.Products.Include(x => x.Supplier).Include(x => x.MeasuredUnits).Include(x => x.Category).SingleOrDefaultAsync(x => x.ProductId == prodId);
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
                    c.ProductCode = GenerateProductCode();
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
        public async Task<IActionResult> PutProduct(int id, Models.Product p)
        {
            try
            {
                var editProduct = await _context.Products.SingleOrDefaultAsync(x => x.ProductId == id);
                if (p != null)
                {
                    editProduct.ProductName = p.ProductName;
                    editProduct.ProductCode = p.ProductCode;
                    editProduct.CategoryId = p.CategoryId;
                    editProduct.Description = p.Description;
                    editProduct.SupplierId = p.SupplierId;
                    editProduct.CostPrice = p.CostPrice;
                    editProduct.SellingPrice = p.SellingPrice;
                    editProduct.DefaultMeasuredUnit = p.DefaultMeasuredUnit;
                    editProduct.InStock = p.InStock;
                    editProduct.StockPrice = p.StockPrice;
                    editProduct.Image = p.Image;
                    editProduct.Created = p.Created;
                    editProduct.Status = p.Status;
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

        [HttpGet("Export")]
        public async Task<FileResult> ExportExcel(List<Models.Product> list)
        {
            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("Product");
                worksheet.Cell(1, 1).Value = "ProductID";
                worksheet.Cell(1, 2).Value = "ProductName";
                worksheet.Cell(1, 3).Value = "ProductCode";
                worksheet.Cell(1, 4).Value = "Category";
                worksheet.Cell(1, 5).Value = "Description";
                worksheet.Cell(1, 6).Value = "Supplier";
                worksheet.Cell(1, 7).Value = "CostPrice";
                worksheet.Cell(1, 8).Value = "SellingPrice";
                worksheet.Cell(1, 9).Value = "DefaultMeasuredUnit";
                worksheet.Cell(1, 10).Value = "InStock";
                worksheet.Cell(1, 11).Value = "StockPrice";
                worksheet.Cell(1, 12).Value = "Image";
                worksheet.Cell(1, 13).Value = "Created";
                worksheet.Cell(1, 14).Value = "Status";

                IXLRange range = worksheet.Range(worksheet.Cell(1, 1).Address, worksheet.Cell(1, 14).Address);
                range.Style.Fill.SetBackgroundColor(XLColor.Almond);

                int index = 1;

                foreach (var item in list)
                {
                    index++;
                    worksheet.Cell(index, 1).Value = item.ProductId;
                    worksheet.Cell(index, 2).Value = item.ProductName;
                    worksheet.Cell(index, 3).Value = item.ProductCode;
                    worksheet.Cell(index, 4).Value = item.Category.CategoryName;
                    worksheet.Cell(index, 5).Value = item.Description;
                    worksheet.Cell(index, 6).Value = item.Supplier.SupplierName;
                    worksheet.Cell(index, 7).Value = item.CostPrice;
                    worksheet.Cell(index, 8).Value = item.SellingPrice;
                    worksheet.Cell(index, 9).Value = item.DefaultMeasuredUnit;
                    worksheet.Cell(index, 10).Value = item.InStock;
                    worksheet.Cell(index, 11).Value = item.StockPrice;
                    worksheet.Cell(index, 12).Value = item.Image;
                    worksheet.Cell(index, 13).Value = item.Created;
                    worksheet.Cell(index, 14).Value = item.Status;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    var strDate = DateTime.Now.ToString("yyyyMMdd");
                    string fileName = string.Format($"Products_{strDate}.xlsx");
                    return File(content, contentType, fileName);
                }
            }
        }
        private string GenerateProductCode()
        {
            var result = "SP" + _context.Products.Count();
            return result;
        }
    }
}
