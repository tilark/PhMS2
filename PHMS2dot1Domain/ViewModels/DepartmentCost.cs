using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhMS2dot1Domain.ViewModels
{
    public class DepartmentCost
    {
        public int DepartmentID { get; set; }
        public int DrugCJID { get; set; }
        public Decimal Cost { get; set; }
    }
}
