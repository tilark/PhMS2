﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PHMS2.Models.ViewModels.InPatientReporter
{
    public class InPatientAverageAntibioticCostRate
    {
        public InPatientAverageAntibioticCostRate()
        {
            this.TotalAntibioticCost = 0;
            this.TotalAntibioticPerson = 0;
        }
        public virtual Decimal TotalAntibioticCost { get; set; }
        public virtual int TotalAntibioticPerson { get; set; }

        public Decimal Rate
        {
            get
            {
                return this.TotalAntibioticPerson != 0 ? Decimal.Round((Decimal)this.TotalAntibioticCost  / (Decimal) this.TotalAntibioticPerson, 2) : 0;
            }
        }
    }
}