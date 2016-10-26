using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PHMS2.Models.ViewModels.InPatientReporter
{
    public class InPatientAverageAntibioticCategoryRate
    {
        public InPatientAverageAntibioticCategoryRate()
        {
            this.TotalAntibioticCategoryNumber = 0;
            this.TotalAntibioticPerson = 0;
        }
        public virtual int TotalAntibioticCategoryNumber { get; set; }
        public virtual int TotalAntibioticPerson { get; set; }

        public Decimal Rate
        {
            get
            {
                return this.TotalAntibioticPerson != 0 ? Decimal.Round(this.TotalAntibioticCategoryNumber * 100 / this.TotalAntibioticPerson, 2) : 0;
            }
        }
    }
}