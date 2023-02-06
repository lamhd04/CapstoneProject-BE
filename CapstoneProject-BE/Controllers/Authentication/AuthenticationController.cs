using CapstoneProject_BE.DTO;
using CapstoneProject_BE.Helper;
using CapstoneProject_BE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CapstoneProject_BE.Controllers.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly InventoryManagementContext _context;
        public AuthenticationController(InventoryManagementContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == model.Email);
                if (user!=null&&HashHelper.Decrypt(user.Password,_configuration)==model.Password)
                {
                    return Ok(GenerateToken(user));
                }
                else
                {
                    return BadRequest("InvalidCredential");
                }
            }
            catch
            {
                return StatusCode(500);
            }

        }

        private string GenerateRefreshToken()
        {
            var random = new Byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);
                return Convert.ToBase64String(random);
            }
        }
        private TokenModel GenerateToken(User user)
        {
            var access = GenerateAccessToken(user);
            var refresh = GenerateRefreshToken();
            var tokenhandler = new JwtSecurityTokenHandler();
            var refreshEntity = new RefreshToken
            {
                UserId = user.UserId,
                Token = refresh,
                Created = DateTime.UtcNow,
                JwtId = tokenhandler.ReadJwtToken(access).Id,
                ExpiredAt = DateTime.UtcNow.AddMonths(1)
            };
            if (_context.RefreshTokens.SingleOrDefault(x => x.UserId == user.UserId) != null)
            {
                _context.Update(refreshEntity);
            }
            else
            {
                _context.Add(refreshEntity);
            }
            _context.SaveChanges();
            return new TokenModel(access, refresh);
        }
        private string GenerateAccessToken(User user)
        {
            var claims = new[]
{
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", user.UserId.ToString()),
                    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddSeconds(2),
                    signingCredentials: signIn
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        [HttpPost("RenewToken")]
        public async Task<IActionResult> RenewToken(TokenModel tokenmodel)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretkeyByte = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var tokenValidateParameter = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidIssuer = _configuration["Jwt:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                ClockSkew = TimeSpan.Zero,
                ValidateLifetime = false
            };
            try
            {
                //check format cua access token
                var tokenInVerification = jwtTokenHandler.ValidateToken(tokenmodel.AccessToken
                    , tokenValidateParameter, out var ValidatedToken);
                //check thuat toan ma hoa accesstoken
                if (ValidatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256);
                    if (!result)
                    {
                        return BadRequest("Invalid Token");
                    }
                }
                var refresh = await _context.RefreshTokens.SingleOrDefaultAsync(rf => rf.Token.Equals(tokenmodel.RefreshToken));
                //check refresh ton tai ?
                if (refresh == null)
                {
                    return BadRequest("Refresh token does not exist");
                }
                else if (refresh.IsRevoked)
                {
                    return BadRequest("Refresh token is revoked");
                }
                else if (refresh.JwtId != tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value)
                {
                    return BadRequest("Token does not match each other");
                }
                //check access het han chua
                if (ValidatedToken.ValidTo > DateTime.UtcNow)
                {
                    return BadRequest("current access token hasn't expired yet");
                }
                int uid = Int32.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type.Equals("UserId")).Value);
                var user = await _context.Users.SingleOrDefaultAsync(a => a.UserId == uid);
                return Ok(GenerateToken(user));
            }
            catch
            {
                return StatusCode(500);
            }
        }

    }
}
