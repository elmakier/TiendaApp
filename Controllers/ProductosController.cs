using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaApp.Data;
using TiendaApp.Models;

namespace TiendaApp.Controllers;

public class ProductosController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProductosController(ApplicationDbContext context)
    {
        _context = context;
    }

    // LISTAR PRODUCTOS
    public async Task<IActionResult> Index()
    {
        var productos = await _context.Productos
            .AsNoTracking()
            .ToListAsync();

        return View(productos);
    }

    // MOSTRAR FORMULARIO
    public IActionResult Create()
    {
        return View();
    }

    // GUARDAR PRODUCTO
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Producto producto)
    {
        if (ModelState.IsValid)
        {
            // Convierte la fecha a UTC para PostgreSQL
            producto.FechaVencimiento = DateTime.SpecifyKind(
                producto.FechaVencimiento,
                DateTimeKind.Utc);

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        return View(producto);
    }
    [Route("Promociones-del-mes")]
    public IActionResult Promociones()
    {
        return View();
        
    }
}