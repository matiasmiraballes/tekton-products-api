using Domain.Products;
using Infrastructure.Persistence.DataProvider.DataClient;
using Infrastructure.Persistence.DataProvider.DbModels;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly InMemoryDbContext _context;

        public ProductsRepository(InMemoryDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Product?> GetByIdAsync(ProductId productId, CancellationToken cancellationToken)
        {
            var inMemoryProduct = await _context.Products.FindAsync(productId.Value, cancellationToken);

            return inMemoryProduct?.ToModel();
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
