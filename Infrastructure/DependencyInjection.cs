using Domain.Products;
using Infrastructure.Persistence.DataProvider.DataClient;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddPersistence(configuration);

            services.AddMemoryCache();
            services.AddScoped<IProductStatusService, ProductStatusService>();
            services.AddScoped<IProductsDiscountService, ProductsDiscountService>();

            return services;
        }

        private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<InMemoryDbContext>();
            services.AddScoped<IProductsRepository, ProductsRepository>();

            return services;
        }
    }
}
