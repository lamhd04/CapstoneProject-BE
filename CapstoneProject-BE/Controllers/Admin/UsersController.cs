using CapstoneProject_BE.DTO;
using CapstoneProject_BE.Helper;
using CapstoneProject_BE.Models;
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
        public UsersController(InventoryManagementContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                return Ok(await _context.Users.ToListAsync());
            }
            catch
            {
                return StatusCode(500);
            }

        }
        [HttpPost("CreateStaff")]
        public async Task<IActionResult> CreateStaff()
        {
            try
            {
                var storageid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "StorageId").Value);
                var user = new User
                {
                    StorageId = storageid,
                    UserCode = storageid + TokenHelper.GenerateRandomToken(4),
                    Password = "Nhanvien" + storageid,
                    Status = true,
                    RoleId=3
                };
                _context.Add(user);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch
            {
                return StatusCode(500);
            }

        }
    }
}
