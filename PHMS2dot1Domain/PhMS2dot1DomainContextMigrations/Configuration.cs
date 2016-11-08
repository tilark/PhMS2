namespace PhMS2dot1Domain.PhMS2dot1DomainContextMigrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using PhMS2dot1Domain.Models;
    internal sealed class Configuration : DbMigrationsConfiguration<PhMS2dot1Domain.Models.PhMS2dot1DomainContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"PhMS2dot1DomainContextMigrations";
        }

        protected override void Seed(PhMS2dot1Domain.Models.PhMS2dot1DomainContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            var guid1 = Guid.Parse("a1ac51b3-8497-49be-920e-ea070b1f8067");
            var guid2 = Guid.Parse("bbf2c365-0efb-4ce6-a12d-c405910193ac");
            var guid3 = Guid.Parse("75eaccac-3a03-456d-a447-8f90c575d9ae");
            var guid4 = Guid.Parse("c9b4add3-32d6-4b5d-a2cc-6f929266b596");
            var guid5 = Guid.Parse("421a5dde-d9cf-4844-9bc0-ef3114331802");
            var guid6 = Guid.Parse("f17beda9-530b-4167-a138-3e10b9a0e0de");
            var guid7 = Guid.Parse("88090c83-6e1c-4380-a1b2-383d7d56cfd5");
            var guid8 = Guid.Parse("32276913-86b5-4723-b190-7a66b32b0df4");
            var guid9 = Guid.Parse("49f39f62-f491-4b68-9d88-8270cfd8f8bf");
            var guid10 = Guid.Parse("3abf4cb7-3aa2-4d9e-a982-f1c947b431ea");

            #region Patient
            var patient1 = new Patient
            {
                PatientID = guid1

            };
            var patient2 = new Patient
            {
                PatientID = guid2

            };
            var patient3 = new Patient
            {
                PatientID = guid3

            };
            context.Patients.AddOrUpdate(i => i.PatientID,
                patient1,
                patient2,
                patient3);
            context.SaveChanges();
            #endregion
            #region InPatient

            var inPatient1 = new InPatient
            {
                InPatientID = guid1,
                PatientID = patient1.PatientID,
                InDate = DateTime.Parse("2016-7-1 10:00:00"),
                OutDate = DateTime.Parse("2016-7-3 23:59:59"),
                Origin_DEPT_ID = 132,
                Origin_IN_DEPT = 132,
                Times = 2,                 
            };
            var inPatient2 = new InPatient
            {
                InPatientID = guid2,
                PatientID = patient2.PatientID,
                InDate = DateTime.Parse("2016-7-2 10:00:00"),
                OutDate = DateTime.Parse("2016-7-3 23:59:59"),
                Origin_DEPT_ID = 132,
                Origin_IN_DEPT = 132,
                Times = 2,
            };
            var inPatient3 = new InPatient
            {
                InPatientID = guid3,
                PatientID = patient3.PatientID,
                InDate = DateTime.Parse("2016-7-2 10:00:00"),
                OutDate = DateTime.Parse("2016-7-4 23:59:59"),
                Origin_DEPT_ID = 132,
                Origin_IN_DEPT = 132,
                Times = 2,
            };
            context.InPatients.AddOrUpdate(i => i.InPatientID,
                inPatient1,
                inPatient2,
                inPatient3);
            context.SaveChanges();
            #endregion

            #region DrugRecord

            var drugRecord1 = new InPatientDrugRecord
            {
                InPatientDrugRecordID = guid1,
                InPatientID = inPatient1.InPatientID,
                IsEssential = true,
                Origin_KSSDJ = 0,
                Origin_CJID = 1,
                Origin_EXEC_DEPT = 132,
                Origin_ORDER_DOC = 15,
                ProductName = "测试药品1",
                Origin_ORDER_USAGE = "口服"
            };
            var drugRecord2 = new InPatientDrugRecord
            {
                InPatientDrugRecordID = guid2,
                InPatientID = inPatient1.InPatientID,
                IsEssential = true,
                Origin_KSSDJ = 0,
                Origin_CJID = 2,
                Origin_EXEC_DEPT = 132,
                Origin_ORDER_DOC = 15,
                ProductName = "测试药品2",
                Origin_ORDER_USAGE = "口服"
            };

            var drugRecord3 = new InPatientDrugRecord
            {
                InPatientDrugRecordID = guid3,
                InPatientID = inPatient2.InPatientID,
                IsEssential = true,
                Origin_KSSDJ = 3,
                Origin_CJID = 132,
                Origin_EXEC_DEPT = 132,
                Origin_ORDER_DOC = 15,
                ProductName = "测试药品3",
                Origin_ORDER_USAGE = "口服"
            };
            var drugRecord4 = new InPatientDrugRecord
            {
                InPatientDrugRecordID = guid4,
                InPatientID = inPatient2.InPatientID,
                IsEssential = true,
                Origin_KSSDJ = 1,
                Origin_CJID = 133,
                Origin_EXEC_DEPT = 132,
                Origin_ORDER_DOC = 15,
                ProductName = "测试药品4",
                Origin_ORDER_USAGE = "口服"
            };

            context.InPatientDrugRecords.AddOrUpdate(i => i.InPatientDrugRecordID,
                drugRecord1,
                drugRecord2,
                drugRecord3,
                drugRecord4);
            context.SaveChanges();
            #endregion

            #region DrugFee
            var drugFee1 = new DrugFee
            {
                DrugFeeID = guid5,
                InPatientDrugRecordID = drugRecord1.InPatientDrugRecordID,
                ChargeTime = DateTime.Parse("2016-7-2 10:00:00"),
                ActualPrice = 15,
                Origin_Unit = "包",
                Quantity = 3,
                UnitPrice = 5.00M
            };
            var drugFee2 = new DrugFee
            {
                DrugFeeID = guid6,
                 InPatientDrugRecordID = drugRecord1.InPatientDrugRecordID,
                ChargeTime = DateTime.Parse("2016-7-2 16:00:00"),
                ActualPrice = 15,
                Origin_Unit = "包",
                Quantity = 3,
                UnitPrice = 5.00M
            };
            var drugFee3 = new DrugFee
            {
                DrugFeeID = guid7,
                InPatientDrugRecordID = drugRecord2.InPatientDrugRecordID,
                ChargeTime = DateTime.Parse("2016-7-2 16:00:00"),
                ActualPrice = 15,
                Origin_Unit = "包",
                Quantity = 3,
                UnitPrice = 5.00M
            };
            var drugFee4 = new DrugFee
            {
                DrugFeeID = guid8,
                InPatientDrugRecordID = drugRecord2.InPatientDrugRecordID,
                ChargeTime = DateTime.Parse("2016-7-2 16:00:00"),
                ActualPrice = 15,
                Origin_Unit = "包",
                Quantity = 3,
                UnitPrice = 5.00M
            };
            var drugFee5 = new DrugFee
            {
                DrugFeeID = guid9,
                InPatientDrugRecordID = drugRecord3.InPatientDrugRecordID,
                ChargeTime = DateTime.Parse("2016-7-2 16:00:00"),
                ActualPrice = 15,
                Origin_Unit = "包",
                Quantity = 3,
                UnitPrice = 5.00M
            };
            var drugFee6 = new DrugFee
            {
                DrugFeeID = guid10,
                InPatientDrugRecordID = drugRecord3.InPatientDrugRecordID,
                ChargeTime = DateTime.Parse("2016-7-2 16:00:00"),
                ActualPrice = 15,
                Origin_Unit = "包",
                Quantity = 3,
                UnitPrice = 5.00M
            };
            var drugFee7 = new DrugFee
            {
                DrugFeeID = guid1,
                InPatientDrugRecordID = drugRecord4.InPatientDrugRecordID,
                ChargeTime = DateTime.Parse("2016-7-2 16:00:00"),
                ActualPrice = 15,
                Origin_Unit = "包",
                Quantity = 3,
                UnitPrice = 5.00M
            };
            var drugFee8 = new DrugFee
            {
                DrugFeeID = guid2,
                InPatientDrugRecordID = drugRecord4.InPatientDrugRecordID,
                ChargeTime = DateTime.Parse("2016-7-2 16:00:00"),
                ActualPrice = 15,
                Origin_Unit = "包",
                Quantity = 3,
                UnitPrice = 5.00M
            };

            //测试找到OutDate在DrugFee之前，即在出院后仍有数据产生

            var drugFee9 = new DrugFee
            {
                DrugFeeID = guid3,
                InPatientDrugRecordID = drugRecord4.InPatientDrugRecordID,
                ChargeTime = DateTime.Parse("2016-7-2 16:00:00"),
                ActualPrice = -15,
                Origin_Unit = "包",
                Quantity = -3,
                UnitPrice = 5.00M
            };
            var drugFee10 = new DrugFee
            {
                DrugFeeID = guid4,
                InPatientDrugRecordID = drugRecord4.InPatientDrugRecordID,
                ChargeTime = DateTime.Parse("2016-7-2 16:00:00"),
                ActualPrice = -15,
                Origin_Unit = "包",
                Quantity = -3,
                UnitPrice = 5.00M
            };
            context.DrugFees.AddOrUpdate(i => i.DrugFeeID,
                drugFee1,
                drugFee2,
                drugFee3,
                drugFee4,
                drugFee5,
                drugFee6,
                drugFee7,
                drugFee8,
                drugFee9,
                drugFee10
                );
            context.SaveChanges();
            #endregion
        }
    }
}
