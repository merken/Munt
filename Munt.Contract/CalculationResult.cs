using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Munt.Contract
{
    public class CalculationResult
    {
        public string Code { get; set; }

        public string Description { get; set; }

        public double Days { get; set; }

        public double Hours { get; set; }

        public double Amount { get; set; }

        public double Value { get; set; }
    }
}
