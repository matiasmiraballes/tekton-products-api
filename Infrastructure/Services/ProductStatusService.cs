using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Services
{
    public class ProductStatusService : IProductStatusService
    {
        private readonly IMemoryCache _memoryCache;

        public ProductStatusService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        public Dictionary<int, string> GetProductStatusDictionary()
        {
            if (!_memoryCache.TryGetValue("ProductStatusDictionary", out Dictionary<int, string> productStatusDictionary))
            {
                productStatusDictionary = new Dictionary<int, string>
                {
                    { 1, "Active" },
                    { 0, "Inactive" }
                };

                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                };

                _memoryCache.Set("ProductStatusDictionary", productStatusDictionary, cacheEntryOptions);
            }

            return productStatusDictionary!;
        }
    }
}
