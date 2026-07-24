using System.ComponentModel.DataAnnotations;

namespace TiendaApp.ViewModels;

public class ProductoCreateViewModel
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, MinimumLength = 3,
        ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [Range(0.01, 10000,
        ErrorMessage = "El precio debe ser mayor a cero.")]
    public decimal Precio { get; set; }

    [Required]
    [Range(0, 100000,
        ErrorMessage = "El stock no puede ser negativo.")]
    public int Stock { get; set; }

    [Required(ErrorMessage = "La fecha es obligatoria.")]
    [DataType(DataType.Date)]
    public DateTime FechaVencimiento { get; set; }
}