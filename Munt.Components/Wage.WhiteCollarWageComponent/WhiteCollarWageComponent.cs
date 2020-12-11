using System.Collections.Generic;
using System.Threading.Tasks;
using Munt.Contract;
using Prise.Plugin;
using System.Linq;

namespace Wage.WhiteCollarWageComponent
{
    [Plugin(PluginType = typeof(IMuntCalculationComponent))]
    public class WhiteCollarWageComponent : IMuntCalculationComponent
    {
        public async Task<List<CalculationResult>> Calculate(MuntContext context, ComponentContext componentContext)
        {
            var calculations = new List<CalculationResult>();

            //The bruto base salary for the employee
            var brutoSalary = context.PerformanceInformation.BaseSalary;
            //The number of days that the employee is supposed to work 
            var performanceBaseline = context.PerformanceInformation.PerformanceBaseline;
            //The amount of hours/day that the employee is supposed to work 
            var contractualHoursPerDay = context.PerformanceInformation.ContractualHoursPerDay;
            //The hourly rate for the employee
            var hourlyRateForEmployee = (brutoSalary / performanceBaseline) / contractualHoursPerDay;

            var performanceCodes = context.PerformanceInformation.Performances
                .Where(p =>
                    p.Type == PerformanceType.BusinessDay ||
                    p.Type == PerformanceType.Illness ||
                    p.Type == PerformanceType.WorkingHoliday ||
                    p.Type == PerformanceType.Holiday)
                .Select(p => p.Code)
                .Distinct();

            foreach (var performanceCode in performanceCodes)
            {
                var performances = context.PerformanceInformation.Performances.Where(p => p.Code == performanceCode);
                var hours = performances.Sum(p => p.Hours);
                var description = performances.FirstOrDefault()?.Description;
                var days = performances.Sum(p => p.Days);

                var value = hourlyRateForEmployee * hours;

                //Add a calculation result for this area
                calculations.Add(CalculationResult.New(componentContext.CalculationAreaOrder, componentContext.Order,
                    performanceCode.ToString(), description,
                    days: days,
                    hours: hours,
                    value: value));
            }
            
            return calculations;
        }
    }
}