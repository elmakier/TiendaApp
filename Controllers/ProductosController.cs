using Microsoft.AspNetCore.Mvc;
using TiendaApp.Interfaces;
using TiendaApp.Models;

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
    public async Task<IActionResult> Create(Producto producto)
    {
        if (ModelState.IsValid)
        {
            await _repository.AddAsync(producto);
            await _repository.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        return View(producto);
    }
}