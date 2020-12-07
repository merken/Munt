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
    public class MuntCalculationComponent : IMuntCalculationComponent
    {
        [PluginService(ServiceType = typeof(IAdvancesService), ProvidedBy = ProvidedBy.Host, ProxyType = typeof(IAdvancesService))]
        private readonly IAdvancesService advancesService;

        public async Task<List<CalculationResult>> Calculate(MuntContext context, CalculationContext calculationContext)
        {
            var calculations = new List<CalculationResult>();

            var advances = await this.advancesService.GetAdvancesForEmployeeId(context.EmployeeInformation.Id);
            var advancesAlreadyPaidInThisPeriod = advances.Where(a => context.CalculationInformation.StartDate <= a.PaidOn && a.PaidOn <= context.CalculationInformation.EndDate);

            if (!advancesAlreadyPaidInThisPeriod.Any())
                return calculations; // Nothing to do, short circuit here

            foreach (var advance in advancesAlreadyPaidInThisPeriod.OrderBy(a => a.PaidOn))
                calculations.Add(new CalculationResult("Advances", $"Voorschot {advance.PaidOn.DateTime.ToShortDateString()}", value: -(advance.Amount)));

            return calculations;
        }
    }
}

