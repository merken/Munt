using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Munt.Contract
{
    public class ComponentContext
    {
        /// <summary>
        /// The order of the Calculation Area 
        /// </summary>
        public int CalculationAreaOrder { get; set;}        

        /// <summary>
        /// The order of the Calculation Component 
        /// </summary>
        public int Order { get; set;}    
        
        /// <summary>
        /// Each calculation belongs to a Calculation Area (group of calculations), the AmountForCalculationArea contains the starting value of the area 
        /// </summary>
        public double AmountForCalculationArea { get; set; }
        
        /// <summary>
        /// </summary>
        public double IntermediateResult { get; set; }
        
        /// <summary>
        /// This list contains all of the previous calculation results
        /// </summary>
        public List<CalculationResult> CalculationResults { get; set; }
    }
}
