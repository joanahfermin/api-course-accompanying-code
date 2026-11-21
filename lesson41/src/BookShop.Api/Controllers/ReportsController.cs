using BookShop.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Manager")]
[ApiController]
[Route("reports")]
public class ReportsController : ControllerBase
{
    private readonly IStockReportService _reportService;
    private readonly ILogger<ReportsController> _logger;

    public ReportsController(IStockReportService reportService,
        ILogger<ReportsController> logger)
    {
        _reportService = reportService;
        _logger = logger;
    }

    [HttpGet("stock")]
    public async Task<ActionResult<List<CategoryStock>>> GetStock()
    {
        _logger.LogInformation("Stock report viewed by {UserName}",
            User.FindFirst("name")?.Value);
        var report = await _reportService.GetByCategoryAsync();
        return report;
    }
}
