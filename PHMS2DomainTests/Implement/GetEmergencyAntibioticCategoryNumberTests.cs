using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using PHMS2Domain.Interface;
using ClassViewModelToDomain.Interface;

namespace PHMS2Domain.Tests.Implement
{
    [TestClass()]
    public class GetEmergencyAntibioticCategoryNumberTests
    {
       
        [TestMethod()]
        public void GetAntibioticCategoryNumberTest()
        {
            string strStartTime = "2016-1-1";
            string strEndTime = "2016-5-1";
            DateTime startTime = DateTime.Parse(strStartTime);
            DateTime endTime = DateTime.Parse(strEndTime);

            var iAntibioticPerson = new Mock<IAntibioticPerson>();
            iAntibioticPerson.Setup(s => s.GetAntibioticPerson(startTime, endTime)).Returns(10);
            Assert.Fail();
        }
    }
}