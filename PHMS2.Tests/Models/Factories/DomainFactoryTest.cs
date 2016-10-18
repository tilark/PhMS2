using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PHMS2Domain.Models;

using PHMS2Domain.Factory;
using PHMS2Domain.Interface;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

namespace PHMS2.Tests.Models.Factories
{
    [TestClass]
    public class DomainFactoryTest
    {
        //private IDomainFactory domainFactory = null;
        
        [ClassInitialize]
        public static void Initial(TestContext context)
        {

        }
        [TestMethod]
        public void DomainInternalFactoryTest()
        {
            string strStartTime = "2016-1-1";
            string strEndTime = "2016-5-1";
            DateTime startTime = DateTime.Parse(strStartTime);
            DateTime endTime = DateTime.Parse(strEndTime);
            var mockPrescriptionInDurationList = new List<OutPatientPrescriptions>();
            var mockDomainFactory = new Mock<IDomainOuterFactory>();
            var iPrescriptionInDuration = new Mock<IPrescriptionInDuration>();
            iPrescriptionInDuration.Setup(s => s.GetPrescriptionInDuration(startTime, endTime)).Returns(mockPrescriptionInDurationList);
        }
    }
}
