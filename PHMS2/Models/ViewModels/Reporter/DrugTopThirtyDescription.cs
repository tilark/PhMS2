using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PHMS2.Models.ViewModels.Reporter
{
    public class DrugTopThirtyDescription
    {
        public int ProductCJID { get; set; }

        public string ProductNumber { get; set; }
        public string ProductName { get; set; }
        public Decimal Cost { get; set; }
        public bool IsAntibiotic { get; set; }

        public int DoctorID { get; set; }
        public string Doctor { get; set; }
        public int DepartmentID { get; set; }
        public string Department { get; set; }

    }
}