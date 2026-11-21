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
            .OrderBy(g => g.Key)
            .Select(g => new CategoryStock(
                g.Key,
                g.Count(),
                g.Sum(b => b.Stock),
                g.Sum(b => b.Price * b.Stock)))
            .ToListAsync();
    }
}
