using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sarepta_WebApplication1.Models
{
    public class Patient_Payment
    {
        public Patients patient { get; set; }       
        public Processes process { get; set; }
        public Treatments treatment { get; set; }
        public Payments payment { get; set; }

        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string treatment_Name { get; set; }
    }
}
