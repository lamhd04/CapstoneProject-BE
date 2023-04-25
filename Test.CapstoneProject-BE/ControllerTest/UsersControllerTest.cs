using CapstoneProject_BE.DTO;
using CapstoneProject_BE.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Test.CapstoneProject_BE.ControllerTest
{
    public class UsersControllerTest
    {
        private readonly string port = APIconfig.port;
        private readonly string token = APIconfig.token;

        [Fact]
        public async Task GetUsers_WithoutAccessToken_ReturnUndocumented()
        {
            // Arrange 
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, port + "Users/GetUsers?offset=0&limit=10&roleid=1&status=true");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
        [Fact]
        public async Task GetUsers_WithAccessToken_ReturnOK()
        {
            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = new HttpRequestMessage(HttpMethod.Get, port + "Users/GetUsers?offset=0&limit=10&roleid=1&status=true");
            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async Task GetUserDetail_TrueUserId_ReturnUser()
        {
            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = new HttpRequestMessage(HttpMethod.Get, port + "Users/GetUserDetail?userid=01");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var import = JsonConvert.DeserializeObject<User>(result);
            Assert.NotNull(import);
        }

        [Fact]
        public async Task GetUserDetail_NonExistedUserId_ReturnNull()
        {
            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = new HttpRequestMessage(HttpMethod.Get, port + "Users/GetUserDetail?userid=01234");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task Deactivate_ActivateSuccessfull_ReturnStatusOk()
        {

            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            await client.PostAsync(port + "Users/Activate?userid=1", null);

            // Act
            var response = await client.PostAsync(port + "Users/Deactivate?userid=1", null);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async Task Deactivate_InappropriateStatus_ReturnBadRequest()
        {

            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await client.PostAsync(port + "Users/Deactivate?userid=4", null);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task Activate_ActivateSuccessfull_ReturnStatusOk()
        {

            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            await client.PostAsync(port + "Users/Deactivate?userid=2", null);

            // Act
            var response = await client.PostAsync(port + "Users/Activate?userid=2", null);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async Task Activate_InappropriateStatus_ReturnBadRequest()
        {

            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await client.PostAsync(port + "Users/Deactivate?userid=3", null);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task CreateUser_CreateSuccessfull_ReturnStatusOk()
        {
            var userCreate = new UserDTO
            {
                UserCode = "User113",
                UserName = "Phuong121",
                Email = "abcs@gmail.com",
                RoleId = 1,
            };

            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            StringContent content = new(JsonConvert.SerializeObject(userCreate), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(port + "Users/CreateStaff", content);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task CreateUser_ExistedEmail_ReturnBadRequest()
        {
            var userCreate = new UserDTO
            {
                UserCode = "User1",
                UserName = "Phuong",
                Email = "abc@gmail.com",
                RoleId = 1,
            };

            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            StringContent content = new(JsonConvert.SerializeObject(userCreate), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(port + "Users/CreateStaff", content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task EditUser_InvalidUserId_ReturnInternalServerError()
        {
            var userCreate = new UserDTO
            {
                UserId = 9,
                UserCode = "AB98989",
                UserName = "KmqftTD5",
                Email = "nnphuong322@gmail.com",
                RoleId = 1,
                Gender = false,
            };

            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            StringContent content = new(JsonConvert.SerializeObject(userCreate), Encoding.UTF8, "application/json");
            var response = await client.PutAsync(port + "Users/UpdateStaff", content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ChangeStaffPassword_ChangeSuccessfull_ReturnOK()
        {
            var userCreate = new PasswordDTO
            {
                UserId = 5,
                Password = "Phuong123@",
                OldPassword = "Phuong123@"
            };

            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            StringContent content = new(JsonConvert.SerializeObject(userCreate), Encoding.UTF8, "application/json");
            var response = await client.PutAsync(port + "Users/ChangePassword", content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async Task ChangeStaffPassword_NonExistedUserId_ReturnBadRequest()
        {
            var userCreate = new PasswordDTO
            {
                UserId = 51241,
                Password = "Phuong123@",
                OldPassword = "Phuong123@"
            };

            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            StringContent content = new(JsonConvert.SerializeObject(userCreate), Encoding.UTF8, "application/json");
            var response = await client.PutAsync(port + "Users/ChangePassword", content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task ChangeUserPassword_ChangeSuccessful_ReturnOK()
        {
            var userCreate = new PasswordDTO
            {
                UserId = 02,
                Password = "Phuong123@",
                OldPassword = "string"
            };

            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            StringContent content = new(JsonConvert.SerializeObject(userCreate), Encoding.UTF8, "application/json");
            var response = await client.PutAsync(port + "Users/ChangePassword", content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async Task ChangeUserPassword_WrongOldPassword_ReturnBadRequest()
        {
            var userCreate = new PasswordDTO
            {
                UserId = 0,
                Password = "Phuong123@",
                OldPassword = "Phuong12312@"
            };

            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            StringContent content = new(JsonConvert.SerializeObject(userCreate), Encoding.UTF8, "application/json");
            var response = await client.PutAsync(port + "Users/ChangePassword", content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

    }
}
