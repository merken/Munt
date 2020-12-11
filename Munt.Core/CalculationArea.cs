namespace Munt.Core
{
    public class CalculationArea
    {
        public int Order { get; set; }
        public string Description { get; set; }
        public CalculationComponent[] Components { get; set; }
    }
}