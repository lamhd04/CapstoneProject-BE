using CapstoneProject_BE.DTO;
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
    }
}
