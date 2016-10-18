using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using PHMS2Domain.Interface;
using PHMS2Domain.Models;

namespace PHMS2Domain.Tests.Implement
{
    [TestClass()]
    public class GetOutPatientRegisterInDurationTests
    {
        [TestMethod()]
        public void GetOutPatientRegisterInDurationTest()
        {
            string strStartTime = "2016-1-1";
            string strEndTime = "2016-5-1";
            DateTime startTime = DateTime.Parse(strStartTime);
            DateTime endTime = DateTime.Parse(strEndTime);
            List<Registers> registersList = new List<Registers>();
            var mockRegisterInDuration = new Mock<IRegisterInDuration>();
            mockRegisterInDuration.Setup(s => s.GetRegisterInDuration(startTime, endTime)).Returns(registersList);
            Assert.IsNull(mockRegisterInDuration.Object);
        }

        
    }
}