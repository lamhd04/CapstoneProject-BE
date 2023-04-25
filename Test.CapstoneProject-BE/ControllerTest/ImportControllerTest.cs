using CapstoneProject_BE.Controllers.Import;
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
    public class ImportControllerTest
    {
        private readonly string port = APIconfig.port;
        private readonly string token = APIconfig.token;

        [Fact]
        public async Task GetAllImport()
        {
            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = new HttpRequestMessage(HttpMethod.Get, port + "Import/GetImportOrder?offset=0&limit=5&supId=0&state=-1");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var importList = JsonConvert.DeserializeObject<ResponseData<ImportOrder>>(result);
            Assert.True(importList.Data.Count() > 0);
            Assert.Equal(5, importList.Data.Count());
        }

        [Fact]
        public async Task GetImportDetail_TrueImportId_ReturnImport()
        {
            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = new HttpRequestMessage(HttpMethod.Get, port + "Import/GetImportDetail?importid=1");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var import = JsonConvert.DeserializeObject<ImportOrder>(result);
            Assert.NotNull(import);
        }

        [Fact]
        public async Task GetImportDetail_NonExistedImportId_ReturnNull()
        {
            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = new HttpRequestMessage(HttpMethod.Get, port + "Import/GetImportDetail?importid=2234");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetDetail_TrueImportCode_ReturnImport()
        {
            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = new HttpRequestMessage(HttpMethod.Get, port + "Import/GetDetail?importcode=NAHA1");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var import = JsonConvert.DeserializeObject<ImportOrder>(result);
            Assert.NotNull(import);
        }

        [Fact]
        public async Task GetDetail_NonExistedImportCode_ReturnBadRequest()
        {
            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = new HttpRequestMessage(HttpMethod.Get, port + "Import/GetDetail?importcode=NAHA12341");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public async Task ApproveImport_ApproveSuccessful_ReturnOK()
        {
            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = new HttpRequestMessage(HttpMethod.Post, port + "Import/ApproveImport?importid=9");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ApproveImport_InappropriateImportOrderStatus_ReturnBadRequest()
        {
            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = new HttpRequestMessage(HttpMethod.Post, port + "Import/ApproveImport?importid=1");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task DenyImport_DenySuccessful_ReturnOK()
        {
            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = new HttpRequestMessage(HttpMethod.Post, port + "Import/DenyImport?importid=10");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DenyImport_InappropriateImportOrderStatus_ReturnBadRequest()
        {
            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = new HttpRequestMessage(HttpMethod.Post, port + "Import/DenyImport?importid=1");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Import_ImportSuccessful_ReturnOK()
        {
            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = new HttpRequestMessage(HttpMethod.Post, port + "Import/Import?importid=1002");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Import_InappropriateImportOrderStatus_ReturnBadRequest()
        {
            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = new HttpRequestMessage(HttpMethod.Post, port + "Import/Import?importid=5");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateImportOrder_CreateSuccessfull_ReturnStatusOk()
        {
            var productCreate = new ImportOrderDTO
            {
                SupplierId = 1,
                UserId = 1,
                TotalCost = 0,
                TotalAmount = 0,
                State = 0,
                ImportOrderDetails = new List<ImportDetailDTO>
                {
                    new ImportDetailDTO
                    {
                        ProductId= 2,
                        Amount = 2,
                        CostPrice = 15000,
                        MeasuredUnitId=0
                    }
                }
            };

            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            StringContent content = new(JsonConvert.SerializeObject(productCreate), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(port + "Import/CreateImportOrder", content);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

    }
}
