using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Munt.Advances.Api.Data;

namespace Munt.Advances.Api.Controllers
{   
    [ApiController]
    [Route("[controller]")]
    public class AdvancesController : ControllerBase
    {
        private readonly ILogger<AdvancesController> logger;
        private readonly AdvancesDataStore dataStore;

        public AdvancesController(ILogger<AdvancesController> logger, AdvancesDataStore dataStore)
        {
            this.logger = logger;
            this.dataStore = dataStore;
        }

        [HttpGet("{employeeId}")]
        public IEnumerable<Advance> Get(int employeeId)
        {
            return this.dataStore.Get(employeeId);
        }

        [HttpPost]
        public ActionResult Add([FromBody] Advance advance)
        {
            this.dataStore.Add(advance);
            return Ok();
        }
    }
}
