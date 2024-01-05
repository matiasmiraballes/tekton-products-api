using Infrastructure.Persistence.DataProvider.DataClient;
using Infrastructure.Persistence.DataProvider.DbModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace TektonProductsApi.IntegrationTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        private static object lockObj = new object();
        private static bool dataSeeded = false;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Seed mock data into the in-memory database.
                lock (lockObj)
                {
                    if (!dataSeeded)
                    {
                        SeedMockData(services);
                        dataSeeded = true;
                    }
                }
            });
        }

        private static void SeedMockData(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var context = scopedServices.GetRequiredService<InMemoryDbContext>();

                // Ensure the database is created.
                context.Database.EnsureCreated();

                // Seed mock user data.
                context.Products.AddRange(
                    new InMemoryProduct
                    {
                        ProductId = new Guid("b1e76df0-11db-4402-a1b0-cb5e3b88bf0a"),
                        Name = "Seeded Product 1",
                        Description = "Seeded Product 1 Description",
                        Price = 10,
                        Status = 1,
                        Stock = 15
                    });

                context.SaveChanges();
            }
        }
    }
}
