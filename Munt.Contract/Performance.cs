using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Munt.Contract
{
    /// <summary>
    /// A performance represents a service from an employee.
    /// If he worked 1 full day, the performance will be :
    /// -Code 001
    /// -Description Work
    /// -Days 1
    /// -Hours 8
    /// -Amount 1
    /// -Value 12.00d (Euro/hour)
    /// -Type BusinessDay
    /// 
    /// For a fixed performance (compensation for a business travel or a compensation for working out of office)
    /// -Code 010
    /// -Description Compensation
    /// -Days 1
    /// -Hours 8
    /// -Amount 1
    /// -Value 14.00 (per day)
    /// -Type BusinessTravel or BusinessDayOnSite 
    /// </summary>
    public class Performance
    {
        /// <summary>
        /// The internal performance code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Human readable description
        /// </summary>
       public  string Description { get; set; }

        /// <summary>
        /// Amount of days for the performance
        /// </summary>
        public double Days { get; set; }

        /// <summary>
        /// Amount of hours for the performance
        /// </summary>
        public double Hours { get; set; }

        /// <summary>
        /// The amount of the performance
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        /// This can be the wage or the value of the performance in case of a fixed value.
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// The type of performance
        /// </summary>
        public PerformanceType Type { get; set; }
    }
}
