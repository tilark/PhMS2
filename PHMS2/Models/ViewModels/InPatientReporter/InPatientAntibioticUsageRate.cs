using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PHMS2.Models.ViewModels.InPatientReporter
{
    
    public class InPatientAntibioticUsageRate
    {
        public InPatientAntibioticUsageRate()
        {
            this.TotalAntibioticCost = 0;
            this.TotalDrugCost = 0;
        }
        public virtual Decimal TotalAntibioticCost { get; set; }
        public virtual Decimal TotalDrugCost { get; set; }

        public Decimal Rate
        {
            get
            {
                return this.TotalDrugCost != 0 ? Decimal.Round(this.TotalAntibioticCost * 100 / this.TotalDrugCost, 2) : 0;
            }
        }
    }
}