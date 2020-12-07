
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Netto.AdvancesComponent.Domain
{
    public interface IAdvancesService
    {
        Task<List<Advance>> GetAdvancesForEmployeeId(int employeeId);
    }
}