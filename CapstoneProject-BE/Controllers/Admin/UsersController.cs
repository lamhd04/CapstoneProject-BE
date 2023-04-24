using AutoMapper;
using CapstoneProject_BE.AutoMapper;
using CapstoneProject_BE.Constants;
using CapstoneProject_BE.DTO;
using CapstoneProject_BE.Helper;
using CapstoneProject_BE.Models;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CapstoneProject_BE.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly InventoryManagementContext _context;
        public IMapper mapper;
        public UsersController(InventoryManagementContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
            mapper = config.CreateMapper();
        }
        [Authorize(Policy = "Owner")]
        [HttpPost("Deactivate")]
        public async Task<IActionResult> Deactivate(int userid)
        {
            try
            {
                //var storageid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "StorageId").Value);
                var result = await _context.Users.SingleOrDefaultAsync(x => x.UserId == userid);
                if (result != null && result.Status)
                {
                    result.Status = false;
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
        [Authorize(Policy = "Owner")]
        [HttpPost("Activate")]
        public async Task<IActionResult> Activate(int userid)
        {
            try
            {
                //var storageid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "StorageId").Value);
                var result = await _context.Users.SingleOrDefaultAsync(x => x.UserId == userid);
                if (result != null && !result.Status)
                {
                    result.Status = true;
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
        [Authorize]
        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword(PasswordDTO p)
        {
            try
            {
                var storageid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "StorageId").Value);
                User result = null;
                if (p.UserId == 0)
                {
                    var userid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "UserId").Value);
                    result = await _context.Users.SingleOrDefaultAsync(x => x.UserId == userid);
                    if (p.OldPassword != HashHelper.Decrypt(result.Password, _configuration))
                    {
                        return BadRequest("Mật khẩu cũ không đúng");
                    }
                }
                else
                {
                    result = await _context.Users.SingleOrDefaultAsync(x => x.UserId == p.UserId);
                }
                if (result != null && result.Status &&Constant.validateGuidRegex.IsMatch(result.Password))
                {
                    result.Password = HashHelper.Encrypt(p.Password, _configuration);
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
        [Authorize]
        [HttpGet("GetUserDetail")]
        public async Task<IActionResult> GetDetail(int? userid=0)
        {
            try
            {
                //var storageid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "StorageId").Value);
                if (userid == 0)
                    userid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "UserId").Value);
                var result = await _context.Users.SingleOrDefaultAsync(x => x.UserId == userid);
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
        [Authorize]
        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers(int offset, int limit, int? roleid = 0, bool? status = null, string? search = "")
        {
            try
            {
                var storageid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "StorageId").Value);
                var result = await _context.Users.Include(x=>x.Role)
                    .Where(x => (x.UserCode.Contains(search)||x.UserName.Contains(search)||x.Phone.Contains(search) || search == "")
                    && (x.Status == status || status == null)&&(x.RoleId==roleid||roleid==0)&&x.StorageId==storageid
                 ).OrderBy(x => x.UserName).ToListAsync();
                if (limit > result.Count() && offset >= 0)
                {
                    return Ok(new
                    {
                        Data = (from u in result
                                select new
                                {
                                    UserId=u.UserId,
                                    UserCode =u.UserCode,
                                    Image=u.Image,
                                    UserName=u.UserName,
                                    Phone=u.Phone,
                                    RoleName=u.Role.RoleName,
                                    Status=u.Status
                                }).Skip(offset).Take(result.Count()).ToList(),
                        Offset = offset,
                        Limit = limit,
                        Total = result.Count()
                    });
                }
                else if (offset >= 0)
                {
                    return Ok(new
                    {
                        Data = (from u in result
                                select new
                                {
                                    UserCode = u.UserCode,
                                    Image = u.Image,
                                    UserName = u.UserName,
                                    Phone = u.Phone,
                                    RoleName = u.Role.RoleName,
                                    Status = u.Status
                                }).Skip(offset).Take(limit).ToList(),
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
        [Authorize(Policy = "Owner")]
        [HttpPost("CreateStaff")]
        public async Task<IActionResult> CreateStaff(UserDTO u)
        {
            try
            {
                var storageid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "StorageId").Value);
                var db = await _context.Users.SingleOrDefaultAsync(x => x.UserCode == u.UserCode);
                if (db != null)
                {
                    return BadRequest("Mã Nhân Viên Đã Tồn Tại");
                }
                var user = mapper.Map<User>(u);
                user.StorageId = storageid;
                user.Email = null;
                if (u.Password==null)
                {
                    user.Password = HashHelper.Encrypt("123456aA@", _configuration);
                }
                else if(Constant.validateGuidRegex.IsMatch(user.Password))
                {
                    user.Password = HashHelper.Encrypt(user.Password, _configuration);
                }
                else
                {
                    return BadRequest("Mật khẩu phải từ 6 đến 32 kí tự có chứa" +
                        "\n Chữ số" +
                        "\n Chữ cái" +
                        "\n Chữ cái hoa" +
                        "\n Kí tự đặc biệt");
                }
                user.Status = true;
                _context.Add(user);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch
            {
                return StatusCode(500);
            }

        }
        [Authorize]
        [HttpPost("UploadImage")]
        public async Task<IActionResult> UploadImage(IFormFile image)
        {
            try
            {
                var extensions = new List<string> { ".jpg", ".png",".jpeg",".gif",".svg" };
                if (image.Length > 5000000)
                    return BadRequest("Kích thước ảnh không được quá 5MB");
                if (!extensions.Contains<string>(System.IO.Path.GetExtension(image.FileName)))
                    return BadRequest("Tệp phải là ảnh có 1 trong những định dang jpg, png, jpeg, gif, svg");
                ImageHelper imagekit = new ImageHelper(_configuration["Imagekit:PublicKey"], _configuration["Imagekit:PrivateKey"], _configuration["Imagekit:UrlEndPoint"]);
               var url= await imagekit.UploadImageAsync(image, image.FileName);
                if (url == null)
                {
                    return BadRequest("Tải ảnh không thành công");
                }
                else
                { 
                    return Ok(url);
                }
            }
            catch
            {
                return StatusCode(500);
            }
        }
        [Authorize(Policy = "Owner")]
        [HttpPut("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile(UserDTO u)
        {
            try
            {
                var storageid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "StorageId").Value);
                var userid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "UserId").Value);
                var db = await _context.Users.SingleOrDefaultAsync(x => x.UserId == u.UserId && x.StorageId == storageid);
                if (db != null)
                {
                    var user = mapper.Map<User>(u);
                    _context.ChangeTracker.Clear();
                    user.Password = db.Password;
                    user.Status = true;
                    user.StorageId = storageid;
                    if (user.Email != db.Email)
                    {
                        return BadRequest("Khong the thay doi email");
                    }
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                    return Ok("Thanh cong");
                }
                else
                {
                    return BadRequest("Người dùng ko tồn tại");
                }
            }
            catch
            {
                return StatusCode(500);
            }

        }
        [Authorize]
        [HttpPut("UpdateStaff")]
        public async Task<IActionResult> UpdateStaff(UserDTO u)
        {
            try
            {
                var storageid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "StorageId").Value);
                var userid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "UserId").Value);
                var roleid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "RoleId").Value);
                var db = await _context.Users.SingleOrDefaultAsync(x => x.UserCode == u.UserCode && x.StorageId == storageid);
                if (roleid != 1 && userid != db.UserId)
                {
                    return BadRequest("Không thể update nhân viên này");
                }
                if (db != null)
                {
                    var user = mapper.Map<User>(u);
                    _context.ChangeTracker.Clear();
                    user.Password = db.Password;
                    user.Status = db.Status;
                    user.StorageId = storageid;
                    if (user.RoleId == 1)
                    {
                        return BadRequest("Không thể cập nhật");
                    }
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                    return Ok("Thanh cong");
                }
                else
                {
                    return BadRequest("Người dùng ko tồn tại");
                }
            }
            catch
            {
                return StatusCode(500);
            }

        }
    }
}
