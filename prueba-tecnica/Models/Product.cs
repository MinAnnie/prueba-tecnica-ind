using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace prueba_tecnica.Models;

public class Product
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre del producto no puede exceder los 100 caracteres.")]
    public string Name { get; set; }
    
    [StringLength(100, ErrorMessage = "La descripción no puede exceder los 100 caracteres.")]
    public string Description { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "El stock debe ser un valor positivo.")]
    public int Stock { get; set; }
}

