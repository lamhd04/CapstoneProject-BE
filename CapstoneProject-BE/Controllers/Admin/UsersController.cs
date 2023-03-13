using AutoMapper;
using CapstoneProject_BE.AutoMapper;
using CapstoneProject_BE.DTO;
using CapstoneProject_BE.Helper;
using CapstoneProject_BE.Models;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Math;
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
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(int userid,string pwd)
        {
            try
            {
                //var storageid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "StorageId").Value);
                var result = await _context.Users.SingleOrDefaultAsync(x => x.UserId == userid);
                if (result != null && result.Status)
                {
                    result.Password = HashHelper.Encrypt(pwd, _configuration);
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
        public async Task<IActionResult> GetDetail(int userid)
        {
            try
            {
                //var storageid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "StorageId").Value);
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
                //var storageid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "StorageId").Value);
                var result = await _context.Users
                    .Where(x => (x.UserCode.Contains(search)||x.UserName.Contains(search)||x.Phone.Contains(search) || search == "")
                    && (x.Status == status || status == null)&&(x.RoleId==roleid||roleid==0)
                 ).OrderBy(x => x.UserName).ToListAsync();
                if (limit > result.Count() && offset >= 0)
                {
                    return Ok(new ResponseData<User>
                    {
                        Data = result.Skip(offset).Take(result.Count()).ToList(),
                        Offset = offset,
                        Limit = limit,
                        Total = result.Count()
                    });
                }
                else if (offset >= 0)
                {
                    return Ok(new ResponseData<User>
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
        [Authorize(Policy = "Owner")]
        [HttpPost("CreateStaff")]
        public async Task<IActionResult> CreateStaff(UserDTO u)
        {
            try
            {
                //var storageid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "StorageId").Value);
                var db = await _context.Users.SingleOrDefaultAsync(x => x.UserCode == u.UserCode&&x.StorageId==1);
                if (db != null)
                {
                    return BadRequest("Mã Nhân Viên Đã Tồn Tại");
                }
                var user = mapper.Map<User>(u);
                user.StorageId = 1;
                user.Email = null;
                user.Password =HashHelper.Encrypt("123456789aA@",_configuration);
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
        [HttpPut("UpdateStaff")]
        public async Task<IActionResult> UpdateStaff(UserDTO u)
        {
            try
            {
                //var storageid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "StorageId").Value);
                var db = await _context.Users.SingleOrDefaultAsync(x => x.UserCode == u.UserCode&&x.StorageId==1);
                if (db != null)
                {
                    var user = mapper.Map<User>(u);
                    user.StorageId = 1;
                    user.Email = null;
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                else
                {
                    return BadRequest("Không thể thay đổi mã NV");
                }

            }
            catch
            {
                return StatusCode(500);
            }

        }
    }
}
