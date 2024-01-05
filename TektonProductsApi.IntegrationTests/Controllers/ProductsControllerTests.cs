using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace TektonProductsApi.IntegrationTests.Controllers
{
    public class ProductsControllerTests
    {
        private CustomWebApplicationFactory _factory;
        private HttpClient _client;

        public ProductsControllerTests()
        {
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();
        }


        [Fact]
        public async Task Post_ProductReturns_201CreatedForValidInput()
        {
            // Arrange
            var validProduct = new
            {
                name = "Sample Product",
                status = 1,
                stock = 10,
                description = "Sample description",
                price = 29.99
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/Products")
            {
                Content = new StringContent(JsonSerializer.Serialize(validProduct), Encoding.UTF8, "application/json")
            };

            // Act
            var response = await _client.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();
            

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            // Check for the existence of the Location header
            Assert.True(response.Headers.Location is not null);
            // Example: Assert that the Location URI contains the correct path or segments
            Assert.Contains("/Products", response.Headers.Location.OriginalString);
        }

        [Fact]
        public async Task Post_ProductReturns_400ForInvalidProductName()
        {
            // Arrange
            var invalidProduct = new
            {
                name = "",  // name is mandatory
                status = 1,
                stock = 10,
                description = "Sample description",
                price = 29.99
            };

            // Convert the product to JSON
            var request = new HttpRequestMessage(HttpMethod.Post, "/Products")
            {
                Content = new StringContent(JsonSerializer.Serialize(invalidProduct), Encoding.UTF8, "application/json")
            };

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var responseBody = await response.Content.ReadAsStringAsync();
            var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.Equal("Product.Name", errorResponse.Code);
            Assert.Equal("Name property is required.", errorResponse.Description);
        }

        [Fact]
        public async Task Post_ProductReturns_400ForInvalidProductStatus()
        {
            // Arrange
            var invalidProduct = new
            {
                name = "Sample Product",
                status = 2, // status can be "0" or "1"
                stock = 10,
                description = "Sample description",
                price = 29.99
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/Products")
            {
                Content = new StringContent(JsonSerializer.Serialize(invalidProduct), Encoding.UTF8, "application/json")
            };

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var responseBody = await response.Content.ReadAsStringAsync();
            var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.Equal("Product.Status", errorResponse.Code);
            Assert.Equal("Invalid Status has been supplied.", errorResponse.Description);
        }

        [Fact]
        public async Task Post_ProductReturns_400ForInvalidProductStock()
        {
            // Arrange
            var invalidProduct = new
            {
                name = "Sample Product",
                status = 1,
                stock = -10, // stock cannot be negative
                description = "Sample description",
                price = 29.99
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/Products")
            {
                Content = new StringContent(JsonSerializer.Serialize(invalidProduct), Encoding.UTF8, "application/json")
            };

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var responseBody = await response.Content.ReadAsStringAsync();
            var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.Equal("Product.Stock", errorResponse.Code);
            Assert.Equal("Stock cannot be negative.", errorResponse.Description);
        }

        [Fact]
        public async Task Post_ProductReturns_400ForInvalidProductPrice()
        {
            // Arrange
            var invalidProduct = new
            {
                name = "Sample Product",
                status = 1,
                stock = 10,
                description = "Sample description",
                price = -29.99 // price cannot be negative
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/Products")
            {
                Content = new StringContent(JsonSerializer.Serialize(invalidProduct), Encoding.UTF8, "application/json")
            };

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var responseBody = await response.Content.ReadAsStringAsync();
            var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.Equal("Product.Price", errorResponse.Code);
            Assert.Equal("Price cannot be negative.", errorResponse.Description);
        }

        [Fact]
        public async Task Put_ProductReturns_204ForValidProductData()
        {
            // Arrange
            var seededProductGuid = new Guid("b1e76df0-11db-4402-a1b0-cb5e3b88bf0a");

            var validProduct = new
            {
                productId = seededProductGuid,
                name = "Sample Product Updated",
                status = 0,
                stock = 50,
                description = "Sample description Updated",
                price = 59.99
            };

            var request = new HttpRequestMessage(HttpMethod.Put, "/Products")
            {
                Content = new StringContent(JsonSerializer.Serialize(validProduct), Encoding.UTF8, "application/json")
            };
            var getRequest = new HttpRequestMessage(HttpMethod.Get, "/Products/" + seededProductGuid.ToString());

            // Act
            var response = await _client.SendAsync(request);

            var getResponse = await _client.SendAsync(getRequest);
            var getResponseBody = await getResponse.Content.ReadAsStringAsync();
            var productResponse = JsonSerializer.Deserialize<ProductResponse>(getResponseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equal(validProduct.productId, productResponse.ProductId);
            Assert.Equal(validProduct.name, productResponse.Name);
            Assert.Equal(validProduct.description, productResponse.Description);
            Assert.Equal(validProduct.stock, productResponse.Stock);
        }

        [Fact]
        public async Task Get_ProductByIdReturns_200ForExistentProduct()
        {

            // Arrange
            var seededProductGuid = new Guid("b1e76df0-11db-4402-a1b0-cb5e3b88bf0a");
            var request = new HttpRequestMessage(HttpMethod.Get, "/Products/" + seededProductGuid.ToString());

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseBody = await response.Content.ReadAsStringAsync();
            var productResponse = JsonSerializer.Deserialize<ProductResponse>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Assert that all properties are not null
            Assert.NotNull(productResponse.Name);
            Assert.NotNull(productResponse.StatusName);
            Assert.NotNull(productResponse.Description);
        }

        [Fact]
        public async Task Get_ProductByIdReturns_404ForNonExistentProduct()
        {
            // Arrange
            var product1Guid = Guid.Empty;
            var request = new HttpRequestMessage(HttpMethod.Get, "/Products/" + product1Guid.ToString());

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }

    public record ProductResponse(
        Guid ProductId,
        string Name,
        string StatusName,
        int Stock,
        string Description,
        decimal Price,
        decimal Discount,
        decimal FinalPrice
    );

    public record ErrorResponse(
        string Code,
        string Description
    );
}
