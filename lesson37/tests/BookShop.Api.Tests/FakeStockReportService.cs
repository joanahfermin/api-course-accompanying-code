using BookShop.Data;

namespace BookShop.Api.Tests;

public class FakeStockReportService : IStockReportService
{
    public List<CategoryStock> Report { get; } = [];

    public Task<List<CategoryStock>> GetByCategoryAsync()
    {
        return Task.FromResult(Report);
    }
}
