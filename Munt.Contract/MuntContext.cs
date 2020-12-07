using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Munt.Contract
{
    public class CalculationInformation
    {
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
    }

    /// <summary>
    /// This class represents the context for the salary calculation
    /// </summary>
    public class MuntContext
    {
        public EmployeeInformation EmployeeInformation { get; set; }

        public EmployerInformation EmployerInformation { get; set; }

        public PerformanceInformation PerformanceInformation { get; set; }

        public List<CalculationResult> CalculationResults { get; set; }

        public CalculationInformation CalculationInformation { get; set; }
    }
}
