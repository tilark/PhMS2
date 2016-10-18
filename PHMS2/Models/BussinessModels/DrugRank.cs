using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PHMS2.Models.BussinessModels
{
    public class DrugRank
    {
        public string ProductNumber { get; set; }

        public decimal Cost { get; set; }

        public List<DrugDoctorRank> DrugDoctorRanks { get; set; }


    }
}