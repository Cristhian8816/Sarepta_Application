﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace Sarepta_WebApplication1.Models
{
    public class Patient_Treatment
    {
        public Patients patient { get; set; }
        public Treatments treatment { get; set; }
        public Processes process { get; set; }
    }
}
