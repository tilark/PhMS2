using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PHMS2.Models.BussinessModels
{
    public class DrugDoctorRank
    {
        public string DoctorName { get; set; }

        public string DepartmentName { get; set; }
        public bool IsAntibiotic { get; set; }

        public decimal Cost { get; set; }
    }
}