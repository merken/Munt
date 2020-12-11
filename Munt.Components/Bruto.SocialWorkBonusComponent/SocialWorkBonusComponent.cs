using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Munt.Contract;
using Prise.Plugin;

namespace Bruto.SocialWorkBonusComponent
{
    [Plugin(PluginType = typeof(IMuntCalculationComponent))]
    public class SocialWorkBonusComponent : IMuntCalculationComponent
    {
        public Task<List<CalculationResult>> Calculate(MuntContext context, ComponentContext componentContext)
        {
            var calculations = new List<CalculationResult>();
            var bruto = componentContext.AmountForCalculationArea;

            var bonus = 0.0d;

            if (bruto <= 1501.82d)
            {
                switch (context.EmployeeInformation.EmployeeType)
                {
                    case EmployeeType.WhiteCollar:
                        bonus = 183.97d;
                        break;
                    case EmployeeType.BlueCollar:
                        bonus = 198.69d;
                        break;
                }
            }
            else if (1501.82d <= bruto && bruto <= 2385.41d)
            {
                switch (context.EmployeeInformation.EmployeeType)
                {
                    case EmployeeType.WhiteCollar:
                        bonus = 183.97d - (0.2082 * (bruto - 1501.82d));
                        break;
                    case EmployeeType.BlueCollar:
                        bonus = 198.69d - (0.2249 * (bruto - 1501.82d));
                        break;
                }
            }
            else if (2385.41d < bruto)
            {
                bonus = 0.0d;
            }

            calculations.Add(CalculationResult.New(componentContext.CalculationAreaOrder, componentContext.Order,
                "SocialWorkBonus", "Sociale werkbonus", value: bonus));

            return Task.FromResult(calculations);
        }
    }
}