using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Data.Tests;

[TestClass]
public class StockReportServiceTests
{
    private SqliteConnection _connection = null!;
    private BookShopDbContext _context = null!;
    private StockReportService _service = null!;

    [TestInitialize]
    public void Setup()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<BookShopDbContext>()
            .UseSqlite(_connection)
            .Options;
        _context = new BookShopDbContext(options);
        _context.Database.EnsureCreated();

        _service = new StockReportService(_context);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Dispose();
        _connection.Dispose();
    }

    [TestMethod]
    public async Task GetByCategoryAsync_NoBooks_ReturnsEmptyList()
    {
        var report = await _service.GetByCategoryAsync();

        Assert.IsEmpty(report);
    }

    [TestMethod]
    public async Task GetByCategoryAsync_OneCategory_AddsUpTitlesCopiesAndValue()
    {
        _context.Books.Add(new Book { Title = "The Pragmatic Programmer", Price = 39.90m, Stock = 12, Category = "Programming" });
        _context.Books.Add(new Book { Title = "Clean Code", Price = 34.50m, Stock = 7, Category = "Programming" });
        _context.Books.Add(new Book { Title = "Refactoring", Price = 44.00m, Stock = 3, Category = "Programming" });
        await _context.SaveChangesAsync();

        var report = await _service.GetByCategoryAsync();

        Assert.HasCount(1, report);
        Assert.AreEqual("Programming", report[0].Category);
        Assert.AreEqual(3, report[0].Titles);
        Assert.AreEqual(22, report[0].Copies);
        Assert.AreEqual(852.30m, report[0].StockValue);
    }

    [TestMethod]
    public async Task GetByCategoryAsync_TwoCategories_ReturnsOneLineEachOrderedByCategory()
    {
        _context.Books.Add(new Book { Title = "Clean Code", Price = 34.50m, Stock = 7, Category = "Programming" });
        _context.Books.Add(new Book { Title = "Salt Fat Acid Heat", Price = 29.00m, Stock = 4, Category = "Cooking" });
        _context.Books.Add(new Book { Title = "The Joy of Cooking", Price = 32.50m, Stock = 2, Category = "Cooking" });
        await _context.SaveChangesAsync();

        var report = await _service.GetByCategoryAsync();

        Assert.HasCount(2, report);
        Assert.AreEqual("Cooking", report[0].Category);
        Assert.AreEqual(2, report[0].Titles);
        Assert.AreEqual(6, report[0].Copies);
        Assert.AreEqual(181.00m, report[0].StockValue);
        Assert.AreEqual("Programming", report[1].Category);
    }

    [TestMethod]
    public async Task GetByCategoryAsync_BookWithNoStock_StillCountsAsATitle()
    {
        _context.Books.Add(new Book { Title = "Clean Code", Price = 34.50m, Stock = 0, Category = "Programming" });
        await _context.SaveChangesAsync();

        var report = await _service.GetByCategoryAsync();

        Assert.AreEqual(1, report[0].Titles);
        Assert.AreEqual(0, report[0].Copies);
        Assert.AreEqual(0m, report[0].StockValue);
    }
}
