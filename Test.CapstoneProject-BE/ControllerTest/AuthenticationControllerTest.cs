using CapstoneProject_BE.DTO;
using Irony.Parsing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Test.CapstoneProject_BE.ControllerTest
{
    public class AuthenticationControllerTest
    {
        private readonly string port = APIconfig.port;

        [Fact]
        public async Task Login_LoginSuccessfull_ReturnStatusOk()
        {
            var login = new LoginModel
            {
                Email = "nnphuong22@gmail.com",
                Password = "Phuong123@"
            };

            // Arrange 
            var client = new HttpClient();

            // Act
            StringContent content = new(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(port + "Authentication/Login", content);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async Task Login_WrongPassword_ReturnBadRequest()
        {
            var login = new LoginModel
            {
                Email = "nnphuong22@gmail.com",
                Password = "Phuong123@1234"
            };

            // Arrange 
            var client = new HttpClient();

            // Act
            StringContent content = new(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(port + "Authentication/Login", content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task Login_NonexistedEmail_ReturnBadRequest()
        {
            var login = new LoginModel
            {
                Email = "nnphuong123@gmail.com",
                Password = "Phuong123@1234"
            };

            // Arrange 
            var client = new HttpClient();

            // Act
            StringContent content = new(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(port + "Authentication/Login", content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task ResetPasswordByEmail_NonexistedEmail_ReturnBadRequest()
        {
            var email = "nnphuong123@gmail.com";

            // Arrange 
            var client = new HttpClient();

            // Act
            StringContent content = new(JsonConvert.SerializeObject(email), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(port + "Authentication/ResetPasswordByEmail?email=nnphuogn123%40gmail.com", content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task ResetPasswordByEmail_AvailableEmail_ReturnBadRequest()
        {
            var email = "nnphuong22@gmail.com";

            // Arrange 
            var client = new HttpClient();

            // Act
            StringContent content = new(JsonConvert.SerializeObject(email), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(port + "Authentication/ResetPasswordByEmail?email=nnphuong22%40gmail.com", content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async Task ResetPassword_InvalidToken_ReturnBadRequest()
        {
            var email = "nnphuong22@gmail.com";

            // Arrange 
            var client = new HttpClient();

            // Act
            StringContent content = new(JsonConvert.SerializeObject(email), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(port + "Authentication/ResetPassword?token=1234&newpwd=1234", content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task ConfirmEmail_InvalidToken_ReturnBadRequest()
        {
            var email = "nnphuong22@gmail.com";

            // Arrange 
            var client = new HttpClient();

            // Act
            StringContent content = new(JsonConvert.SerializeObject(email), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(port + "Authentication/ConfirmEmail?token=123", content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task SignUp_ExistedEmail_ReturnBadRequest()
        {
            var signUp = new LoginModel
            {
                Email = "nnphuong22@gmail.com",
                Password = "Phuong123@1234"
            };

            // Arrange 
            var client = new HttpClient();

            // Act
            StringContent content = new(JsonConvert.SerializeObject(signUp), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(port + "Authentication/SignUp", content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task SignUp_ValidEmail_ReturnOk()
        {
            var userCreate = new UserDTO
            {
                UserCode = "User1",
                UserName = "Phuong",
                Email = "kurumi2201@gmail.com",
                RoleId = 1,
            };

            // Arrange 
            var client = new HttpClient();

            // Act
            StringContent content = new(JsonConvert.SerializeObject(userCreate), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(port + "Authentication/SignUp", content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
