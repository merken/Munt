using System.Collections.Generic;
using Munt.Contract;

namespace Munt.Functions.Models
{
    public class CalculationComponent
    {
        public int Order { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Version { get; set; }
    }

    public class CalculationArea
    {
        public int Order { get; set; }
        public string Description { get; set; }
        public CalculationComponent[] Components { get; set; }
    }

    public class Journey
    {
        public string Description { get; set; }
        public CalculationArea[] Areas { get; set; }
        public string[] BreadCrumbs { get; set; }
    }

    public class JourneyMessage
    {
        public double AmountForCalculationArea { get; set; }
        public List<CalculationResult> CalculationResults { get; set; }
        public MuntContext Context { get; set; }
        public Journey Journey { get; set; }
    }
}