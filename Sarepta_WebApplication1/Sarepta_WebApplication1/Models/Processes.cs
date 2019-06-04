using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sarepta_WebApplication1.Models
{
    public partial class Processes
    {
        public int ProcessId { get; set; }

        public int PatientId { get; set; }

        public int TreatmentsId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar si el proceso es a Domicilio o no")]
        public byte Home { get; set; }

        [StringLength(10)]
        public string Tooth { get; set; }

        [Required(ErrorMessage = "El valor no puede quedar vacio, si no hubo gasto digite '0'")]
        public int Laboratory { get; set; }

        [Required(ErrorMessage = "El valor no puede quedar vacio, si no hubo gasto digite '0'")]
        public int Consultory { get; set; }

        [Required(ErrorMessage = "El valor no puede quedar vacio, si no hubo gasto digite '0'")]
        public int Assistant { get; set; }

        [Required(ErrorMessage = "El valor no puede quedar vacio, si no hubo gasto digite '0'")]
        public int Materials { get; set; }

        [Required(ErrorMessage = "El valor no puede quedar vacio, si no hubo gasto digite '0'")]
        public int Transport { get; set; }

        [Required(ErrorMessage = "Debe Ingresar una Fecha")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }
    public enum Home
    {
        si,
        no
    }
}
