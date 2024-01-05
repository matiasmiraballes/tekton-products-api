using Domain.Products;

namespace Infrastructure.Services
{
    public interface IProductsDiscountService
    {
        Task<int> GetDiscountAsync(ProductId productId, CancellationToken cancellationToken = default);
    }
}
