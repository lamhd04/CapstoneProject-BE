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
using BitMiracle.LibTiff.Classic;
using System.IdentityModel.Tokens.Jwt;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using CapstoneProject_BE.Migrations;

namespace CapstoneProject_BE.Controllers.Product
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        public async Task<IActionResult> Get(int offset, int limit,bool? status=null, int? catId = 0, int? supId = 0, string? search = "")
        {
            try
            {
                var storageid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "StorageId").Value);
                var result = await _context.Products.Include(x => x.Supplier).Include(x => x.Category).Include(x=>x.MeasuredUnits)
                    .Where(x => (x.ProductCode.Contains(search) || x.ProductName.Contains(search) || x.Barcode.Contains(search))
                && (x.CategoryId == catId || catId == 0) && (x.Status == status || status == null) && (x.SupplierId == supId || supId == 0)&&x.StorageId== storageid)
                    .OrderByDescending(x=>x.Created).ToListAsync();
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
        public async Task<IActionResult> GetDetail(int prodId,int offset, int limit, string? barcode="")
        {
            try
            {
                var storageid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "StorageId").Value);
                var result = await _context.Products.Include(x => x.Supplier)
                    .Include(x => x.MeasuredUnits).Include(x => x.Category)
                    .SingleOrDefaultAsync(x => x.ProductId == prodId&&(x.Barcode==barcode||barcode=="")&&x.StorageId== storageid);
                var history = await _context.ProductHistories.Where(x => x.ProductId == result.ProductId).Include(x => x.User).Include(x => x.ActionType).OrderByDescending(x=>x.Date).ToListAsync();
                if (result != null)
                {
                    result.ProductHistories = history;
                    return Ok(result);
                    if (limit > history.Count() && offset >= 0)
                    {
                        history = history.Skip(offset).Take(history.Count()).ToList();
                    }
                    else if (offset >= 0)
                    {
                        history = history.Skip(offset).Take(limit).ToList();
                    }

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
                var storageid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "StorageId").Value);
                var uid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type.Equals("UserId")).Value);
                if (p != null)
                {
                    var c = mapper.Map<Models.Product>(p);
                    c.Created = DateTime.UtcNow;
                    if (c.ProductCode == null)
                    {
                        c.ProductCode = GenerateProductCode(_context.Products.Count() + 1);
                    }
                    else if (_context.Products.SingleOrDefault(x => x.ProductCode == c.ProductCode) != null)
                    {
                        return BadRequest("Mã sản phẩm đã tồn tại");
                    }                  
                    c.StorageId = storageid;
                    if (c.Barcode == null)
                    {
                        c.Barcode = c.ProductCode;
                    }
                    foreach(var a in c.MeasuredUnits)
                    {
                        a.SuggestedPrice = (a.MeasuredUnitValue * c.SellingPrice);
                    }
                    _context.Add(c);
                    await _context.SaveChangesAsync();
                    if (c.InStock != 0)
                    {
                        var history = new ProductHistory
                        {
                            UserId = uid,
                            ActionId = 4,
                            ProductId = c.ProductId,
                            CostPrice = c.CostPrice,
                            Price = c.SellingPrice,
                            Amount = c.InStock,
                            AmountDifferential = $"+{c.InStock}",
                            Date = DateTime.Now
                            
                        };
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
        [HttpPut("PutProduct")]
        public async Task<IActionResult> PutProduct(ProductDTO productDTO)
        {
            try
            {
                var storageid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "StorageId").Value);
                var uid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type.Equals("UserId")).Value);
                var editProduct = await _context.Products.SingleOrDefaultAsync(x => x.ProductId == productDTO.ProductId&&x.StorageId== storageid);
                if (editProduct != null)
                {
                    _context.Entry(editProduct).State = EntityState.Detached;
                    var result=mapper.Map<Models.Product>(productDTO);
                    result.Created = editProduct.Created;
                    result.StorageId = storageid;
                    if (result.ProductCode == null)
                    {
                        result.ProductCode = GenerateProductCode(productDTO.ProductId);
                    }else if(_context.Products.SingleOrDefault(x => x.ProductCode == result.ProductCode&& x.ProductId!=editProduct.ProductId) != null)
                    {
                        return BadRequest("Mã sản phẩm đã tồn tại");
                    }
                    if (result.Barcode == "")
                    {
                        result.Barcode = result.ProductCode;
                    }
                    var costdifferential = editProduct.CostPrice - result.CostPrice;
                    var pricedifferential = editProduct.SellingPrice - result.SellingPrice;
                    if (costdifferential != 0 || pricedifferential != 0)
                    {
                        var history = new ProductHistory
                        {
                            UserId = uid,
                            ActionId = 3,
                            ProductId = editProduct.ProductId,
                            CostPrice = editProduct.CostPrice,
                            CostPriceDifferential = costdifferential > 0 ? $"-{costdifferential}" : $"+{costdifferential}",
                            Price = result.SellingPrice,
                            PriceDifferential = pricedifferential > 0 ? $"-{pricedifferential}" : $"+{pricedifferential}",
                            Date = DateTime.Now
                        };
                        _context.Add(history);
                    }                   
                    foreach (var a in result.MeasuredUnits)
                    {
                        a.SuggestedPrice = (a.MeasuredUnitValue * result.SellingPrice);
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
