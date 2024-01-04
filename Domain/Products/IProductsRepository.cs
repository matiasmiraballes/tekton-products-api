namespace Domain.Products
{
    public interface IProductsRepository
    {
        Task AddAsync(Product product, CancellationToken cancellationToken);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
