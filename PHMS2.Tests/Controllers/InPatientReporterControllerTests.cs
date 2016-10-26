using Microsoft.VisualStudio.TestTools.UnitTesting;
using PHMS2.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.MockingKernel;
using Ninject.MockingKernel.Moq;
using PHMS2.Models.ViewModels;
//using Moq;
using PHMS2.Models.ViewModels.Interface;
using System.Web.Mvc;
using ClassViewModelToDomain;
using PHMS2.Models.Factories;
using PHMS2.Models.ViewModels.InPatientReporter;

namespace PHMS2.Controllers.Tests
{
    [TestClass()]
    public class InPatientReporterControllerTests
    {
        private DateTime startTime = DateTime.MinValue;
        private DateTime endTime = DateTime.MaxValue;
        private readonly MoqMockingKernel moqKernel;
        public InPatientReporterControllerTests()
        {
            string strStartTime = "2016-1-1";
            string strEndTime = "2016-5-1";
            this.startTime = DateTime.Parse(strStartTime);
            this.endTime = DateTime.Parse(strEndTime);
            moqKernel = new MoqMockingKernel();

        }
        [TestMethod()]
        public void InPatientAntibioticUsageRateTest()
        {
            //var mockData = new List<DepartmentAntibioticUsageRate>();
            var mockData = new DepartmentAntibioticUsageRate();
            mockData.DepartmentAntibioticUsageRateList = new List<DepartmentAntibioticUsageRateDomain>();
            var tempData = new DepartmentAntibioticUsageRateDomain
            {
                AntibioticPerson = 0,
                RegisterPerson = 0,
                DepartmentID = 0,
                DepartmentName = "Empty"
            };
            mockData.DepartmentAntibioticUsageRateList.Add(tempData);

            var antibioticUsageRateMock = this.moqKernel.GetMock<IDepartmentAntibioticUsageRateList>();
            antibioticUsageRateMock.Setup(a => a.GetDepartmentAntibioticUsageRateList(this.startTime, this.endTime)).Returns(mockData);
            var factoryMock = this.moqKernel.GetMock<IInPatientReporterFactory>();
            factoryMock.Setup(f => f.CreateDepartmentAntibioticUsageRateList()).Returns(antibioticUsageRateMock.Object);

            var inPatientController = new InPatientReporterController(factoryMock.Object);
            var returnResult = inPatientController.InPatientAntibioticUsageRateIndex(this.startTime, this.endTime) as ViewResult;
            Assert.IsNotNull(returnResult.Model);
            var model = (DepartmentAntibioticUsageRate) returnResult.Model;
            var item = model.DepartmentAntibioticUsageRateList.FirstOrDefault();
            Assert.IsNotNull(item);
            Assert.AreEqual(item.AntibioticPerson, 0);
            Assert.AreEqual(item.DepartmentName, "Empty");
            
        }
    }
}