using System;
using System.Collections.Generic;

namespace Sarepta_WebApplication1.Models
{
    public partial class Payments
    {
        public int PaymentId { get; set; }
        public int ProcessId { get; set; }
        public int Pay { get; set; }
        public string PayType { get; set; }
        public string Status { get; set; }
        public int Discount { get; set; }
        public DateTime PayDate { get; set; }
        public byte Finished { get; set; }
    }

    public enum Paytype
    {
        Cash,
        Credit_Card,
        Debit_Card
    }
}
