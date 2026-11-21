using Microsoft.EntityFrameworkCore;

namespace BookShop.Data;

public class StockReportService : IStockReportService
{
    private readonly BookShopDbContext _context;

    public StockReportService(BookShopDbContext context)
    {
        _context = context;
    }

    public async Task<List<CategoryStock>> GetByCategoryAsync()
    {
        return await _context.Books
            .GroupBy(b => b.Category)
            .Select(g => new CategoryStock
            {
                Category = g.Key,
                Titles = g.Count(),
                Copies = g.Sum(b => b.Stock),
                StockValue = g.Sum(b => b.Price * b.Stock)
            })
            .OrderBy(r => r.Category)
            .ToListAsync();
    }
}
