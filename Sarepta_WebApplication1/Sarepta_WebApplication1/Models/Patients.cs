using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sarepta_WebApplication1.Models
{
    public partial class Patients
    {
        public int PatientId { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "EL nombre es obligatorio")]
        public string Name { get; set; }

        
        public string CellphoneNumber { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Birthdate { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Debe elegir el genero")]
        public string Gender { get; set; }        

        [StringLength(50)]
        public string Address { get; set; }

        public DateTime CreatedAt { get; set; }

        [StringLength(20)]
        public string city { get; set; }

        [StringLength(12)]
        public string cedula { get; set; }
    }

    public enum Gender
    {
        M,
        F
    }  
}
