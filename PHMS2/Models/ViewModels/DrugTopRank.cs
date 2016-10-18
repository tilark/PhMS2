using System;
using System.Collections.Generic;
using PHMS2.Models.BussinessModels;
using PHMS2Domain;
using ClassViewModelToDomain;

namespace PHMS2.Models.ViewModel
{
    public class DrugTopRank
    {
       
        public string ProductNumber { get; set; }
        public string ProductName { get; set; }
        public Decimal Cost { get; set; }
        public bool IsAntibiotic { get; set; }
        public List<DrugDoctorDepartmentCost> DrugDoctorDepartmentCostList { get; set; }

    }
}