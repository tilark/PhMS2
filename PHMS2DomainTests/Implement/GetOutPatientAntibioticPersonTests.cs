using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using PHMS2Domain.Factory;
using PHMS2Domain.Implement;
using PHMS2Domain.Interface;
using PHMS2DomainTests.Implement;
using ClassViewModelToDomain;

namespace PHMS2Domain.Implement.Tests
{
    [TestClass()]
    public class GetOutPatientAntibioticPersonTests
    {
        [TestMethod()]
        public void GetOutPatientAntibioticPersonTest()
        {
            string strStartTime = "2016-1-2";
            string strEndTime = "2016-4-11";
            DateTime startTime = DateTime.Parse(strStartTime);
            DateTime endTime = DateTime.Parse(strEndTime);
            BaseDataTest bst = new BaseDataTest(startTime, endTime);
            var iRegisterInDuration = new Mock<IRegisterInDuration>();
            iRegisterInDuration.Setup(r => r.GetRegisterInDuration(bst.startTime, bst.endTime)).Returns(bst.registersList);
            var factory = new Mock<IDomainInnerFactory>();
            factory.Setup(f => f.CreateRegisterFromPrescription(EnumOutPatientCategories.OUTPATIENT)).Returns(iRegisterInDuration.Object);

            DomainUnitOfWork uow = new DomainUnitOfWork(factory.Object);

            var outPatientAntibioticPerson = new ImAntibioticPerson.GetOutPatientAntibioticPerson(uow);

            var result = outPatientAntibioticPerson.GetAntibioticPerson(startTime, endTime);
            Assert.AreEqual(result, 0);
        }

        
    }
}