using System;
using System.ComponentModel.DataAnnotations;

namespace PideYa.Shared.Entities
{
    public class Dish
    {
        public int id { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(20, ErrorMessage = "Nombre debe ser menor a 20 caracteres")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Valor debe ser positivo")]
        [Range(0.01, 999999999.99, ErrorMessage = "El valor debe ser mayor a 0 y menor que 999999999")]
        public float Price { get; set; }
        [Required(ErrorMessage = "Seleccione una categoria valida")]
        [Range(1, 999999, ErrorMessage = "Seleccione una categoria valida")]
        public int CategoryId { get; set; }
        public DishCategory? Category { get; set; }
        public List<DishImage>? Image { get; set; }
    }
}

