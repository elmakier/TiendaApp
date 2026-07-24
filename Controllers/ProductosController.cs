using Microsoft.AspNetCore.Mvc;
using TiendaApp.Interfaces;
using TiendaApp.Models;
using TiendaApp.ViewModels;

namespace TiendaApp.Controllers;

public class ProductosController : Controller
{
    private readonly IRepository<Producto> _repository;

    public ProductosController(IRepository<Producto> repository)
    {
        _repository = repository;
    }

    // GET: Productos
    public async Task<IActionResult> Index()
    {
        var productos = await _repository.GetAllAsync();
        return View(productos);
    }

    // GET: Productos/Create
    public IActionResult Create()
    {
        return View();
    }

// POST: Productos/Create
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create(ProductoCreateViewModel vm)
{
    if (!ModelState.IsValid)
    {
        return View(vm);
    }

    // Mapeo del ViewModel a la entidad Producto
    var producto = new Producto
    {
        Nombre = vm.Nombre,
        Precio = vm.Precio,
        Stock = vm.Stock,
        FechaVencimiento = DateTime.SpecifyKind(
            vm.FechaVencimiento,
            DateTimeKind.Utc),
        CategoriaId = 1
    };

    await _repository.AddAsync(producto);
    await _repository.SaveChangesAsync();

    return RedirectToAction(nameof(Index));
}

}