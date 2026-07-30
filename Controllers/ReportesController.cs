using Microsoft.AspNetCore.Mvc;
using TiendaApp.Interfaces;

namespace TiendaApp.Controllers;

public class ReportesController : Controller
{
    private readonly IReportService _reportService;

    public ReportesController(IReportService reportService)
    {
        _reportService = reportService;
    }

    public async Task<IActionResult> Excel()
    {
        return await _reportService.GenerarExcelAsync();
    }

    public async Task<IActionResult> Pdf()
    {
        return await _reportService.GenerarPdfAsync();
    }
}