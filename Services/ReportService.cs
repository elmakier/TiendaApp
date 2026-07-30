using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using TiendaApp.Data;
using TiendaApp.Interfaces;

namespace TiendaApp.Services;

public class ReportService : IReportService
{
    private readonly ApplicationDbContext _context;

    public ReportService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<FileContentResult> GenerarExcelAsync()
    {
        using var workbook = new XLWorkbook();
        var ws = workbook.Worksheets.Add("Productos");

        ws.Cell(1, 1).Value = "Nombre";
        ws.Cell(1, 2).Value = "Precio";
        ws.Cell(1, 3).Value = "Stock";

        var productos = await _context.Productos
            .AsNoTracking()
            .ToListAsync();

        int fila = 2;

        foreach (var p in productos)
        {
            ws.Cell(fila, 1).Value = p.Nombre;
            ws.Cell(fila, 2).Value = p.Precio;
            ws.Cell(fila, 3).Value = p.Stock;
            fila++;
        }

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);

        return new FileContentResult(
            stream.ToArray(),
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
        {
            FileDownloadName = "Productos.xlsx"
        };
    }

    public async Task<FileContentResult> GenerarPdfAsync()
    {
        QuestPDF.Settings.License = LicenseType.Community;

        var productos = await _context.Productos
            .AsNoTracking()
            .ToListAsync();

        var pdf = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(20);

                page.Header()
                    .Text("Reporte de Productos")
                    .FontSize(20)
                    .Bold();

                page.Content().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                    });

                    table.Header(header =>
                    {
                        header.Cell().Text("Nombre").Bold();
                        header.Cell().Text("Precio").Bold();
                        header.Cell().Text("Stock").Bold();
                    });

                    foreach (var p in productos)
                    {
                        table.Cell().Text(p.Nombre);
                        table.Cell().Text(p.Precio.ToString());
                        table.Cell().Text(p.Stock.ToString());
                    }
                });
            });
        }).GeneratePdf();

        return new FileContentResult(pdf, "application/pdf")
        {
            FileDownloadName = "Productos.pdf"
        };
    }
}