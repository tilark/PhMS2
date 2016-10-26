using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PHMS2.Models.ViewModels.InPatientReporter
{
    public class InPatientDrugMessage
    {
        public virtual int AntibioticCategoryNumber { get; set; }
        public virtual Decimal AntibioticCost { get; set; }
        public virtual Decimal TotalDrugCost { get; set; }

        public virtual int UnionAntibioticPerson { get; set; }
    }
}