namespace TiendaApp.ViewModels;
public record ProductoDto (
    int Id,
    string Nombre,
    decimal Precio,
    int Stock
);