using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Sarepta_WebApplication1.Models
{
    public partial class Treatments
    {
        public int TreatmentId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
    }    

    public enum Name
    {
        Seleccione,        
        Corona_MP,
        [Description("Protesis Parcial Superior Acrilico")]
        Protesis_Partial_superior_acrilico,
        [Description("Resina de Fotocurado")]
        Resina_de_Fotocurado,
        
    }

}
