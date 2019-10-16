using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Sarepta_WebApplication1.Models
{
    public partial class Treatments
    {
        
        public int TreatmentId { get; set; }

        [Required(ErrorMessage = "Debe Seleccionar un Tratamiento")]
        public string Name { get; set; }
       
        public int Price { get; set; }
    }  
    
}
