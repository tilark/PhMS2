using Microsoft.VisualStudio.TestTools.UnitTesting;
using PHMS2.Controllers;
using PHMS2.Controllers.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PHMS2.Models.Factories;
using System.Web.Mvc;
using System.Collections;
using System.Data;
using System.Data.Common;
using PHMS2.Tests;
using ClassViewModelToDomain.Interface;
using ClassViewModelToDomain.IFactory;
using Ninject.MockingKernel;
using Ninject.MockingKernel.Moq;
using Moq;
using PHMS2.Models.ViewModels.Interface;
using PHMS2.Models.ViewModels;
using ClassViewModelToDomain;
using PHMS2.Models.ViewModels.Reporter;
using PhMS2dot1Domain.Factories;

namespace PHMS2.Controllers.Tests
{

    [TestClass()]
    public class ReportersControllerTests
    {
        private ReportersController controller = null;
        private ReporterUnitOfWork unitOfWork = null;
        private DateTime startTime = DateTime.MinValue;
        private DateTime endTime = DateTime.MaxValue;
        //Mock<IReporterViewFactory> factoryMock = null;

        internal void InitialBaseData()
        {
            string strStartTime = "2016-1-1";
            string strEndTime = "2016-5-1";
            this.startTime = DateTime.Parse(strStartTime);
            this.endTime = DateTime.Parse(strEndTime);
            Mock<IReporterViewFactory> factoryMock = new Mock<IReporterViewFactory>();
            this.unitOfWork = new ReporterUnitOfWork(factoryMock.Object);
            //this.controller = new ReportersController(this.unitOfWork);
        }
        [AssemblyInitialize]
        public static void AssemblyInitial(TestContext context)
        {


        }
        [ClassInitialize()]
        public static void Initial(TestContext context)
        {

        }
        [TestMethod()]
        public void GetTopTenAntibioticTest()
        {
            //Assert.Fail();


        }

        [TestMethod()]
        public void GetOutpatientAntimicrobialRateTest()
        {
            InitialBaseData();
            AntibioticUsageRate mockData = new AntibioticUsageRate
            {
                AntibioticPerson = 10,
                RegisterPerson = 20,
            };

            var antibioticUsageRate = new Mock<IAntibioticUsageRate>();
            antibioticUsageRate.Setup(a => a.GetAntibioticUsageRate(startTime, endTime, EnumOutPatientCategories.OUTPATIENT)).Returns(mockData);
            Mock<IReporterViewFactory> factoryMock = new Mock<IReporterViewFactory>();

            factoryMock.Setup(f => f.CreateAntibioticUsageRate()).Returns(antibioticUsageRate.Object);
            var returnValue = antibioticUsageRate.Object.GetAntibioticUsageRate(startTime, endTime, EnumOutPatientCategories.OUTPATIENT);

            var resultTest = this.controller.GetOutpatientAntibioticUsageRate(startTime, endTime) as ViewResult;
            Assert.IsNotNull(resultTest.Model);
            Assert.AreEqual(returnValue.AntibioticPerson, mockData.AntibioticPerson);
            Assert.AreEqual(((AntibioticUsageRate)resultTest.Model).AntibioticPerson, mockData.AntibioticPerson);
        }

        [TestMethod()]
        public void GetEssentialDrugRateTest()
        {
            InitialBaseData();

            var mockData = new EssentialDrugCategoryRate
            {
                EssentialDrugNums = 10,
                DrugCategoriesNums = 20
            };
            var essentialDrugRate = new Mock<IEssentialDrugRate>();
            essentialDrugRate.Setup(e => e.GetEssentialDrugCategoryRate(this.startTime, this.endTime)).Returns(mockData);
            Mock<IReporterViewFactory> factoryMock = new Mock<IReporterViewFactory>();


            factoryMock.Setup(f => f.CreateEssentialDrugRate()).Returns(essentialDrugRate.Object);
            var returnValue = essentialDrugRate.Object.GetEssentialDrugCategoryRate(this.startTime, this.endTime);
            Assert.AreEqual(returnValue.EssentialDrugNums, mockData.EssentialDrugNums);
            Assert.AreEqual(returnValue.Rate, Decimal.Round((Decimal)mockData.EssentialDrugNums * 100 / (Decimal)mockData.DrugCategoriesNums, 2));
        }

        [TestMethod()]
        public void GetAverageDrugCategoryTest()
        {
            //InitialBaseData();
            //var mockKernerl = new MoqMockingKernel();

            //var drugCategoryRate = new DrugCategoryRate
            //{
            //    DrugCategoryNums = 10,
            //    RegisterPersons = 100
            //};
            //var iDrugCategoryRate = mockKernerl.GetMock<IDrugCategoryRate>();
            //iDrugCategoryRate.Setup(i => i.GetDrugCategoryRate(this.startTime, this.endTime)).Returns(drugCategoryRate);

            ////int drugCategoryNumbers = 10;
            ////var drugCategoriesNums = mockKernerl.GetMock<IDrugCategoriesNumbers>();
            ////drugCategoriesNums.Setup(d => d.GetDrugCategoriesNumbers(this.startTime, this.endTime)).Returns(drugCategoryNumbers);

            ////int registerPersons = 100;
            ////var registerPerson = mockKernerl.GetMock<IRegisterPerson>();
            ////registerPerson.Setup(r => r.GetRegisterPerson(this.startTime, this.endTime)).Returns(registerPersons);


            //var reporterViewFactory = mockKernerl.GetMock<IReporterViewFactory>();
            //reporterViewFactory.Setup(r => r.CreateDrugCategoryRate()).Returns(iDrugCategoryRate.Object);
            //var domainFactory = mockKernerl.GetMock<IDomainFacotry>();

            ////domainFactory.Setup(d => d.CreateDrugCategoriesNumbers()).Returns(drugCategoriesNums.Object);
            ////domainFactory.Setup(d => d.CreateRegisterPerson(EnumOutPatientCategories.OUTPATIENT_EMERGEMENT)).Returns(registerPerson.Object);

            //var controller = new ReportersController(reporterViewFactory.Object);

            //var viewResult = controller.GetAverageDrugCategory(this.startTime, this.endTime) as ViewResult;

            //var resultModel = (DrugCategoryRate)viewResult.Model;
            //Assert.AreNotEqual(resultModel.DrugCategoryNums, -1);
            //Assert.AreEqual(resultModel.DrugCategoryNums, 10);
            //Assert.Fail();
        }



        //[ClassCleanup()]
        //public static void ClassCleanup()
        //{

        //}
    }
}