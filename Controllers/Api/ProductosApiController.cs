using Microsoft.AspNetCore.Mvc;
using TiendaApp.Interfaces;
using TiendaApp.Models;
using TiendaApp.ViewModels;

namespace TiendaApp.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
public class ProductosApiController : ControllerBase
{
    private readonly IRepository<Producto> _repository;

    public ProductosApiController(IRepository<Producto> repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductoDto>>> GetProductos()
    {
        var productos = await _repository.GetAllAsync();

        var dtos = productos.Select(p =>
            new ProductoDto(
                p.Id,
                p.Nombre,
                p.Precio,
                p.Stock
            ));

        return Ok(dtos);
    }
}