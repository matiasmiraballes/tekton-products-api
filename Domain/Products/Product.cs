using Domain.Primitives;
using ErrorOr;

namespace Domain.Products
{
    public sealed class Product : AggregateRoot
    {
        private const int NAME_MINLENGTH = 3;
        private static readonly int[] VALID_STATUSES = new int[] { 0, 1 };

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
        public int Stock { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }

        public static ErrorOr<Product> Create(ProductId productId, string name, int status, int stock, string description, decimal price)
        {
            if (String.IsNullOrEmpty(name) || name.Length < NAME_MINLENGTH)
            {
                return Errors.Product.NameRequired;
            }

            if (!VALID_STATUSES.Contains(status))
            {
                return Errors.Product.InvalidStatus;
            }

            if (stock < 0)
            {
                return Errors.Product.NegativeStock;
            }

            if (price < 0)
            {
                return Errors.Product.NegativePrice;
            }

            return new Product(productId, name, status, stock, description, price);
        }
    }
}
