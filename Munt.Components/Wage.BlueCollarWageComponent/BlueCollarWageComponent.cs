using System.Collections.Generic;
using System.Threading.Tasks;
using Munt.Contract;
using Prise.Plugin;
using System.Linq;

namespace Wage.BlueCollarWageComponent
{
    [Plugin(PluginType = typeof(IMuntCalculationComponent))]
    public class BlueCollarWageComponent : IMuntCalculationComponent
    {
        public async Task<List<CalculationResult>> Calculate(MuntContext context, ComponentContext componentContext)
        {
            var calculations = new List<CalculationResult>();
            
            var performanceCodes = context.PerformanceInformation.Performances.Select(p => p.Code).Distinct();
            foreach (var performanceCode in performanceCodes)
            {
                var performances = context.PerformanceInformation.Performances.Where(p => p.Code == performanceCode);

                var wage = performances.FirstOrDefault()?.Value;
                var description = performances.FirstOrDefault()?.Description;
                var hours = performances.Sum(p => p.Hours);
                var days = performances.Sum(p => p.Days);
                var value = hours * wage;

                //Add a calculation result for this area
                calculations.Add(CalculationResult.New(componentContext.CalculationAreaOrder, componentContext.Order,
                    performanceCode.ToString(), description,
                    days: days,
                    hours: hours,
                    value: value.HasValue ? value.Value : 0));
            }

            return calculations;
        }
    }
}