using PhMS2dot1Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhMS2dot1Domain.ImplementOuterRepository
{
    public class GetCountFromRegisterList
    {
        public int GetAntibioticPersonCount(List<OutPatient> registers, DateTime startTime, DateTime endTime)
        {
            return registers.Sum(reg => reg.AntibioticPerson(startTime, endTime));
        }

        public int GetRegisterPersonCount(List<OutPatient> registers, DateTime startTime, DateTime endTime)
        {
            return registers.Sum(reg => reg.RegisterPerson(startTime, endTime));
        }
        public int GetAntibioticCategoryNumberCount(List<OutPatient> registers, DateTime startTime, DateTime endTime)
        {
            return registers.SelectMany(r => r.AntibioticCategoryNumberList(startTime, endTime)).Distinct().ToList().Count;
        }
    }
}
