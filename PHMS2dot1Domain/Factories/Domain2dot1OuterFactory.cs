using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassViewModelToDomain.Interface;
using PhMS2dot1Domain.Implement;

namespace PhMS2dot1Domain.Factories
{
    public class Domain2dot1OuterFactory : IDomain2dot1OuterFactory
    {
        private readonly IDomain2dot1InnerFactory innerFactory;
        public Domain2dot1OuterFactory(IDomain2dot1InnerFactory factory)
        {
            this.innerFactory = factory;
        }
        public IDepartmentAntibioticUsageRateDomain CreateDepartmentAntibioticUsageRateDomain()
        {
            return new ImDepartmentAntibioticUsageRateDomain(this.innerFactory);
        }
    }
}
