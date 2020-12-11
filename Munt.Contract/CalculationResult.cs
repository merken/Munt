using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Munt.Contract
{
    public class CalculationResult
    {
        public static CalculationResult New(int calculationArea, int calculationComponent, string code, string description, double days = 0.0, double hours = 0.0, double amount = 0.0, double value = 0.0)
        {
            return new CalculationResult
            {
                CalculationArea = calculationArea,
                CalculationComponent = calculationComponent,
                Code = code,
                Description = description,
                Days = days,
                Hours = hours,
                Amount = amount,
                Value = value
            };
        }

        public int CalculationArea { get; set; }
        public int CalculationComponent { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public double Days { get; set; }
        public double Hours { get; set; }
        public double Amount { get; set; }
        public double Value { get; set; }
    }
}
