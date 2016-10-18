namespace PhMS2dot1Domain.PhMS2dot1DomainContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOutPatientInfrastructer : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DrugRecords", "InPatientID", "dbo.InPatients");
            DropForeignKey("dbo.DrugFees", "DrugRecordID", "dbo.DrugRecords");
            DropIndex("dbo.DrugFees", new[] { "DrugRecordID" });
            DropIndex("dbo.DrugRecords", new[] { "InPatientID" });
            CreateTable(
                "dbo.InPatientDrugRecords",
                c => new
                    {
                        InPatientDrugRecordID = c.Guid(nullable: false),
                        Origin_ORDER_ID = c.Guid(),
                        InPatientID = c.Guid(nullable: false),
                        Origin_KSSDJ = c.Int(),
                        Origin_EXEC_DEPT = c.Int(nullable: false),
                        Origin_ORDER_DOC = c.Int(nullable: false),
                        Origin_CJID = c.Int(nullable: false),
                        ProductName = c.String(),
                        IsEssential = c.Boolean(nullable: false),
                        DosageForm = c.String(),
                        DDD = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Origin_ORDER_USAGE = c.String(),
                    })
                .PrimaryKey(t => t.InPatientDrugRecordID)
                .ForeignKey("dbo.InPatients", t => t.InPatientID)
                .Index(t => t.InPatientID);
            
            CreateTable(
                "dbo.OutPatients",
                c => new
                    {
                        OutPatientID = c.Guid(nullable: false),
                        Origin_GHXXID = c.Guid(),
                        Origin_GHLB = c.Int(),
                        ChargeTime = c.DateTime(nullable: false),
                        CancelChargeTime = c.DateTime(nullable: false),
                        PatientID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.OutPatientID)
                .ForeignKey("dbo.Patients", t => t.PatientID, cascadeDelete: true)
                .Index(t => t.PatientID);
            
            CreateTable(
                "dbo.OutPatientPrescriptions",
                c => new
                    {
                        OutPatientPrescriptionID = c.Guid(nullable: false),
                        Origin_CFID = c.Guid(nullable: false),
                        OutPatientID = c.Guid(nullable: false),
                        ChargeTime = c.DateTime(nullable: false),
                        Origin_KSDM = c.Int(),
                        Origin_YSDM = c.Int(),
                    })
                .PrimaryKey(t => t.OutPatientPrescriptionID)
                .ForeignKey("dbo.OutPatients", t => t.OutPatientID)
                .Index(t => t.OutPatientID);
            
            CreateTable(
                "dbo.OutPatientDrugRecords",
                c => new
                    {
                        OutPatientDrugRecordID = c.Guid(nullable: false),
                        Origin_CFMXID = c.Guid(),
                        OutPatientPrescriptionID = c.Guid(nullable: false),
                        Origin_KSSDJ = c.Int(),
                        Origin_CJID = c.Int(),
                        ProductName = c.String(),
                        IsEssential = c.Boolean(nullable: false),
                        IsWesternMedicine = c.Boolean(nullable: false),
                        IsChinesePatentMedicine = c.Boolean(nullable: false),
                        DosageForm = c.String(),
                        Ddd = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Origin_YFMC = c.String(),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 4),
                        UnitName = c.String(),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 4),
                        ActualPrice = c.Decimal(nullable: false, precision: 18, scale: 4),
                    })
                .PrimaryKey(t => t.OutPatientDrugRecordID)
                .ForeignKey("dbo.OutPatientPrescriptions", t => t.OutPatientPrescriptionID, cascadeDelete: true)
                .Index(t => t.OutPatientPrescriptionID);
            
            CreateTable(
                "dbo.OutPatientCategories",
                c => new
                    {
                        OutPatientCategoryID = c.Int(nullable: false, identity: true),
                        Origin_GHLB = c.Int(),
                        OutPatientCategoryName = c.String(),
                        IsClinic = c.Boolean(nullable: false),
                        IsEmergency = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.OutPatientCategoryID);
            
            AddColumn("dbo.DrugFees", "DrugRecord_InPatientDrugRecordID", c => c.Guid());
            CreateIndex("dbo.DrugFees", "DrugRecord_InPatientDrugRecordID");
            AddForeignKey("dbo.DrugFees", "DrugRecord_InPatientDrugRecordID", "dbo.InPatientDrugRecords", "InPatientDrugRecordID");
            DropTable("dbo.DrugRecords");
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.DrugRecordID);
            
            DropForeignKey("dbo.DrugFees", "DrugRecord_InPatientDrugRecordID", "dbo.InPatientDrugRecords");
            DropForeignKey("dbo.OutPatients", "PatientID", "dbo.Patients");
            DropForeignKey("dbo.OutPatientPrescriptions", "OutPatientID", "dbo.OutPatients");
            DropForeignKey("dbo.OutPatientDrugRecords", "OutPatientPrescriptionID", "dbo.OutPatientPrescriptions");
            DropForeignKey("dbo.InPatientDrugRecords", "InPatientID", "dbo.InPatients");
            DropIndex("dbo.OutPatientDrugRecords", new[] { "OutPatientPrescriptionID" });
            DropIndex("dbo.OutPatientPrescriptions", new[] { "OutPatientID" });
            DropIndex("dbo.OutPatients", new[] { "PatientID" });
            DropIndex("dbo.InPatientDrugRecords", new[] { "InPatientID" });
            DropIndex("dbo.DrugFees", new[] { "DrugRecord_InPatientDrugRecordID" });
            DropColumn("dbo.DrugFees", "DrugRecord_InPatientDrugRecordID");
            DropTable("dbo.OutPatientCategories");
            DropTable("dbo.OutPatientDrugRecords");
            DropTable("dbo.OutPatientPrescriptions");
            DropTable("dbo.OutPatients");
            DropTable("dbo.InPatientDrugRecords");
            CreateIndex("dbo.DrugRecords", "InPatientID");
            CreateIndex("dbo.DrugFees", "DrugRecordID");
            AddForeignKey("dbo.DrugFees", "DrugRecordID", "dbo.DrugRecords", "DrugRecordID", cascadeDelete: true);
            AddForeignKey("dbo.DrugRecords", "InPatientID", "dbo.InPatients", "InPatientID");
        }
    }
}
