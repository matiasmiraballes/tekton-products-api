using Domain.Products;
using Infrastructure.Persistence.DataProvider.DataClient;
using Infrastructure.Persistence.DataProvider.DbModels;

namespace Infrastructure.Persistence.Repositories
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly InMemoryDbContext _context;

        public ProductsRepository(InMemoryDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddAsync(Product product, CancellationToken cancellationToken)
        {
            var inMemoryProduct = product.FromModel();

            await _context.Products.AddAsync(inMemoryProduct, cancellationToken);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
