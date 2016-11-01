using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClassViewModelToDomain
{
    public class DrugDoctorDepartmentCost
    {
        public long DoctorID { get; set; }
        public string Doctor { get; set; }
        public int DepartmentID { get; set; }
        public string Department { get; set; }
        public Decimal Cost { get; set; }
    }
}