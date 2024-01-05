using Domain.Products;

namespace Infrastructure.Services
{
    public class ProductsDiscountService : IProductsDiscountService
    {
        public Task<int> GetDiscountAsync(ProductId productId, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(0);
        }
    }
}
