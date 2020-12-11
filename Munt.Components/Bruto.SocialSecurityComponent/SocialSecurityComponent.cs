using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Munt.Contract;
using Prise.Plugin;

namespace Bruto.SocialSecurityComponent
{
    [Plugin(PluginType = typeof(IMuntCalculationComponent))]
    public class SocialSecurityComponent : IMuntCalculationComponent
    {
        public Task<List<CalculationResult>> Calculate(MuntContext context, ComponentContext componentContext)
        {
            var calculations = new List<CalculationResult>();
            var bruto = componentContext.AmountForCalculationArea;

            var contributionPct = 1d;

            if (context.EmployeeInformation.EmployeeType == EmployeeType.BlueCollar)
                contributionPct = 1.08d;

            bruto = bruto * contributionPct;

            var contribution = -(bruto * 13.07 / 100);

            calculations.Add(CalculationResult.New(componentContext.CalculationAreaOrder, componentContext.Order, "RSZ",
                "RSZ bijdrage", value: contribution));

            return Task.FromResult(calculations);
        }
    }
}