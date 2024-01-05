namespace Infrastructure.Services
{
    public interface IProductStatusService
    {
        Dictionary<int, string> GetProductStatusDictionary();
    }
}
