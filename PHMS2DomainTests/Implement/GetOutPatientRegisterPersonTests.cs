using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PHMS2Domain.Factory;
using PHMS2Domain.Implement;
using PHMS2Domain.Interface;
using PHMS2DomainTests.Implement;
using ClassViewModelToDomain;

using Moq;

namespace PHMS2Domain.Implement.Tests
{
    [TestClass()]
    public class GetOutPatientRegisterPersonTests
    {
        [TestMethod()]
        public void GetOutPatientRegisterPersonTest()
        {
            string strStartTime = "2016-1-2";
            string strEndTime = "2016-4-11";
            DateTime startTime = DateTime.Parse(strStartTime);
            DateTime endTime = DateTime.Parse(strEndTime);
            BaseDataTest bst = new BaseDataTest(startTime, endTime);
            var iRegisterInDuration = new Mock<IRegisterInDuration>();
            iRegisterInDuration.Setup(r => r.GetRegisterInDuration(bst.startTime, bst.endTime)).Returns(bst.registersList);
            var factory = new Mock<IDomainInnerFactory>();
            factory.Setup(f => f.CreateRegisterInDuration(EnumOutPatientCategories.OUTPATIENT)).Returns(iRegisterInDuration.Object);

            DomainUnitOfWork uow = new DomainUnitOfWork(factory.Object);

            var result = new ImRegisterPerson.GetOutPatientRegisterPerson(uow).GetRegisterPerson(bst.startTime, bst.endTime);
            Assert.Fail();
        }       
    }
}