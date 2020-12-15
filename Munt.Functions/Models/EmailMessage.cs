using System;
using Munt.Contract;
namespace Munt.Functions.Models
{
    public class EmailMessage
    {
        public string Employee { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string EmailAddress { get; set; }
        public CalculationResult[] CalculationResults { get; set; }
    }
}