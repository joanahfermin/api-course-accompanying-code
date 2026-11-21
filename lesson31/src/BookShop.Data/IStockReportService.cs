namespace BookShop.Data;

public interface IStockReportService
{
    Task<List<CategoryStock>> GetByCategoryAsync();
}
