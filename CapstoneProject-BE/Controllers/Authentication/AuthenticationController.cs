﻿using CapstoneProject_BE.DTO;
using CapstoneProject_BE.Helper;
using CapstoneProject_BE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using MimeKit;

using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Security.Principal;
using System.Text.RegularExpressions;
using Org.BouncyCastle.Utilities;
using System.Net.Mail;
using CapstoneProject_BE.Constants;
using System;

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
                User user = new User();
                if (model.Email == null)
                {
                    user = await _context.Users.SingleOrDefaultAsync(u => u.UserCode == model.Usercode);
                }
                else
                {
                    user=await _context.Users.SingleOrDefaultAsync(u => u.Email == model.Email);
                }                   
                if (user!=null&&HashHelper.Decrypt(user.Password,_configuration)==model.Password&&user.Status)
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
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(string token,string newpwd)
        {
            try
            {
                
                var emailtoken = await _context.EmailTokens.SingleOrDefaultAsync(t => t.Token == token);
                if (emailtoken != null && !emailtoken.IsUsed && !emailtoken.IsRevoked&&Constant.validateGuidRegex.IsMatch(newpwd))
                {
                    var user = await _context.Users.SingleOrDefaultAsync(t => t.UserId == emailtoken.UserId);
                    user.Password = HashHelper.Encrypt(newpwd,_configuration);
                    emailtoken.IsUsed = true;
                    _context.Update(emailtoken);
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                else
                {
                    return BadRequest("Invalid Token");
                }
            }
            catch
            {
                return StatusCode(500);
            }


        }
        [HttpPost("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token)
        {
            try
            {
                
                var emailtoken = await _context.EmailTokens.SingleOrDefaultAsync(t => t.Token == token);
                if (emailtoken != null && !emailtoken.IsUsed && !emailtoken.IsRevoked)
                {
                    var user = await _context.Users.SingleOrDefaultAsync(t => t.UserId == emailtoken.UserId);
                    user.Status = true;
                    emailtoken.IsUsed = true;
                    _context.Update(emailtoken);
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                else
                {
                    return BadRequest("Invalid Token");
                }
            }
            catch
            {
                return StatusCode(500);
            }


        }

        [HttpPost("RevokeToken")]
        public async Task<IActionResult> RevokeToken(string token)
        {
            try
            {

                var emailtoken = await _context.EmailTokens.SingleOrDefaultAsync(t => t.Token == token);
                if (emailtoken != null)
                {                   
                    emailtoken.IsRevoked=true;
                    _context.Update(emailtoken);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                else
                {
                    return BadRequest("Invalid Token");
                }
            }
            catch
            {
                return StatusCode(500);
            }


        }
        [HttpPost("ResetPasswordByEmail")]
        public async Task<IActionResult> ResetPasswordByEmail(string email)
        {
            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
                if (user != null)
                {
                    var token = TokenHelper.GenerateRandomToken(64);
                    string url = Constant.ClientUrl+"/set-password?token=" + token;
                    string urlNotMe = Constant.ClientUrl + "/not-me?token=" + token;
                    MailMessage mm = new MailMessage("nguyendailam04@gmail.com", email);
                    mm.Subject = "Reset your password";
                    //mm.Body = "<a href='" + url + "'> Reset Password </a>" + "<br>" +
                    //    "<a href='" + urlNotMe + "'> Not you ? </a>" + "<br>" +
                    //    "This link will be expired in 1 day";
                    mm.Body = "<!DOCTYPE html>\r\n<html>\r\n\r\n<head>\r\n\r\n    <meta charset=\"utf-8\">\r\n    <meta http-equiv=\"x-ua-compatible\" content=\"ie=edge\">\r\n    <title>Email Confirmation</title>\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">\r\n    <style type=\"text/css\">\r\n        /**\r\n   * Google webfonts. Recommended to include the .woff version for cross-client compatibility.\r\n   */\r\n        @media screen {\r\n            @font-face {\r\n                font-family: 'Source Sans Pro';\r\n                font-style: normal;\r\n                font-weight: 400;\r\n                src: local('Source Sans Pro Regular'), local('SourceSansPro-Regular'), url(https://fonts.gstatic.com/s/sourcesanspro/v10/ODelI1aHBYDBqgeIAH2zlBM0YzuT7MdOe03otPbuUS0.woff) format('woff');\r\n            }\r\n\r\n            @font-face {\r\n                font-family: 'Source Sans Pro';\r\n                font-style: normal;\r\n                font-weight: 700;\r\n                src: local('Source Sans Pro Bold'), local('SourceSansPro-Bold'), url(https://fonts.gstatic.com/s/sourcesanspro/v10/toadOcfmlt9b38dHJxOBGFkQc6VGVFSmCnC_l7QZG60.woff) format('woff');\r\n            }\r\n        }\r\n\r\n        /**\r\n   * Avoid browser level font resizing.\r\n   * 1. Windows Mobile\r\n   * 2. iOS / OSX\r\n   */\r\n        body,\r\n        table,\r\n        td,\r\n        a {\r\n            -ms-text-size-adjust: 100%;\r\n            /* 1 */\r\n            -webkit-text-size-adjust: 100%;\r\n            /* 2 */\r\n        }\r\n\r\n        /**\r\n   * Remove extra space added to tables and cells in Outlook.\r\n   */\r\n        table,\r\n        td {\r\n            mso-table-rspace: 0pt;\r\n            mso-table-lspace: 0pt;\r\n        }\r\n\r\n        /**\r\n   * Better fluid images in Internet Explorer.\r\n   */\r\n        img {\r\n            -ms-interpolation-mode: bicubic;\r\n        }\r\n\r\n        /**\r\n   * Remove blue links for iOS devices.\r\n   */\r\n        a[x-apple-data-detectors] {\r\n            font-family: inherit !important;\r\n            font-size: inherit !important;\r\n            font-weight: inherit !important;\r\n            line-height: inherit !important;\r\n            color: inherit !important;\r\n            text-decoration: none !important;\r\n        }\r\n\r\n        /**\r\n   * Fix centering issues in Android 4.4.\r\n   */\r\n        div[style*=\"margin: 16px 0;\"] {\r\n            margin: 0 !important;\r\n        }\r\n\r\n        body {\r\n            width: 100% !important;\r\n            height: 100% !important;\r\n            padding: 0 !important;\r\n            margin: 0 !important;\r\n        }\r\n\r\n        /**\r\n   * Collapse table borders to avoid space between cells.\r\n   */\r\n        table {\r\n            border-collapse: collapse !important;\r\n        }\r\n\r\n        a {\r\n            color: #1a82e2;\r\n        }\r\n\r\n        img {\r\n            height: auto;\r\n            line-height: 100%;\r\n            text-decoration: none;\r\n            border: 0;\r\n            outline: none;\r\n        }\r\n    </style>\r\n\r\n</head>\r\n\r\n<body style=\"background-color: #e9ecef;\">\r\n\r\n    <!-- start preheader -->\r\n    <div class=\"preheader\"\r\n        style=\"display: none; max-width: 0; max-height: 0; overflow: hidden; font-size: 1px; line-height: 1px; color: #fff; opacity: 0;\">\r\n        A preheader is the short summary text that follows the subject line when an email is viewed in the inbox.\r\n    </div>\r\n    <!-- end preheader -->\r\n\r\n    <!-- start body -->\r\n    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n\r\n        <!-- start logo -->\r\n        <tr>\r\n            <td align=\"center\" bgcolor=\"#e9ecef\">\r\n                <!--[if (gte mso 9)|(IE)]>\r\n        <table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"600\">\r\n        <tr>\r\n        <td align=\"center\" valign=\"top\" width=\"600\">\r\n        <![endif]-->\r\n                <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">\r\n                    <tr>\r\n                        <td align=\"center\" valign=\"top\" style=\"padding: 36px 24px;\">\r\n                            <a href=\"https://www.blogdesire.com\" target=\"_blank\" style=\"display: inline-block;\">\r\n                                <img src=\"https://www.blogdesire.com/wp-content/uploads/2019/07/blogdesire-1.png\"\r\n                                    alt=\"Logo\" border=\"0\" width=\"48\"\r\n                                    style=\"display: block; width: 48px; max-width: 48px; min-width: 48px;\">\r\n                            </a>\r\n                        </td>\r\n                    </tr>\r\n                </table>\r\n                <!--[if (gte mso 9)|(IE)]>\r\n        </td>\r\n        </tr>\r\n        </table>\r\n        <![endif]-->\r\n            </td>\r\n        </tr>\r\n        <!-- end logo -->\r\n\r\n        <!-- start hero -->\r\n        <tr>\r\n            <td align=\"center\" bgcolor=\"#e9ecef\">\r\n                <!--[if (gte mso 9)|(IE)]>\r\n        <table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"600\">\r\n        <tr>\r\n        <td align=\"center\" valign=\"top\" width=\"600\">\r\n        <![endif]-->\r\n                <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">\r\n                    <tr>\r\n                        <td align=\"left\" bgcolor=\"#ffffff\"\r\n                            style=\"padding: 36px 24px 0; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; border-top: 3px solid #d4dadf;\">\r\n                            <h1\r\n                                style=\"margin: 0; font-size: 32px; font-weight: 700; letter-spacing: -1px; line-height: 48px;\">\r\n                                Confirm Your Email Address</h1>\r\n                        </td>\r\n                    </tr>\r\n                </table>\r\n                <!--[if (gte mso 9)|(IE)]>\r\n        </td>\r\n        </tr>\r\n        </table>\r\n        <![endif]-->\r\n            </td>\r\n        </tr>\r\n        <!-- end hero -->\r\n\r\n        <!-- start copy block -->\r\n        <tr>\r\n            <td align=\"center\" bgcolor=\"#e9ecef\">\r\n                <!--[if (gte mso 9)|(IE)]>\r\n        <table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"600\">\r\n        <tr>\r\n        <td align=\"center\" valign=\"top\" width=\"600\">\r\n        <![endif]-->\r\n                <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">\r\n\r\n                    <!-- start copy -->\r\n                    <tr>\r\n                        <td align=\"left\" bgcolor=\"#ffffff\"\r\n                            style=\"padding: 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px;\">\r\n                            <p style=\"margin: 0;\">Tap the button below to confirm your email address. If you didn't\r\n                                create an account with <a href=\"https://blogdesire.com\">Paste</a>, you can safely delete\r\n                                this email.</p>\r\n                        </td>\r\n                    </tr>\r\n                    <!-- end copy -->\r\n\r\n                    <!-- start button -->\r\n                    <tr>\r\n                        <td align=\"left\" bgcolor=\"#ffffff\">\r\n                            <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n                                <tr>\r\n                                    <td align=\"center\" bgcolor=\"#ffffff\" style=\"padding: 12px;\">\r\n                                        <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\">\r\n                                            <tr>\r\n                                                <td align=\"center\" bgcolor=\"#1a82e2\" style=\"border-radius: 6px;\">\r\n                                                    <a href=\"https://www.blogdesire.com\" target=\"_blank\"\r\n                                                        style=\"display: inline-block; padding: 16px 36px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; color: #ffffff; text-decoration: none; border-radius: 6px;\">Do\r\n                                                        Something Sweet</a>\r\n                                                </td>\r\n                                            </tr>\r\n                                        </table>\r\n                                    </td>\r\n                                </tr>\r\n                            </table>\r\n                        </td>\r\n                    </tr>\r\n                    <!-- end button -->\r\n\r\n                    <!-- start copy -->\r\n                    <tr>\r\n                        <td align=\"left\" bgcolor=\"#ffffff\"\r\n                            style=\"padding: 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px;\">\r\n                            <p style=\"margin: 0;\">If that doesn't work, copy and paste the following link in your\r\n                                browser:</p>\r\n                            <p style=\"margin: 0;\"><a href=\"https://blogdesire.com\"\r\n                                    target=\"_blank\">https://blogdesire.com/xxx-xxx-xxxx</a></p>\r\n                        </td>\r\n                    </tr>\r\n                    <!-- end copy -->\r\n\r\n                    <!-- start copy -->\r\n                    <tr>\r\n                        <td align=\"left\" bgcolor=\"#ffffff\"\r\n                            style=\"padding: 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px; border-bottom: 3px solid #d4dadf\">\r\n                            <p style=\"margin: 0;\">Cheers,<br> Paste</p>\r\n                        </td>\r\n                    </tr>\r\n                    <!-- end copy -->\r\n\r\n                </table>\r\n                <!--[if (gte mso 9)|(IE)]>\r\n        </td>\r\n        </tr>\r\n        </table>\r\n        <![endif]-->\r\n            </td>\r\n        </tr>\r\n        <!-- end copy block -->\r\n\r\n        <!-- start footer -->\r\n        <tr>\r\n            <td align=\"center\" bgcolor=\"#e9ecef\" style=\"padding: 24px;\">\r\n                <!--[if (gte mso 9)|(IE)]>\r\n        <table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"600\">\r\n        <tr>\r\n        <td align=\"center\" valign=\"top\" width=\"600\">\r\n        <![endif]-->\r\n                <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">\r\n\r\n                    <!-- start permission -->\r\n                    <tr>\r\n                        <td align=\"center\" bgcolor=\"#e9ecef\"\r\n                            style=\"padding: 12px 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 14px; line-height: 20px; color: #666;\">\r\n                            <p style=\"margin: 0;\">You received this email because we received a request for\r\n                                [type_of_action] for your account. If you didn't request [type_of_action] you can safely\r\n                                delete this email.</p>\r\n                        </td>\r\n                    </tr>\r\n                    <!-- end permission -->\r\n\r\n                    <!-- start unsubscribe -->\r\n                    <tr>\r\n                        <td align=\"center\" bgcolor=\"#e9ecef\"\r\n                            style=\"padding: 12px 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 14px; line-height: 20px; color: #666;\">\r\n                            <p style=\"margin: 0;\">To stop receiving these emails, you can <a\r\n                                    href=\"https://www.blogdesire.com\" target=\"_blank\">unsubscribe</a> at any time.</p>\r\n                            <p style=\"margin: 0;\">Paste 1234 S. Broadway St. City, State 12345</p>\r\n                        </td>\r\n                    </tr>\r\n                    <!-- end unsubscribe -->\r\n\r\n                </table>\r\n                <!--[if (gte mso 9)|(IE)]>\r\n        </td>\r\n        </tr>\r\n        </table>\r\n        <![endif]-->\r\n            </td>\r\n        </tr>\r\n        <!-- end footer -->\r\n\r\n    </table>\r\n    <!-- end body -->\r\n\r\n</body>\r\n\r\n</html>";
                    mm.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential();
                    NetworkCred.UserName = "nguyendailam04@gmail.com";
                    NetworkCred.Password = "tbsnoshajsenptbd";
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(mm);
                    var emailtoken = new EmailToken {
                        Token = token,
                        IssuedAt = DateTime.UtcNow,
                        ExpiredAt = DateTime.UtcNow.AddDays(1),
                        IsRevoked=false,
                        IsUsed=false,
                        UserId=user.UserId
                    };
                    _context.Add(emailtoken);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                else
                {
                    return BadRequest("Invalid Email");
                }
            }
            catch
            {
                return StatusCode(500);
            }

        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(SignUpModel model)
        {
            try
            {

                var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == model.Email);
                if (user == null && Constant.validateGuidRegex.IsMatch(model.Password))
                {
                    var token = TokenHelper.GenerateRandomToken(64);
                    MailMessage mm = new MailMessage("nguyendailam04@gmail.com", model.Email);
                    string url = Constant.ClientUrl + "/verification-success?token=" + token;
                    string urlNotMe = Constant.ClientUrl + "/not-me?token=" + token;
                    mm.Subject = "Confirm your email";
                    mm.Body = "<a href='" + url + "'> Confirm email </a>" + "<br>" +
                        "<a href='" + urlNotMe + "'> Not you ? </a>" + "<br>" +
                        "This link will be expired in 1 day";
                    mm.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential();
                    NetworkCred.UserName = "nguyendailam04@gmail.com";
                    NetworkCred.Password = "tbsnoshajsenptbd";
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(mm);
                    var newstorage = new Storage
                    {
                        StorageName = "KHO" + _context.Storages.Count() + 1
                    };
                    _context.Add(newstorage);
                    await _context.SaveChangesAsync();
                    var newuser = new User
                    {
                        StorageId = newstorage.StorageId,
                        Email = model.Email,
                        Password = HashHelper.Encrypt(model.Password, _configuration),
                        RoleId = 1,
                        UserName = TokenHelper.GenerateRandomToken(8)
                    };
                    _context.Add(newuser);
                    await _context.SaveChangesAsync();
                    var emailtoken = new EmailToken
                    {
                        Token = token,
                        IssuedAt = DateTime.UtcNow,
                        ExpiredAt = DateTime.UtcNow.AddDays(1),
                        IsRevoked = false,
                        IsUsed = false,
                        UserId = newuser.UserId
                    };
                    _context.Add(emailtoken);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                else
                {
                    return BadRequest("This Email is already existed");
                }
            }
            catch
            {
                return StatusCode(500);
            }
        }

        




        private TokenModel GenerateToken(User user)
        {
            var access = GenerateAccessToken(user);
            var refresh = TokenHelper.GenerateRandomToken();
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
                        new Claim("RoleId", user.RoleId.ToString()),
                        new Claim("StorageId", user.StorageId.ToString()),
                    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddHours(1),
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
                ValidateLifetime = true
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
