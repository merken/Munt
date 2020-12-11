namespace Munt.Functions.Models
{
    public static class JourneyUtils
    {
        public static string ToBreadCrumb(CalculationComponent component) =>
            ToBreadCrumb(component.Type, component.Version);

        public static string ToBreadCrumb(string componentType, string componentVersion) =>
            $"{componentType},{componentVersion}";
    }
}