using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Munt.Contract;
using Prise.Plugin;

namespace Bruto.CompensationNaturaCompanyCarComponent
{
    [Plugin(PluginType = typeof(IMuntCalculationComponent))]
    public class MuntCalculationComponent : IMuntCalculationComponent
    {
        public Task<List<CalculationResult>> Calculate(MuntContext context, CalculationContext calculationContext)
        {
            var calculations = new List<CalculationResult>();
            var bruto = calculationContext.AmountForCalculationArea;

            var contribution = 0.0d;

            if (context.EmployeeInformation.FirstName == "Jan")
            {
                contribution = 76.21d;
            }

            if (context.EmployeeInformation.FirstName == "Peter")
            {
                contribution = 175.22d;
            }

            calculations.Add(new CalculationResult("CompensationNaturaCompanyCar", "Voordeel natura wagen", amount: 1, value: contribution));

            return Task.FromResult(calculations);
        }
    }
}
