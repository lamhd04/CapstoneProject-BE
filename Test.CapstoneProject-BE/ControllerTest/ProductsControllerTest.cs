using AssertExLib;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Xunit;
using CapstoneProject_BE.Models;
using CapstoneProject_BE.DTO;

namespace Test.CapstoneProject_BE.ControllerTest
{
    public class ProductsControllerTest
    {
        private readonly string port = APIconfig.port;
        private readonly string token = APIconfig.token;

        [Fact]
        public async Task GetAllProduct()
        {
            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = new HttpRequestMessage(HttpMethod.Get, port + "Products/Get?offset=0&limit=1&catId=0&supId=0");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var productList = JsonConvert.DeserializeObject<ResponseData<Product>>(result);
            Assert.True(productList.Data.Count() > 0);
            Assert.Equal(1, productList.Data.Count());
        }
        [Fact]
        public async Task GetProductDetail_TrueProductId_ReturnProduct()
        {
            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = new HttpRequestMessage(HttpMethod.Get, port + "Products/GetDetail?prodId=2");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var productList = JsonConvert.DeserializeObject<Product>(result);
            Assert.NotNull(productList);
        }
        [Fact]
        public async Task GetProductDetail_NonExistedProductId_ReturnNull()
        {
            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = new HttpRequestMessage(HttpMethod.Get, port + "Products/GetDetail?prodId=21234");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            var productList = JsonConvert.DeserializeObject<Product>(result);
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public async Task PostProduct_CreateSuccessfull_ReturnStatusOk()
        {
            var productCreate = new ProductDTO
            {
                ProductName = "Kẹo dẻo",
                CategoryId = 2,
                SupplierId = 2,
                DefaultMeasuredUnit = "Thùng",
                InStock = 0,
                StockPrice = 0,
                Image = "image",
                Status = true,
                MeasuredUnits = null
            };

            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            StringContent content = new(JsonConvert.SerializeObject(productCreate), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(port + "Products/PostProduct", content);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async Task PostProduct_ProductNameNull_ReturnBadRequest()
        {
            var productCreate = new ProductDTO
            {
                ProductName = null,
                CategoryId = 1,
                SupplierId = 1,
                DefaultMeasuredUnit = "Thùng",
                InStock = 0,
                StockPrice = 0,
                Image = "image",
                Status = true,
                MeasuredUnits = null
            };

            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            StringContent content = new(JsonConvert.SerializeObject(productCreate), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(port + "Products/PostProduct", content);
            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task PostProduct_CategoryIdNull_ReturnBadRequest()
        {
            var productCreate = new ProductDTO
            {
                ProductName = "Kẹo dẻo",
                CategoryId = null,
                SupplierId = 1,
                DefaultMeasuredUnit = "Thùng",
                InStock = 0,
                StockPrice = 0,
                Image = "image",
                Status = true,
                MeasuredUnits = null
            };

            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            StringContent content = new(JsonConvert.SerializeObject(productCreate), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(port + "Products/PostProduct", content);
            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }
        [Fact]
        public async Task PostProduct_SupplierIdNull_ReturnBadRequest()
        {
            var productCreate = new ProductDTO
            {
                ProductName = "Kẹo dẻo",
                CategoryId = 1,
                SupplierId = null,
                DefaultMeasuredUnit = "Thùng",
                InStock = 0,
                StockPrice = 0,
                Image = "image",
                Status = true,
                MeasuredUnits = null
            };

            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            StringContent content = new(JsonConvert.SerializeObject(productCreate), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(port + "Products/PostProduct", content);
            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }
        [Fact]
        public async Task PostProduct_NonExistedSupplierId_ReturnBadRequest()
        {
            var productCreate = new ProductDTO
            {
                ProductName = "Kẹo dẻo",
                CategoryId = 1,
                SupplierId = 123143,
                DefaultMeasuredUnit = "Thùng",
                InStock = 0,
                StockPrice = 0,
                Image = "image",
                Status = true,
                MeasuredUnits = null
            };

            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            StringContent content = new(JsonConvert.SerializeObject(productCreate), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(port + "Products/PostProduct", content);
            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }
        [Fact]
        public async Task PostProduct_NonExistedCategoryId_ReturnBadRequest()
        {
            var productCreate = new ProductDTO
            {
                ProductName = "Kẹo dẻo",
                CategoryId = 12342134,
                SupplierId = 1,
                DefaultMeasuredUnit = "Thùng",
                InStock = 0,
                StockPrice = 0,
                Image = "image",
                Status = true,
                MeasuredUnits = null
            };

            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            StringContent content = new(JsonConvert.SerializeObject(productCreate), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(port + "Products/PostProduct", content);
            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }
        [Fact]
        public async Task PutProduct_NonExistedProductId_ReturnBadRequest()
        {
            var productCreate = new ProductDTO
            {
                ProductId = 2,
                ProductName = "Kẹo dẻo",
                CategoryId = 1,
                SupplierId = 1,
                DefaultMeasuredUnit = "Thùng",
                InStock = 0,
                StockPrice = 0,
                Image = "image",
                Status = true,
                MeasuredUnits = null
            };

            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            StringContent content = new(JsonConvert.SerializeObject(productCreate), Encoding.UTF8, "application/json");
            var response = await client.PutAsync(port + "Products/PutProduct", content);
            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }
    }
}
