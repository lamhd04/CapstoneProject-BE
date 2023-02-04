using CapstoneProject_BE.DTO;
using CapstoneProject_BE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using MimeKit;
using MailKit.Security;
using MailKit.Net.Smtp;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Security.Principal;

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
                var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == model.Username && u.Password == model.Password);
                if (user != null)
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

        [HttpGet("Forgot")]
        public async Task<IActionResult> ForgotPassword(TokenModel token)
        {
            try
            {
                var user = await _context.ForgotPasswordModels.SingleOrDefaultAsync(u => u.Token == token.AccessToken);
                if (user != null)
                {
                    return Ok();
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

        [HttpGet("SendPasswordResetLink/{email}")]
        public async Task<IActionResult> SendPasswordResetLink(string email)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == email);
            if (user == null)
            {
                return StatusCode(500);
            }
            string token = GenerateAccessToken(user);
            ForgotPasswordModel forgotPasswordModel = new ForgotPasswordModel();
            forgotPasswordModel.Email = email;
            forgotPasswordModel.Token = token;
            string resetLink = "https://localhost:7265/reset-password?token=" + token;
            var emailToSend = new MimeMessage();
            emailToSend.From.Add(new MailboxAddress("Sender name", "kiennthe153296@fpt.edu.vn"));
            emailToSend.To.Add(new MailboxAddress("Receiver name", email));
            emailToSend.Subject = "Password Reset Request";
            emailToSend.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = $"Dear {user.UserName},\n\n" +
                               $"We received a request to reset the password for your account. If you made this request, please follow the link below to reset your password:\n\n" +
                               $"{resetLink}\n\n" +
                               "If you did not make this request, you can safely ignore this email. Your password will not be reset without clicking on the link above.\n\n" +
                               "Thank you,\n" +
                               "IMSD System."
            };
            using (var emailClient = new SmtpClient())
            {
                emailClient.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                emailClient.Authenticate("kiennthe153296@fpt.edu.vn", "02112001Kien");
                emailClient.Send(emailToSend);
                emailClient.Disconnect(true);
            }
            forgotPasswordModel.EmailSent = true;
            await _context.SaveChangesAsync();
            return Ok(forgotPasswordModel);
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
            if (_context.RefreshTokens.SingleOrDefault(x=>x.UserId==user.UserId)!=null)
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
                ValidateLifetime=false
            };
            try
            {
                //check format cua access token
                var tokenInVerification = jwtTokenHandler.ValidateToken(tokenmodel.AccessToken
                    , tokenValidateParameter, out var ValidatedToken);
                //check thuat toan ma hoa accesstoken
                if(ValidatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256);
                    if (!result)
                    {
                        return BadRequest("Invalid Token");
                    }
                }
                var refresh = await _context.RefreshTokens.SingleOrDefaultAsync(rf => rf.Token.Equals(tokenmodel.RefreshToken));
                //check refresh ton tai ?
                if ( refresh== null)
                {
                    return BadRequest("Refresh token does not exist");
                }
                else if(refresh.IsRevoked)
                {
                    return BadRequest("Refresh token is revoked");
                }else if (refresh.JwtId != tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value)
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
