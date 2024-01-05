using ErrorOr;

namespace Domain.Products
{
    public static partial class Errors
    {
        public static class Product
        {
            public static Error NameRequired => Error.Validation("Product.Name", "Name property is required.");
            public static Error InvalidStatus => Error.Validation("Product.Status", "Invalid Status has been supplied.");
            public static Error NegativeStock => Error.Validation("Product.Stock", "Stock cannot be negative.");
            public static Error NegativePrice => Error.Validation("Product.Price", "Price cannot be negative.");
        }
    }
}
