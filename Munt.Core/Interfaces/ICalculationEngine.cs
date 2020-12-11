using System.Collections.Generic;
using System.Threading.Tasks;
using Munt.Contract;

namespace Munt.Core
{
    public interface ICalculationEngine
    {
        Task<List<CalculationResult>> Calculate(MuntContext context);
    }
}