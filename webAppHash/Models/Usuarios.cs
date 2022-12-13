using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace webAppHash.Models
{
    public class Usuarios
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [DisplayName("Usuario")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre de usuario es un dato requerido")]
        [RegularExpression(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ]+$", ErrorMessage = "El nombre ingresado no es valido")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "El nombre debe contener entre 2 y 30 caracteres")]
        public string Name { get; set; } = string.Empty;

        [DisplayName("Clave")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El Password es un dato requerido")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "El password debe contener al menos 6 caracteres")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [HiddenInput(DisplayValue = false)]
        public string Salt { get; set; } = string.Empty;
    }
}
