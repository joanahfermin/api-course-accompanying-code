using BookShop.Data;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("reports")]
public class ReportsController : ControllerBase
{
    private readonly IStockReportService _reportService;

    public ReportsController(IStockReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("stock")]
    public async Task<ActionResult<List<CategoryStock>>> GetStock()
    {
        var report = await _reportService.GetByCategoryAsync();
        return report;
    }
}
