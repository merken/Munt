using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Munt.Advances.Api.Data
{
    public class Advance
    {
        public int EmployeeId { get; set; }
        public double Amount { get; set; }
        public DateTimeOffset PaidOn { get; set; }
    }
}
