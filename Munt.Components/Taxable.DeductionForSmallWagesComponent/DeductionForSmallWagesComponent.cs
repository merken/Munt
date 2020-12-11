using System.Collections.Generic;
using System.Threading.Tasks;
using Munt.Contract;
using Prise.Plugin;
using System.Linq;

namespace Taxable.DeductionForSmallWagesComponent
{
    [Plugin(PluginType = typeof(IMuntCalculationComponent))]
    public class DeductionForSmallWagesComponent : IMuntCalculationComponent
    {
        public async Task<List<CalculationResult>> Calculate(MuntContext context, ComponentContext componentContext)
        {
            var calculations = new List<CalculationResult>();

            // If the person has a low income (small wage), there's an additional discount on the Witholding Tax (Bedrijfsvoorheffing)
            if (componentContext.AmountForCalculationArea <= 2291.80d)
            {
                calculations.Add(CalculationResult.New(componentContext.CalculationAreaOrder, componentContext.Order,
                    "DeductionForSmallWages", "Vermindering BVH van 6.46", value: 6.46d));
            }

            return calculations;
        }
    }
}