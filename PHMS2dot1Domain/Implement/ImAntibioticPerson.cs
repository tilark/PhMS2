using ClassViewModelToDomain.Interface;
using PhMS2dot1Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhMS2dot1Domain.Implement
{
    public class ImAntibioticPerson : IAntibioticPerson
    {
        private readonly PhMS2dot1DomainContext context;

        public ImAntibioticPerson(PhMS2dot1DomainContext context)
        {
            this.context = context;
        }
        public int GetAntibioticPerson(DateTime startTime, DateTime endTime)
        {
            throw new NotImplementedException();
        }
    }
}
