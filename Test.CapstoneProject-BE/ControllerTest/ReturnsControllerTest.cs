using CapstoneProject_BE.DTO;
using CapstoneProject_BE.Models;
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
    public class ReturnsControllerTest
    {
        private readonly string port = APIconfig.port;
        private readonly string token = APIconfig.token;

        [Fact]
        public async Task GetAllReturn()
        {
            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = new HttpRequestMessage(HttpMethod.Get, port + "Returns/Get?offset=0&limit=5&type=import&suppid=0");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var returnList = JsonConvert.DeserializeObject<ResponseData<ReturnsOrder>>(result);
            Assert.True(returnList.Data.Count() > 0);
            Assert.Equal(5, returnList.Data.Count());
        }
        [Fact]
        public async Task GetReturnDetail_TrueReturnId_ReturnReturn()
        {
            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = new HttpRequestMessage(HttpMethod.Get, port + "Returns/GetReturnsDetail?returnid=1");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var Return = JsonConvert.DeserializeObject<ReturnsOrder>(result);
            Assert.NotNull(Return);
        }

        [Fact]
        public async Task GetReturnDetail_NonExistedReturnId_ReturnNull()
        {
            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = new HttpRequestMessage(HttpMethod.Get, port + "Returns/GetReturnsDetail?returnid=11234");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetDetail_TrueReturnCode_ReturnReturn()
        {
            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = new HttpRequestMessage(HttpMethod.Get, port + "Returns/GetDetail?returncode=TAHA1");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var Return = JsonConvert.DeserializeObject<ReturnsOrder>(result);
            Assert.NotNull(Return);
        }

        [Fact]
        public async Task GetDetail_NonExistedReturnCode_ReturnBadRequest()
        {
            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = new HttpRequestMessage(HttpMethod.Get, port + "Returns/GetDetail?returncode=TAHA1112341");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task CreateReturnOrder_CreateSuccessfull_ReturnStatusOk()
        {
            var productCreate = new ReturnsDTO
            {
                ImportId = 11,
                SupplierId = 2,
                UserId = 1,
                Note = "",
                Media = "",
                ReturnsOrderDetails = new List<ReturnsDetailDTO>
                {
                    new ReturnsDetailDTO
                    {
                        ProductId= 2,
                        ReturnsId=1,
                        Amount = 2,
                        Price=20000,
                        MeasuredUnitId=0
                    }
                }
            };

            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            StringContent content = new(JsonConvert.SerializeObject(productCreate), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(port + "Returns/Create", content);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetAvailable_TrueImportId_ReturnReturn()
        {
            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = new HttpRequestMessage(HttpMethod.Get, port + "Returns/GetAvailable?importid=11");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var Return = JsonConvert.DeserializeObject<List<AvailableDTO>>(result);
            Assert.NotNull(Return);
        }
        [Fact]
        public async Task GetAvailable_WrongImportId_ReturnReturn()
        {
            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = new HttpRequestMessage(HttpMethod.Get, port + "Returns/GetAvailable?importid=113241");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var Return = JsonConvert.DeserializeObject<List<AvailableDTO>>(result);
            Assert.Equal(Return, new List<AvailableDTO>());
        }
    }
}
