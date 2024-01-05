using Domain.Products;
using System.Text.Json;

namespace Infrastructure.Services
{
    public class ProductsDiscountService : IProductsDiscountService
    {
        public readonly IHttpClientFactory _clientFactory;

        public ProductsDiscountService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
        }

        public async Task<int> GetDiscountAsync(ProductId productId, CancellationToken cancellationToken = default)
        {
            using HttpClient client = _clientFactory.CreateClient();

            var response = await client.GetAsync("https://www.randomnumberapi.com/api/v1.0/random?min=0&max=100&count=1");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var randomNumbers = JsonSerializer.Deserialize<int[]>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return randomNumbers[0];
            }
            else
            {
                return -1;
            }
        }
    }
}
