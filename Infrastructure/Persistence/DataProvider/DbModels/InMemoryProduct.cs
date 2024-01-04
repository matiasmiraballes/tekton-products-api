using Domain.Products;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Persistence.DataProvider.DbModels
{
    internal class InMemoryProduct
    {
        [Key]
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }


        public Product ToModel()
        {
            return new Product(
                new ProductId(ProductId),
                Name,
                Status,
                Stock,
                Description,
                Price
            );
        }
    }

    internal static class ProductExtension
    {
        internal static InMemoryProduct FromModel(this Product product)
        {
            return new InMemoryProduct()
            {
                ProductId = product.ProductId.Value,
                Name = product.Name,
                Status = product.Status,
                Stock = product.Stock,
                Description = product.Description,
                Price = product.Price
            };
        }
    }
}
