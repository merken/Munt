using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Munt.Contract;
using Prise.Plugin;
using Netto.AdvancesComponent.Domain;
using System.Linq;

namespace Netto.AdvancesComponent
{
    [Plugin(PluginType = typeof(IMuntCalculationComponent))]
    public class AdvancesComponent : IMuntCalculationComponent
    {
        [PluginService(ServiceType = typeof(IAdvancesService), ProvidedBy = ProvidedBy.Plugin,
            ProxyType = typeof(IAdvancesService))]
        private readonly IAdvancesService advancesService;
        // TODO safety check did you mean PluginService?

        public async Task<List<CalculationResult>> Calculate(MuntContext context, ComponentContext componentContext)
        {
            var calculations = new List<CalculationResult>();

            var advances = await this.advancesService.GetAdvancesForEmployeeId(context.EmployeeInformation.Id);
            var advancesAlreadyPaidInThisPeriod = advances.Where(a =>
                context.CalculationInformation.StartDate <= a.PaidOn &&
                a.PaidOn <= context.CalculationInformation.EndDate);

            if (!advancesAlreadyPaidInThisPeriod.Any())
                return calculations; // Nothing to do, short circuit here

            foreach (var advance in advancesAlreadyPaidInThisPeriod.OrderBy(a => a.PaidOn))
                calculations.Add(CalculationResult.New(componentContext.CalculationAreaOrder, componentContext.Order,
                    "Advances", $"Voorschot {advance.PaidOn.DateTime.ToShortDateString()}", value: -(advance.Amount)));

            return calculations;
        }
    }
}