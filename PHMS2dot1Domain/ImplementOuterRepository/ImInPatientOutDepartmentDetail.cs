using ClassViewModelToDomain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassViewModelToDomain;
using PhMS2dot1Domain.Factories;

namespace PhMS2dot1Domain.ImplementOuterRepository
{
    public class ImInPatientOutDepartmentDetail : IInPatientOutDepartmentDetail
    {
        private readonly IDomain2dot1InnerFactory innerFactory;
        public ImInPatientOutDepartmentDetail(IDomain2dot1InnerFactory factory)
        {
            this.innerFactory = factory;
        }
        public List<InPatientOutDepartmentDomain> GetInPatientOutDepartmentDetail(DateTime startTime, DateTime endTime)
        {

            var result = this.innerFactory.CreateInPatientOutDepartmentPerson().GetInPatientOutDepartmentDetails(startTime, endTime);
            return result;
        }
    }
}
