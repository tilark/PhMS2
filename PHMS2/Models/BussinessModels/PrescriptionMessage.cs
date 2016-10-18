using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PHMS2.Models.BussinessModels
{
    public class PrescriptionMessage
    {
        public int InjectAntibioticPerson { get; set; }
        public int UseDrugPerson { get; set; }
        public int AntibioticCategoryNumber { get; set; }
        public decimal AntibioticCost { get; set; }
        public decimal DrugCost { get; set; }
    }
}