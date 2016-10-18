using PHMS2Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PHMS2Domain.Implement
{
    public class GetCountFromRegisterList
    {
        public int GetAntibioticPersonCount(List<Registers> registers, DateTime startTime, DateTime endTime)
        {
            return registers.Sum(reg => reg.AntibioticPerson(startTime, endTime));
        }

       public int GetRegisterPersonCount(List<Registers> registers, DateTime startTime, DateTime endTime)
        {
            return registers.Sum(reg => reg.RegisterPerson(startTime, endTime));
        }
        public int GetAntibioticCategoryNumberCount(List<Registers> registers, DateTime startTime, DateTime endTime)
        {
            return registers.SelectMany(r => r.AntibioticCategoryNumberList(startTime, endTime)).Distinct().ToList().Count;
        }
    }
}