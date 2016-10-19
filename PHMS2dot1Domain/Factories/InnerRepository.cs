using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhMS2dot1Domain.Interface;
using PhMS2dot1Domain.Implement;

namespace PhMS2dot1Domain.Factories
{
    public class InnerRepository : IInnerRepository
    {
        private readonly IDomain2dot1InnerFactory innerFactory;
        public InnerRepository(IDomain2dot1InnerFactory factory)
        {
            this.innerFactory = factory;
        }

        public IOutDepartmentPerson CreateOutDepartmentPerson()
        {
            return new ImOutDepartmentPerson(this.innerFactory);
        }
    }
}
