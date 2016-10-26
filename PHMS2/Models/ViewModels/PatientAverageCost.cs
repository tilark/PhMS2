using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PHMS2.Models.ViewModels
{
    public class PatientAverageCost
    {
        public PatientAverageCost()
        {
            this.PatientCost = 0M;
            this.RegisterPerson = 0;
        }
        public Decimal PatientCost { get; set; }
        public int RegisterPerson { get; set; }
        public decimal AverageCost { get {
            return this.RegisterPerson > 0 ? Decimal.Round(
               this.PatientCost / (Decimal)this.RegisterPerson, 2) : 0;
            } }
    }
}