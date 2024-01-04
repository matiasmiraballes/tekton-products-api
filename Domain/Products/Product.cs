using Domain.Primitives;
using ErrorOr;

namespace Domain.Products
{
    public sealed class Product : AggregateRoot
    {
        public Product(ProductId productId, string name, int status, int stock, string description, decimal price)
        {
            ProductId = productId;
            Name = name;
            Status = status;
            Stock = stock;
            Description = description;
            Price = price;
        }

        public ProductId ProductId { get; private set; }
        public string Name { get; private set; }
        public int Status { get; private set; }
        public int Stock {  get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }

        public static ErrorOr<Product> Create(ProductId productId, string name, int status, int stock, string description, decimal price)
        {
            // TODO: Perform validations here

            return new Product(productId, name, status, stock, description, price);
        }
    }
}
