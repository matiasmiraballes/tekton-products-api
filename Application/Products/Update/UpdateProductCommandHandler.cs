using Domain.Products;
using ErrorOr;
using MediatR;

namespace Application.Products.Update
{
    public record UpdateProductCommand(
            Guid ProductId,
            string Name,
            int Status,
            int Stock,
            string Description,
            decimal Price
        ) : IRequest<ErrorOr<Unit>>;

    internal sealed class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ErrorOr<Unit>>
    {
        private readonly IProductsRepository _productsRepository;

        public UpdateProductCommandHandler(IProductsRepository productsRepository)
        {
            _productsRepository = productsRepository ?? throw new ArgumentNullException(nameof(productsRepository));
        }

        public async Task<ErrorOr<Unit>> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            Product? dbProduct = await _productsRepository.GetByIdAsync(new ProductId(command.ProductId), cancellationToken);

            if (dbProduct == null)
            {
                return Errors.Product.NotFound(command.ProductId);
            }

            var product = Product.Create(
                dbProduct.ProductId,
                command.Name,
                command.Status,
                command.Stock,
                command.Description,
                command.Price
            );

            if (product.IsError)
                return product.FirstError;

            await _productsRepository.UpdateAsync(product.Value, cancellationToken);
            await _productsRepository.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
