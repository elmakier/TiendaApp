using Microsoft.AspNetCore.Mvc;

namespace TiendaApp.Interfaces
{
    public interface IReportService
    {
        Task<FileContentResult> GenerarExcelAsync();
        Task<FileContentResult> GenerarPdfAsync();
    }
}