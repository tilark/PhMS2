using Microsoft.VisualStudio.TestTools.UnitTesting;
using PHMS2Domain.Implement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PHMS2Domain.Models;
using Moq;
using PHMS2Domain.Interface;
using PHMS2Domain.Factory;
using PHMS2DomainTests.Implement;
using ClassViewModelToDomain;

namespace PHMS2Domain.Implement.Tests
{
    [TestClass()]
    public class ImGetEssentialDrugCategoryNumbersTests
    {
        
        [TestMethod()]
        public void GetEssentialDrugCategoryNumbersTest()
        {
            string strStartTime = "2016-4-2";
            string strEndTime = "2016-4-4";
            DateTime startTime = DateTime.Parse(strStartTime);
            DateTime endTime = DateTime.Parse(strEndTime);
            BaseDataTest bst = new BaseDataTest(startTime, endTime);

            var iRegisterInDuration = new Mock<IRegisterInDuration>();
            iRegisterInDuration.Setup(r => r.GetRegisterInDuration(bst.startTime, bst.endTime)).Returns(bst.registersList);
            var factory = new Mock<IDomainInnerFactory>();
            factory.Setup(f => f.CreateRegisterFromPrescription(EnumOutPatientCategories.OUTPATIENT_EMERGEMENT)).Returns(iRegisterInDuration.Object);

            DomainUnitOfWork uow = new DomainUnitOfWork(factory.Object);
            var getEssentialDrug = new ImGetEssentialDrugCategoryNumbers(uow);
            var result = getEssentialDrug.GetEssentialDrugCategoryNumbers(bst.startTime, bst.endTime);
            //Assert.AreNotEqual(result, 0);
            Assert.AreEqual(result, -2);
            //Assert.Fail();
        }
       

    }
}