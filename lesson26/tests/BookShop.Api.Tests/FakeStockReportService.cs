using BookShop.Data;

namespace BookShop.Api.Tests;

public class FakeStockReportService : IStockReportService
{
    public List<CategoryStock> Report { get; } = new List<CategoryStock>();

    public Task<List<CategoryStock>> GetByCategoryAsync()
    {
        return Task.FromResult(Report);
    }
}
