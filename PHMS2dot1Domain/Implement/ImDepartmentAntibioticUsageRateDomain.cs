using ClassViewModelToDomain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassViewModelToDomain;
using PhMS2dot1Domain.Factories;

namespace PhMS2dot1Domain.Implement
{
    public class ImDepartmentAntibioticUsageRateDomain : IDepartmentAntibioticUsageRateDomain
    {
        private readonly IDomain2dot1InnerFactory innerFactory;

        public ImDepartmentAntibioticUsageRateDomain(IDomain2dot1InnerFactory factory)
        {
            this.innerFactory = factory;
        }
        public List<DepartmentAntibioticUsageRateDomain> GetDepartmentAntibioticUsageRateDomain(DateTime startTime, DateTime endTime)
        {
            var result = new List<DepartmentAntibioticUsageRateDomain>();
            var inPatientList = this.innerFactory.CreateInPatientFromDrugRecords().GetInPatientFromDrugRecords(startTime, endTime);

            return result;
        }
    }
}
