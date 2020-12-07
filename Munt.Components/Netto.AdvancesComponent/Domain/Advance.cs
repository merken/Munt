using System;

namespace Netto.AdvancesComponent.Domain
{
    public class Advance
    {
        public int EmployeeId { get; set; }
        public double Amount { get; set; }
        public DateTimeOffset PaidOn { get; set; }
    }
}