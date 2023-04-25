using CapstoneProject_BE.Controllers.Import;
using CapstoneProject_BE.Controllers.Stock;
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
    public class StockTakeControllerTest
    {
        private readonly string port = APIconfig.port;
        private readonly string token = APIconfig.token;

        [Fact]
        public async Task GetAllStocktake()
        {
            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = new HttpRequestMessage(HttpMethod.Get, port + "Stocktake/Get?offset=0&limit=5&state=-1");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var stocktakeList = JsonConvert.DeserializeObject<ResponseData<StocktakeNote>>(result);
            Assert.True(stocktakeList.Data.Count() > 0);
            Assert.Equal(5, stocktakeList.Data.Count());
        }

        [Fact]
        public async Task GetStocktakeDetail_TrueStocktakeId_ReturnStocktake()
        {
            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = new HttpRequestMessage(HttpMethod.Get, port + "Stocktake/GetStocktakeDetail?stocktakeid=1");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var stocktake = JsonConvert.DeserializeObject<ImportOrder>(result);
            Assert.NotNull(stocktake);
        }

        [Fact]
        public async Task GetStocktakeDetail_NonExistedStocktakeId_ReturnNull()
        {
            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = new HttpRequestMessage(HttpMethod.Get, port + "Stocktake/GetStocktakeDetail?stocktakeid=12234");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetDetail_TrueStocktakeCode_ReturnStocktake()
        {
            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = new HttpRequestMessage(HttpMethod.Get, port + "Stocktake/GetDetail?stocktakecode=KIHA1");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var stocktake = JsonConvert.DeserializeObject<StocktakeNote>(result);
            Assert.NotNull(stocktake);
        }

        [Fact]
        public async Task GetDetail_NonExistedImportCode_ReturnBadRequest()
        {
            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = new HttpRequestMessage(HttpMethod.Get, port + "Stocktake/GetDetail?stocktakecode=KIHA112341");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task CreateStocktakeOrder_CreateSuccessfull_ReturnStatusOk()
        {
            var stocktakeCreate = new StocktakeDTO
            {
                StocktakeId = 0,
                StocktakeCode = "KIHA12",
                StorageId = 1,
                Note = "string",
                CreatedId = 1,
                StocktakeNoteDetails = new List<StocktakeDetailDTO>
                {
                    new StocktakeDetailDTO
                    {
                        ProductId= 2,
                        StocktakeId = 0,
                        CurrentStock = 10,
                        ActualStock=8,
                        AmountDifferential=2,
                        
                    }
                }
            };

            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            StringContent content = new(JsonConvert.SerializeObject(stocktakeCreate), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(port + "Stocktake/Create", content);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async Task DenyStocktake_DenySuccessful_ReturnBadRequest()
        {
            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = new HttpRequestMessage(HttpMethod.Post, port + "Stocktake/Cancel?stocktakeid=3");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async Task DenyStocktake_InappropriateStocktakeStatus_ReturnBadRequest()
        {
            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = new HttpRequestMessage(HttpMethod.Post, port + "Stocktake/Cancel?stocktakeid=1");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task Stocktake_StocktakeSuccessful_ReturnBadRequest()
        {
            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = new HttpRequestMessage(HttpMethod.Post, port + "Stocktake/Stocktake?stocktakeid=4");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async Task Stocktake_InappropriateStocktakeOrderStatus_ReturnBadRequest()
        {
            // Arrange 
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = new HttpRequestMessage(HttpMethod.Post, port + "Stocktake/Stocktake?stocktakeid=1");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
