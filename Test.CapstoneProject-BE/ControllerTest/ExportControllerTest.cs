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
    public class ExportControllerTest
    {
        private readonly string port = APIconfig.port;
        private readonly string token = APIconfig.token;

        [Fact]
        public async Task GetAllExport()
        {
            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = new HttpRequestMessage(HttpMethod.Get, port + "Export/GetExportOrder?offset=0&limit=5&supId=0&state=-1");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var exportList = JsonConvert.DeserializeObject<ResponseData<ExportOrder>>(result);
            Assert.True(exportList.Data.Count() > 0);
            Assert.Equal(5, exportList.Data.Count());
        }

        [Fact]
        public async Task GetExportDetail_TrueExportId_ReturnExport()
        {
            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = new HttpRequestMessage(HttpMethod.Get, port + "Export/GetExportDetail?exportId=1");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var import = JsonConvert.DeserializeObject<ImportOrder>(result);
            Assert.NotNull(import);
        }

        [Fact]
        public async Task GetExportDetail_NonExistedExportId_ReturnNull()
        {
            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = new HttpRequestMessage(HttpMethod.Get, port + "Export/GetExportDetail?exportId=1234");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetDetail_TrueExportCode_ReturnExport()
        {
            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = new HttpRequestMessage(HttpMethod.Get, port + "Export/GetDetail?exportcode=XAHA1");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var export = JsonConvert.DeserializeObject<ImportOrder>(result);
            Assert.NotNull(export);
        }

        [Fact]
        public async Task GetDetail_NonExistedExportCode_ReturnBadRequest()
        {
            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = new HttpRequestMessage(HttpMethod.Get, port + "Export/GetDetail?exportcode=XAHA112341");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ApproveExport_ApproveSuccessful_ReturnOK()
        {
            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = new HttpRequestMessage(HttpMethod.Post, port + "Export/ApproveExport?exportId=6");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ApproveExport_InappropriateExportOrderStatus_ReturnBadRequest()
        {
            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = new HttpRequestMessage(HttpMethod.Post, port + "Export/ApproveExport?exportId=5");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task DenyExport_DenySuccessful_ReturnOK()
        {
            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = new HttpRequestMessage(HttpMethod.Post, port + "Export/DenyImport?exportId=2");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DenyExport_InappropriateExportOrderStatus_ReturnBadRequest()
        {
            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = new HttpRequestMessage(HttpMethod.Post, port + "Export/DenyImport?exportId=1");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Export_ExportSuccessful_ReturnOK()
        {
            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = new HttpRequestMessage(HttpMethod.Post, port + "Export/Export?exportId=5");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Export_InappropriateExportOrderStatus_ReturnBadRequest()
        {
            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = new HttpRequestMessage(HttpMethod.Post, port + "Export/Export?exportId=2");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task CreateExportOrder_CreateSuccessfull_ReturnStatusOk()
        {
            var productCreate = new ExportOrderDTO
            {
                ExportId = 0,
                State = 0,
                UserId=1,
                ExportCode = "string",
                Note = "",
                ExportOrderDetails = new List<ExportDetailDTO>
                {
                    new ExportDetailDTO
                    {
                        ProductId= 1,
                        Amount = 2,
                        Discount = 1,
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
            var response = await client.PostAsync(port + "Export/CreateExportOrder", content);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
