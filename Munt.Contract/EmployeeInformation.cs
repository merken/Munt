using System;
using System.Threading.Tasks;

namespace Munt.Contract
{
    public class EmployeeInformation
    {
       public int Id { get; set; }
       public string FirstName { get; set; }
       public string LastName { get; set; }
       public string Email { get; set; }
       public int NumberOfChildrenOnCharge { get; set; }
       public int NumberOfChildrenOnChargeWithHandicap { get; set; }
       public EmployeeType EmployeeType { get; set; }
    }
}
