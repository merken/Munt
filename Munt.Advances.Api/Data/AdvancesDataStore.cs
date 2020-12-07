using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Munt.Advances.Api.Data
{
    // Singleton in-memory data store (for testing)
    public class AdvancesDataStore
    {
        private readonly List<Advance> advances;

        public AdvancesDataStore()
        {
            this.advances = new List<Advance>();
        }

        public List<Advance> Get(int employeeId) => this.advances.Where(a => a.EmployeeId == employeeId).ToList();
        public void Add(Advance advance) => this.advances.Add(advance);
    }
}
