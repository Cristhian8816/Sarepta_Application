using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Sarepta_WebApplication1.Models
{
    public class userAccount
    {
        [Key]
        public int UserID { get; set; }

        [Required(ErrorMessage = "El Nombre es olbigatorio.")]
        public string  FirstName { get; set; }

        [Required(ErrorMessage = "El Apellido es olbigatorio.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "El e-mail es olbigatorio.")]
        [EmailAddress(ErrorMessage = "The Email field is not a valid e-mail address.")]
        public string email { get; set; }

        [Required(ErrorMessage = "Nombre de Usuario es requerido")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "La clave es obligatoria")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Las claves no coinciden")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

    }
}
