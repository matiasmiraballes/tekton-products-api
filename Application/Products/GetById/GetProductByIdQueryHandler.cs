using Application.Products.Common;
using Infrastructure.Services;
using Domain.Products;
using ErrorOr;
using MediatR;
using Infrastructure.Persistence.DataProvider.DataClient;

namespace Application.Products.GetById
{
    public record GetProductByIdQuery(Guid ProductId) : IRequest<ErrorOr<ProductResponse>>;

    internal sealed class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ErrorOr<ProductResponse>>
    {
        private readonly IProductsDiscountService _productsDiscountService;
        private readonly IProductStatusService _productStatusService;
        private readonly InMemoryDbContext _context;

        public GetProductByIdQueryHandler(IProductsDiscountService productsDiscountService, IProductStatusService productStatusService, InMemoryDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _productsDiscountService = productsDiscountService ?? throw new ArgumentNullException(nameof(productsDiscountService));
            _productStatusService = productStatusService ?? throw new ArgumentNullException(nameof(productStatusService));
        }

        public async Task<ErrorOr<ProductResponse>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            // For queries we can either use the repository or access the dbContext directly for better performance and flexibility.
            // Product? product = await _productsRepository.GetByIdAsync(new ProductId(request.ProductId), cancellationToken);

            Product? product = (await _context.Products.FindAsync(request.ProductId, cancellationToken))?.ToModel();

            if (product is null)
            {
                return Errors.Product.NotFound(request.ProductId);
            }

            int productDiscount = await _productsDiscountService.GetDiscountAsync(product!.ProductId, cancellationToken);
            var statusDictionary = _productStatusService.GetProductStatusDictionary();

            ProductResponse response = new ProductResponse(
                product.ProductId.Value,
                product.Name,
                statusDictionary[product.Status],
                product.Stock,
                product.Description,
                product.Price,
                productDiscount,
                product.Price * (100 - productDiscount) / 100
            );

            return response;
        }
    }
}
