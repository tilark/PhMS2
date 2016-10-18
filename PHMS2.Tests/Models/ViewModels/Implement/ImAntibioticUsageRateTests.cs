using Microsoft.VisualStudio.TestTools.UnitTesting;
using PHMS2.Models.ViewModel.Implement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using PHMS2.Models.Factories;
using PHMS2Domain.Factory;
using PHMS2Domain.Interface;
using PHMS2Domain;
using ClassViewModelToDomain.Interface;

namespace PHMS2.Models.ViewModel.Implement.Tests
{
    [TestClass()]
    public class ImAntibioticUsageRateTests
    {

        [TestMethod()]
        public void ImAntibioticUsageRateTest()
        {
            string strStartTime = "2016-1-1";
            string strEndTime = "2016-5-1";
            DateTime startTime = DateTime.Parse(strStartTime);
            DateTime endTime = DateTime.Parse(strEndTime);

            var antibioticPerson = new Mock<IAntibioticPerson>();
            antibioticPerson.Setup(s => s.GetAntibioticPerson(startTime, endTime)).Returns(10);
            var registerPerson = new Mock<IRegisterPerson>();
            registerPerson.Setup(s => s.GetRegisterPerson(startTime, endTime)).Returns(20);
           

            var result =
             new AntibioticUsageRate
             {
                 AntibioticPerson = antibioticPerson.Object.GetAntibioticPerson(startTime, endTime),
                 RegisterPerson = registerPerson.Object.GetRegisterPerson(startTime, endTime)
             };
            

            Assert.AreEqual(result.UsageRate, (Decimal)50);
        }
    }
}