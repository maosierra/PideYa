using System.ComponentModel.DataAnnotations;

namespace PideYa.Server.Dtos;

public class RegisterRequest
{
    [Required(ErrorMessage = "El nombre es un campo obligatorio")]
    [StringLength(20, ErrorMessage = "Nombre debe ser menor a 20 y mayor a 3 caracteres", MinimumLength = 3)]
    public string FirstName { get; set; }
    public string LastName { get; set; }

    [Required(ErrorMessage = "El Email es un campo obligatorio")]
    [EmailAddress(ErrorMessage = "Ingrese un email valido")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Contrasena es requerida")]
    [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required(ErrorMessage = "Confirm Password is required")]
    [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
    [DataType(DataType.Password)]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }
}
