using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Munt.Contract
{
      /// <summary>
    /// This class represents a calculation on top of the remuneration.
    /// To create a new calculation, inherit this interface and provide an implementation for the Calculate method.
    /// The calculation engine will scan all implementations of this interface through reflection and execute them for each calculation area, in order.
    /// </summary>
    public interface IMuntCalculationComponent
    {
        /// <summary>
        /// This method will be called from the calculation engine, the context and calculation context will be injected.
        /// The implementation can access theses contexts in a read-only fashion.
        /// 
        /// </summary>
        /// <param name="context">The general context, this contains information regarding the employee, employer and the performances delivered by that employee for the employer.</param>
        /// <param name="componentContext">The context for the current calculation component.
        /// These areas can be :
        /// - Wage
        /// - Bruto
        /// - Taxable
        /// - Netto
        /// 
        /// In case the amount needs to be adjusted (addition, subtraction), the implementation is allowed to do so.
        /// </param>
        /// <returns>A calculation result, which translates in a line on the employee payslip.</returns>
        Task<List<CalculationResult>> Calculate(MuntContext context, ComponentContext componentContext);
    }
}
