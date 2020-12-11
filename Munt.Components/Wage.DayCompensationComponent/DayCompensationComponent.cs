using System.Collections.Generic;
using System.Threading.Tasks;
using Munt.Contract;
using Prise.Plugin;
using System.Linq;

namespace Wage.DayCompensationComponent
{
    [Plugin(PluginType = typeof(IMuntCalculationComponent))]
    public class DayCompensationComponent : IMuntCalculationComponent
    {
        public async Task<List<CalculationResult>> Calculate(MuntContext context, ComponentContext componentContext)
        {
            var calculations = new List<CalculationResult>();
            var dayCompensations =
                context.PerformanceInformation.Performances.Where(p => p.Type == PerformanceType.BusinessDayOnSite);

            //TODO group by value
            if (dayCompensations.Any())
            {
                var amountOfDays = dayCompensations.Count();
                var compensationRate = dayCompensations.FirstOrDefault().Value;
                var value = amountOfDays * compensationRate;

                calculations.Add(CalculationResult.New(componentContext.CalculationAreaOrder, componentContext.Order,
                    "DayCompensation", "Bruto Dagvergoeding", value: value));
            }

            return calculations;
        }
    }
}