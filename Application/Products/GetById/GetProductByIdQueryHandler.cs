using Application.Products.Common;
using Infrastructure.Services;
using Domain.Products;
using ErrorOr;
using MediatR;

namespace Application.Products.GetById
{
    public record GetProductByIdQuery(Guid ProductId) : IRequest<ErrorOr<ProductResponse>>;

    internal sealed class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ErrorOr<ProductResponse>>
    {
        private readonly IProductsRepository _productsRepository;
        private readonly IProductsDiscountService _productsDiscountService;
        private readonly IProductStatusService _productStatusService;

        public GetProductByIdQueryHandler(IProductsRepository productsRepository, IProductsDiscountService productsDiscountService, IProductStatusService productStatusService)
        {
            _productsRepository = productsRepository ?? throw new ArgumentNullException(nameof(productsRepository));
            _productsDiscountService = productsDiscountService ?? throw new ArgumentNullException(nameof(productsDiscountService));
            _productStatusService = productStatusService ?? throw new ArgumentNullException(nameof(productStatusService));
        }

        public async Task<ErrorOr<ProductResponse>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            Product? product = await _productsRepository.GetByIdAsync(new ProductId(request.ProductId), cancellationToken);

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
