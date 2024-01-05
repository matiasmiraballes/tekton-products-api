using Domain.Products;
using ErrorOr;
using MediatR;

namespace Application.Products.Create
{
    public record CreateProductsCommand(
        string Name,
        int Status,
        int Stock,
        string Description,
        decimal Price
    ) : IRequest<ErrorOr<Guid>>;

    internal sealed class CreateProductsCommandHandler : IRequestHandler<CreateProductsCommand, ErrorOr<Guid>>
    {
        private readonly IProductsRepository _productsRepository;

        public CreateProductsCommandHandler(IProductsRepository productsRepository)
        {
            _productsRepository = productsRepository ?? throw new ArgumentNullException(nameof(productsRepository));
        }

        public async Task<ErrorOr<Guid>> Handle(CreateProductsCommand command, CancellationToken cancellationToken)
        {
            var product = Product.Create(
                new ProductId(Guid.NewGuid()),
                command.Name,
                command.Status,
                command.Stock,
                command.Description,
                command.Price
            );

            if (product.IsError)
                return product.FirstError;

            await _productsRepository.AddAsync(product.Value, cancellationToken);
            await _productsRepository.SaveChangesAsync(cancellationToken);

            return product.Value.ProductId.Value;
        }
    }
}
