using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Munt.Contract
{
    public class PerformanceInformation
    {
        /// <summary>
        /// The bruto base salary of the employee.
        /// </summary>
        public double BaseSalary { get; set; }

        /// <summary>
        /// The number of days that the employee is supposed to work.
        /// Number of working days in the month.
        /// </summary>
        public double PerformanceBaseline { get; set; }

        /// <summary>
        /// The amount of hours/day that the employee is supposed to work, according to the agreement.
        /// </summary>
        public double ContractualHoursPerDay { get; set; }

        /// <summary>
        /// The performances delivered by the employee.
        /// </summary>
        public List<Performance> Performances { get; set; }
    }
}
