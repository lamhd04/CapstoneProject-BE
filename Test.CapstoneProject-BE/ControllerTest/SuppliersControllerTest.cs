using CapstoneProject_BE.Controllers.Product;
using CapstoneProject_BE.DTO;
using CapstoneProject_BE.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Test.CapstoneProject_BE.ControllerTest
{
    public class SuppliersControllerTest
    {
        private readonly string port = APIconfig.port;
        private readonly string token = APIconfig.token;

        [Fact]
        public async Task GetAllSuppliers()
        {
            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = new HttpRequestMessage(HttpMethod.Get, port + "Suppliers/Get?offset=0&limit=10");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var productList = JsonConvert.DeserializeObject<ResponseData<Supplier>>(result);
            Assert.True(productList.Data.Count() > 0);
            Assert.Equal(5, productList.Data.Count());
        }

        [Fact]
        public async Task GetSupplierDetail_TrueSupplierId_ReturnSupplier()
        {
            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = new HttpRequestMessage(HttpMethod.Get, port + "Suppliers/GetDetail?supId=4");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var supplierList = System.Text.Json.JsonSerializer.Deserialize<Supplier>(result);
            Assert.NotNull(supplierList);
        }

        [Fact]
        public async Task GetSupplierDetail_NonExistedSupplierId_ReturnNull()
        {
            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = new HttpRequestMessage(HttpMethod.Get, port + "Suppliers/GetDetail?supId=51234");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PostSupplier_CreateSuccessfull_ReturnStatusOk()
        {
            var supplierCreate = new SupplierDTO
            {
                SupplierName = "Hải Hà",
                SupplierPhone = "0948123673",
                City = new City { Id = 1, Name = "Hà Nội" },
                District = new District { Id = 1, Name="Đống Đa" },
                Ward = new Ward { Id=1, Name= "phường Đống Đa"},
                Address = "Số nhà 12, ngõ 14",
                Note = "Làm việc giờ hành chính",
                SupplierEmail = "HaiHa@gmail.com",
                Status = true
            };

            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            StringContent content = new(JsonConvert.SerializeObject(supplierCreate), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(port + "Suppliers/PostSupplier", content);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async Task PutSupplier_NonExistedSupplierId_ReturnBadRequest()
        {
            var supplierCreate = new SupplierDTO
            {
                SupplierId = 1123,
                SupplierName = "Hải Hà",
                SupplierPhone = "0948123673",
                City = new City { Id = 1, Name = "Hà Nội" },
                District = new District { Id = 1, Name = "Đống Đa" },
                Ward = new Ward { Id = 1, Name = "phường Đống Đa" },
                Address = "Số nhà 12, ngõ 14",
                Note = "Làm việc giờ hành chính",
                SupplierEmail = "HaiHa@gmail.com",
                Status = true
            };

            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            StringContent content = new(JsonConvert.SerializeObject(supplierCreate), Encoding.UTF8, "application/json");
            var response = await client.PutAsync(port + "Products/PutProduct", content);
            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
