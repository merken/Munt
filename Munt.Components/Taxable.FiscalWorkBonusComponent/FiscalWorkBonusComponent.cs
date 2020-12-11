using System.Collections.Generic;
using System.Threading.Tasks;
using Munt.Contract;
using Prise.Plugin;
using System.Linq;

namespace Taxable.FiscalWorkBonusComponent
{
    [Plugin(PluginType = typeof(IMuntCalculationComponent))]
    public class FiscalWorkBonusComponent : IMuntCalculationComponent
    {
        public async Task<List<CalculationResult>> Calculate(MuntContext context, ComponentContext componentContext)
        {
            var calculations = new List<CalculationResult>();

            var bruto = componentContext.AmountForCalculationArea;

            var socialWorkBonus = componentContext.CalculationResults.FirstOrDefault(r => r.Code == "SocialWorkBonus");

            if (socialWorkBonus != null)
            {
                var fiscalWorkBonus = socialWorkBonus.Value * 0.1440;
                calculations.Add(CalculationResult.New(componentContext.CalculationAreaOrder, componentContext.Order,
                    "FiscalWorkBonus", "Fiscale werkbonus", value: fiscalWorkBonus));
            }

            return calculations;
        }
    }
}