using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Munt.Contract
{
    /// <summary>
    /// This class represents the context for the salary calculation
    /// </summary>
    public class MuntContext
    {
        EmployeeInformation EmployeeInformation { get; set; }

        EmployerInformation EmployerInformation { get; set; }

        PerformanceInformation PerformanceInformation { get; set; }

        List<CalculationResult> CalculationResults { get; set; }

        DateTime? Date { get; set; }
    }
}
