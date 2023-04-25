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
using System.Threading.Tasks;

namespace Test.CapstoneProject_BE.ControllerTest
{
    public class CategoriesControllerTest
    {
        private readonly string port = APIconfig.port;
        private readonly string token = APIconfig.token;

        [Fact]
        public async Task GetAllCategories()
        {
            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = new HttpRequestMessage(HttpMethod.Get, port + "Categories/Get?offset=0&limit=2");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var categorytList = JsonConvert.DeserializeObject<ResponseData<Category>>(result);
            Assert.True(categorytList.Data.Count() > 0);
            Assert.Equal(2, categorytList.Data.Count());
        }

        [Fact]
        public async Task GetCategoryDetail_TrueCategoryId_ReturnCategory()
        {
            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = new HttpRequestMessage(HttpMethod.Get, port + "Categories/GetDetail?catId=2");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var categoryList = System.Text.Json.JsonSerializer.Deserialize<Category>(result);
            Assert.NotNull(categoryList);
        }

        [Fact]
        public async Task GetCategoryListDetail_NonExistedCategoryId_ReturnNull()
        {
            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = new HttpRequestMessage(HttpMethod.Get, port + "Categories/GetDetail?catId=1234");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PostCategory_CreateSuccessfull_ReturnStatusOk()
        {
            var categoryCreate = new CategoryDTO
            {
                CategoryName="Bánh ngọt",
                Description= "Bánh quy nướng nhân sữa"
            };

            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            StringContent content = new(JsonConvert.SerializeObject(categoryCreate), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(port + "Categories/PostCategory", content);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async Task PutSupplier_UpdateSuccessful_ReturnOk()
        {
            var categoryCreate = new CategoryDTO
            {
                CategoryId = 2,
                CategoryName = "Bánh quy bơ",
                Description = "Bánh quy nướng nhân sữa"
            };

            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            StringContent content = new(JsonConvert.SerializeObject(categoryCreate), Encoding.UTF8, "application/json");
            var response = await client.PutAsync(port + "Categories/PutCategory", content);
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async Task PutSupplier_NonExistedSupplierId_ReturnBadRequest()
        {
            var categoryCreate = new CategoryDTO
            {
                CategoryId = 1234,
                CategoryName = "Bánh quy bơ",
                Description = "Bánh quy nướng nhân sữa"
            };

            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            StringContent content = new(JsonConvert.SerializeObject(categoryCreate), Encoding.UTF8, "application/json");
            var response = await client.PutAsync(port + "Categories/PutCategoryt", content);
            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
