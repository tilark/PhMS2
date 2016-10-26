using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PHMS2.Models.ViewModels
{
    public class AntibioticUsageRate
    {
        public AntibioticUsageRate()
        {
            this.AntibioticPerson = 0;
            this.RegisterPerson = 0;
        }
        public int AntibioticPerson { get; set; }
        //public int CancelAntibioticPerson { get; set; }
        public int RegisterPerson { get; set; }
        public decimal UsageRate { get
            {
                return this.RegisterPerson != 0
                    ? Decimal.Round((Decimal)this.AntibioticPerson * 100 / (Decimal)this.RegisterPerson, 2)
                   : 0;
            } }

    }
}