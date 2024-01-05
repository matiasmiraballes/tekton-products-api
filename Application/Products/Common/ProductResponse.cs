namespace Application.Products.Common;

public record ProductResponse(
    Guid ProductId,
    string Name,
    string StatusName,
    int Stock,
    string Description,
    decimal Price,
    decimal Discount,
    decimal FinalPrice
);
