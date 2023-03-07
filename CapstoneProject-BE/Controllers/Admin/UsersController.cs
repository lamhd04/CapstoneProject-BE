using AutoMapper;
using CapstoneProject_BE.AutoMapper;
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
        public IMapper mapper;
        public UsersController(InventoryManagementContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
            mapper = config.CreateMapper();
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
        public async Task<IActionResult> CreateStaff(UserDTO u)
        {
            try
            {
                //var storageid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "StorageId").Value);
                var user = mapper.Map<User>(u);
                user.StorageId = 1;
                user.Password = "123456789aA@";
                _context.Add(user);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch
            {
                return StatusCode(500);
            }

        }
        [HttpPut("UpdateStaff")]
        public async Task<IActionResult> UpdateStaff(UserDTO u)
        {
            try
            {
                //var storageid = Int32.Parse(User.Claims.SingleOrDefault(x => x.Type == "StorageId").Value);
                var user = mapper.Map<User>(u);
                user.StorageId = 1;
                _context.Update(user);
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
