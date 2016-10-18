using PHMS2Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHMS2DomainTests.Implement
{
    public class BaseDataTest
    {
        public List<AntibioticManageLevels> antibioticManageLevels;
        public List<DrugMaintenances> drugMaintenancesList;
        public List<OutPatientPrescriptionDetails> outPatientPrescriptionDetailsList;
        public List<OutPatientPrescriptions> outPatientPrescriptionList;
        public List<Registers> registersList;
        public DateTime startTime = DateTime.MinValue;
        public DateTime endTime = DateTime.MaxValue;
        public BaseDataTest()
        {
            string strStartTime = "2016-1-1";
            string strEndTime = "2016-4-2";
            this.startTime = DateTime.Parse(strStartTime);
            this.endTime = DateTime.Parse(strEndTime);
            InitialBaseData();
        }
        public BaseDataTest(DateTime startTime, DateTime endTime)
        {
            this.startTime = startTime;
            this.endTime = endTime;
            InitialBaseData();
        }

        internal void InitialBaseData()
        {
            antibioticManageLevels = new List<AntibioticManageLevels>();
            drugMaintenancesList = new List<DrugMaintenances>();
            outPatientPrescriptionDetailsList = new List<OutPatientPrescriptionDetails>();
            outPatientPrescriptionList = new List<OutPatientPrescriptions>();
            registersList = new List<Registers>();

            

            //Initial DrugMaintenances
            InitialDrugMaintenances();
            //Initial OutPatientPrescriptionDetails
            IntialOutPatientPrescriptionDetails();
            //InitialOutPatientPrescription
            InitialOutPatientPrescriptions();
            //InitialRegisters
            InitialRegisters();

        }
        #region InitialData
        private void InitialRegisters()
        {
            var reg1 = new Registers
            {
                RegisterId = System.Guid.NewGuid(),
                ChargeTime = DateTime.Parse("2016-4-1 09:10:22")
            };
            var reg2 = new Registers
            {
                RegisterId = System.Guid.NewGuid(),
                ChargeTime = DateTime.Parse("2016-4-10 09:10:22")
            };
            reg1.OutPatientPrescriptions.Add(outPatientPrescriptionList.ElementAt(0));
            reg1.OutPatientPrescriptions.Add(outPatientPrescriptionList.ElementAt(1));
            reg2.OutPatientPrescriptions.Add(outPatientPrescriptionList.ElementAt(2));

            registersList.Add(reg1);
            registersList.Add(reg2);
        }

        private void InitialOutPatientPrescriptions()
        {
            var opp0 = new OutPatientPrescriptions
            {
                OutPatientPrescriptionId = System.Guid.NewGuid(),
                ChargeTime = DateTime.Parse("2016-4-1 10:10:10"),
                IsValid = true
            };
            var opp1 = new OutPatientPrescriptions
            {
                OutPatientPrescriptionId = System.Guid.NewGuid(),
                ChargeTime = DateTime.Parse("2016-4-3 10:10:10"),
                IsValid = true
            };
            var opp2 = new OutPatientPrescriptions
            {
                OutPatientPrescriptionId = System.Guid.NewGuid(),
                ChargeTime = DateTime.Parse("2016-4-10 10:10:10"),
                IsValid = true
            };
            //reg1
            opp0.OutPatientPrescriptionDetails.Add(outPatientPrescriptionDetailsList.ElementAt(0));
            opp0.OutPatientPrescriptionDetails.Add(outPatientPrescriptionDetailsList.ElementAt(1));
            opp1.OutPatientPrescriptionDetails.Add(outPatientPrescriptionDetailsList.ElementAt(2));
            opp1.OutPatientPrescriptionDetails.Add(outPatientPrescriptionDetailsList.ElementAt(3));
            //reg2
            opp2.OutPatientPrescriptionDetails.Add(outPatientPrescriptionDetailsList.ElementAt(4));
            opp2.OutPatientPrescriptionDetails.Add(outPatientPrescriptionDetailsList.ElementAt(5));


            outPatientPrescriptionList.Add(opp0);
            outPatientPrescriptionList.Add(opp1);
            outPatientPrescriptionList.Add(opp2);

        }



        private void IntialOutPatientPrescriptionDetails()
        {
            var oppd0 = new OutPatientPrescriptionDetails
            {
                OutPatientPrescriptionDetailsId = System.Guid.NewGuid(),
                Quantity = 4
            };
            var oppd1 = new OutPatientPrescriptionDetails
            {
                OutPatientPrescriptionDetailsId = System.Guid.NewGuid(),
                Quantity = 4
            };

            var oppd2 = new OutPatientPrescriptionDetails
            {
                OutPatientPrescriptionDetailsId = System.Guid.NewGuid(),
                Quantity = -4
            };
            var oppd3 = new OutPatientPrescriptionDetails
            {
                OutPatientPrescriptionDetailsId = System.Guid.NewGuid(),
                Quantity = -4
            };
            var oppd4 = new OutPatientPrescriptionDetails
            {
                OutPatientPrescriptionDetailsId = System.Guid.NewGuid(),
                Quantity = 4
            };
            var oppd5 = new OutPatientPrescriptionDetails
            {
                OutPatientPrescriptionDetailsId = System.Guid.NewGuid(),
                Quantity = 4
            };
            //opp1
            oppd0.DrugMaintenances = drugMaintenancesList.ElementAt(0);
            oppd1.DrugMaintenances = drugMaintenancesList.ElementAt(1);
            //opp2
            oppd2.DrugMaintenances = drugMaintenancesList.ElementAt(0);
            oppd3.DrugMaintenances = drugMaintenancesList.ElementAt(1);
            //opp3
            oppd4.DrugMaintenances = drugMaintenancesList.ElementAt(2);
            oppd5.DrugMaintenances = drugMaintenancesList.ElementAt(3);

            outPatientPrescriptionDetailsList.Add(oppd0);
            outPatientPrescriptionDetailsList.Add(oppd1);
            outPatientPrescriptionDetailsList.Add(oppd2);
            outPatientPrescriptionDetailsList.Add(oppd3);
            outPatientPrescriptionDetailsList.Add(oppd4);
            outPatientPrescriptionDetailsList.Add(oppd5);
        }
        private void InitialDrugMaintenances()
        {
            var drug0 = new DrugMaintenances
            {
                DrugMaintenanceId = 1,
                ProductNumber = "XY0076",
                ProductName = "头孢地尼分散片(希福尼)",
                IsEssential = true,
                Ddd = 0.6M,
                AntibioticManageLevelId = 2,
                UnitCost = 54.6M,
                UnitName = "50mg*12片/盒"
            };
            var drug1 = new DrugMaintenances
            {
                DrugMaintenanceId = 2,
                ProductNumber = "XY0077",
                ProductName = "注射用哌拉西林钠他唑巴坦钠(特治星)",
                IsEssential = true,
                Ddd = 14M,
                AntibioticManageLevelId = 2,
                UnitCost = 163.63M,
                UnitName = "4.5g/支"
            };
            var drug2 = new DrugMaintenances
            {
                DrugMaintenanceId = 3,
                ProductNumber = "XY0078",
                ProductName = "鲑鱼降钙素喷鼻剂(金尔力)",
                IsEssential = false,
                Ddd = 0M,
                AntibioticManageLevelId = 0,
                UnitCost = 163.63M,
                UnitName = "4.5g/支"
            };
            var drug3 = new DrugMaintenances
            {
                DrugMaintenanceId = 4,
                ProductNumber = "XY0079",
                ProductName = "甘草酸二铵肠溶胶囊（天晴甘平）",
                IsEssential = false,
                Ddd = 0M,
                AntibioticManageLevelId = 0,
                UnitCost = 29.67M,
                UnitName = "50mg*24粒/盒",
            };
            var drug4 = new DrugMaintenances
            {
                DrugMaintenanceId = 5,
                ProductNumber = "XY0080",
                ProductName = "甘草酸二铵肠溶胶囊（天晴甘平）",
                IsEssential = false,
                Ddd = 0M,
                AntibioticManageLevelId = 0,
                UnitCost = 29.67M,
                UnitName = "50mg*24粒/盒",
            };
            drugMaintenancesList.Add(drug0);
            drugMaintenancesList.Add(drug1);
            drugMaintenancesList.Add(drug2);
            drugMaintenancesList.Add(drug3);
            drugMaintenancesList.Add(drug4);

        }
        #endregion
    }
}
