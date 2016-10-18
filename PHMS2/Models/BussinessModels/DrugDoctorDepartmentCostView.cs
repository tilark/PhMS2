using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PHMS2.Models.BussinessModels
{
    public class DrugDoctorDepartmentCostView
    {
        public string Doctor { get; set; }
        public string Department { get; set; }
        public Decimal Cost { get; set; }
    }
}