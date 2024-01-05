namespace Domain.Products
{
    public interface IProductsRepository
    {
        Task<Product?> GetByIdAsync(ProductId productId, CancellationToken cancellationToken);
        Task AddAsync(Product product, CancellationToken cancellationToken);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
