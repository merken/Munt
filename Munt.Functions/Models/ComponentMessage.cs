using System.Collections.Generic;
using Munt.Contract;
using Munt.Functions.Models;

namespace Munt.Functions
{
    public class ComponentMessage
    {
        public string ComponentType { get; set; }
        public string ComponentVersion { get; set; }
        public Journey Journey { get; set; }
        public MuntContext MuntContext { get; set; }
        public ComponentContext ComponentContext { get; set; }
    }
}