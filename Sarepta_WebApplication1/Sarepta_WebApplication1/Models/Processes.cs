using System;
using System.Collections.Generic;

namespace Sarepta_WebApplication1.Models
{
    public partial class Processes
    {
        public int ProcessId { get; set; }
        public int PatientId { get; set; }
        public int TreatmentsId { get; set; }
        public byte Home { get; set; }
        public string Tooth { get; set; }
        public int Laboratory { get; set; }
        public int Consultory { get; set; }
        public int Assistant { get; set; }
        public int Materials { get; set; }
        public int Transport { get; set; }
        public DateTime Date { get; set; }
    }
}
