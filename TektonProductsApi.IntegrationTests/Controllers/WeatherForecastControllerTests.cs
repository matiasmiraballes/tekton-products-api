using System.Net;
using Xunit;

namespace TektonProductsApi.IntegrationTests.Controllers
{
    public class WeatherForecastControllerTests
    {
        private CustomWebApplicationFactory _factory;
        private HttpClient _client;

        public WeatherForecastControllerTests()
        {
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task T000_Get_WeatherForecast_ShouldReturn200()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "/WeatherForecast");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
