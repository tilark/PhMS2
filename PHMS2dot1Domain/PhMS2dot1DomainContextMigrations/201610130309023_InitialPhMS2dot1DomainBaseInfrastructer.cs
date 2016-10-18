namespace PhMS2dot1Domain.PhMS2dot1DomainContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialPhMS2dot1DomainBaseInfrastructer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AntibioticLevels",
                c => new
                    {
                        AntibioicLevelID = c.Int(nullable: false, identity: true),
                        Origin_KSSDJ = c.Int(),
                        IsAntibiotic = c.Boolean(nullable: false),
                        IsNonRestrict = c.Boolean(nullable: false),
                        IsRestrict = c.Boolean(nullable: false),
                        AntibioticLevelName = c.String(),
                        AntibioticLevelRemarks = c.String(),
                    })
                .PrimaryKey(t => t.AntibioicLevelID);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        DepartmentID = c.Int(nullable: false, identity: true),
                        ORIGIN_DEPT_ID = c.Int(),
                        DepartmentName = c.String(),
                    })
                .PrimaryKey(t => t.DepartmentID);
            
            CreateTable(
                "dbo.Doctors",
                c => new
                    {
                        DoctorID = c.Int(nullable: false, identity: true),
                        ORIGIN_EMPLOYEE_ID = c.Int(),
                        DoctorName = c.String(),
                        DoctorCode = c.String(),
                    })
                .PrimaryKey(t => t.DoctorID);
            
            CreateTable(
                "dbo.DrugFees",
                c => new
                    {
                        DrugFeeID = c.Guid(nullable: false),
                        ORIGIN_ID = c.Guid(),
                        DrugRecordID = c.Guid(nullable: false),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 4),
                        ORIGIN_Unit = c.String(),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 4),
                        ActualPrice = c.Decimal(nullable: false, precision: 18, scale: 4),
                        ChargeTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.DrugFeeID);
            
            CreateTable(
                "dbo.DrugRecords",
                c => new
                    {
                        DrugRecordID = c.Guid(nullable: false),
                        ORIGIN_ORDER_ID = c.Guid(),
                        InPatientID = c.Guid(nullable: false),
                        ORIGIN_KSSDJ = c.Int(),
                        ORIGIN_EXEC_DEPT = c.Int(nullable: false),
                        ORIGIN_ORDER_DOC = c.Int(nullable: false),
                        ORIGIN_CJID = c.Int(nullable: false),
                        ProductName = c.String(),
                        IsEssential = c.Boolean(nullable: false),
                        DosageForm = c.String(),
                        DDD = c.Decimal(nullable: false, precision: 18, scale: 4),
                        ORIGIN_ORDER_USAGE = c.String(),
                    })
                .PrimaryKey(t => t.DrugRecordID)
                .ForeignKey("dbo.InPatients", t => t.InPatientID)
                .Index(t => t.InPatientID);
            
            CreateTable(
                "dbo.InPatients",
                c => new
                    {
                        InPatientID = c.Guid(nullable: false),
                        Origin_INPATIENT_ID = c.Guid(),
                        PatientID = c.Guid(nullable: false),
                        CaseNumber = c.String(),
                        Times = c.Int(nullable: false),
                        InDate = c.DateTime(nullable: false),
                        OutDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.InPatientID)
                .ForeignKey("dbo.Patients", t => t.PatientID, cascadeDelete: true)
                .Index(t => t.PatientID);
            
            CreateTable(
                "dbo.Patients",
                c => new
                    {
                        PatientID = c.Guid(nullable: false),
                        Origin_PATIENT_ID = c.Guid(),
                        BirthDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.PatientID);
            
            CreateTable(
                "dbo.DrugUnits",
                c => new
                    {
                        DrugUnitID = c.Int(nullable: false, identity: true),
                        ORIGIN_UNIT = c.String(),
                        IsUseByBottle = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.DrugUnitID);
            
            CreateTable(
                "dbo.DrugUsages",
                c => new
                    {
                        DrugUsageID = c.Int(nullable: false, identity: true),
                        ORIGIN_ORDER_USAGE = c.String(),
                        IsUseForInjection = c.Boolean(nullable: false),
                        IsUseForIntravenousTransfusion = c.Boolean(nullable: false),
                        DrugUsageRemarks = c.String(),
                    })
                .PrimaryKey(t => t.DrugUsageID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InPatients", "PatientID", "dbo.Patients");
            DropForeignKey("dbo.DrugRecords", "InPatientID", "dbo.InPatients");
            DropIndex("dbo.InPatients", new[] { "PatientID" });
            DropIndex("dbo.DrugRecords", new[] { "InPatientID" });
            DropTable("dbo.DrugUsages");
            DropTable("dbo.DrugUnits");
            DropTable("dbo.Patients");
            DropTable("dbo.InPatients");
            DropTable("dbo.DrugRecords");
            DropTable("dbo.DrugFees");
            DropTable("dbo.Doctors");
            DropTable("dbo.Departments");
            DropTable("dbo.AntibioticLevels");
        }
    }
}
