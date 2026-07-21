using System.ComponentModel.DataAnnotations;

namespace TiendaApp.Models;

public class Categoria
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(50)]
    public string Nombre { get; set; } = string.Empty;

    // Una categoría puede tener muchos productos
    public virtual ICollection<Producto> Productos { get; set; } = new HashSet<Producto>();
}