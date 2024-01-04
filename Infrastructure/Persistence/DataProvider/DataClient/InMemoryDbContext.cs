using Infrastructure.Persistence.DataProvider.DbModels;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.DataProvider.DataClient
{
    public class InMemoryDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "InMemoryDb");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        internal DbSet<InMemoryProduct> Products { get; set; }
    }
}
