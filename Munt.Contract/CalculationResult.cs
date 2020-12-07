using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Munt.Contract
{
    public class CalculationResult
    {
        public CalculationResult(string code, string description, double days = 0.0, double hours = 0.0, double amount = 0.0, double value = 0.0)
        {
            this.Code = code;
            this.Description = description;
            this.Days = days;
            this.Hours = hours;
            this.Amount = amount;
            this.Value = value;
        }
        
        public string Code { get; set; }

        public string Description { get; set; }

        public double Days { get; set; }

        public double Hours { get; set; }

        public double Amount { get; set; }

        public double Value { get; set; }
    }
}
