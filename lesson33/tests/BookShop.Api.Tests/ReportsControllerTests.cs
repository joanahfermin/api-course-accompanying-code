using BookShop.Data;

namespace BookShop.Api.Tests;

[TestClass]
public class ReportsControllerTests
{
    private FakeStockReportService _fakeService = null!;
    private ReportsController _controller = null!;

    [TestInitialize]
    public void Setup()
    {
        _fakeService = new FakeStockReportService();
        _controller = new ReportsController(_fakeService);
    }

    [TestMethod]
    public async Task GetStock_TwoCategories_ReturnsBothLines()
    {
        _fakeService.Report.Add(new CategoryStock("Cooking", 2, 6, 181.00m));
        _fakeService.Report.Add(new CategoryStock("Programming", 3, 22, 852.30m));

        var result = await _controller.GetStock();

        Assert.IsNotNull(result.Value);
        Assert.HasCount(2, result.Value);
        Assert.AreEqual("Cooking", result.Value[0].Category);
        Assert.AreEqual(852.30m, result.Value[1].StockValue);
    }

    [TestMethod]
    public async Task GetStock_NoBooks_ReturnsAnEmptyList()
    {
        var result = await _controller.GetStock();

        Assert.IsNotNull(result.Value);
        Assert.IsEmpty(result.Value);
    }
}
