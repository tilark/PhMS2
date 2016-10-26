using ClassViewModelToDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PHMS2.Models.ViewModels.InPatientReporter
{
    public class DepartmentAntibioticIntensity
    {
        public List<DepartmentAntibioticIntensityDomain> DepartmentAntibioticIntensityList;
        public string Hospital
        {
            get
            {
                return "全院";
            }
        }
        public Decimal TotalAntibioticDdd
        {
            get
            {
                return this.DepartmentAntibioticIntensityList.Sum(d => d.AntibioticDdd);
            }
        }
        public int TotalPersonNumberDays
        {
            get
            {
                return this.DepartmentAntibioticIntensityList.Sum(d => d.PersonNumberDays);
            }
        }
        public decimal TotalIntensityRate
        {
            get
            {
                return this.DepartmentAntibioticIntensityList.Sum(d => d.IntensityRate);
            }
        }
    }
}