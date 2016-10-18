namespace PhMS2dot1Domain.PhMS2dot1DomainContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixInPatientDrugRecordName : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DrugFees", "DrugRecord_InPatientDrugRecordID", "dbo.InPatientDrugRecords");
            DropIndex("dbo.DrugFees", new[] { "DrugRecord_InPatientDrugRecordID" });
            RenameColumn(table: "dbo.DrugFees", name: "DrugRecord_InPatientDrugRecordID", newName: "InPatientDrugRecordID");
            AlterColumn("dbo.DrugFees", "InPatientDrugRecordID", c => c.Guid(nullable: false));
            CreateIndex("dbo.DrugFees", "InPatientDrugRecordID");
            AddForeignKey("dbo.DrugFees", "InPatientDrugRecordID", "dbo.InPatientDrugRecords", "InPatientDrugRecordID", cascadeDelete: true);
            DropColumn("dbo.DrugFees", "DrugRecordID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DrugFees", "DrugRecordID", c => c.Guid(nullable: false));
            DropForeignKey("dbo.DrugFees", "InPatientDrugRecordID", "dbo.InPatientDrugRecords");
            DropIndex("dbo.DrugFees", new[] { "InPatientDrugRecordID" });
            AlterColumn("dbo.DrugFees", "InPatientDrugRecordID", c => c.Guid());
            RenameColumn(table: "dbo.DrugFees", name: "InPatientDrugRecordID", newName: "DrugRecord_InPatientDrugRecordID");
            CreateIndex("dbo.DrugFees", "DrugRecord_InPatientDrugRecordID");
            AddForeignKey("dbo.DrugFees", "DrugRecord_InPatientDrugRecordID", "dbo.InPatientDrugRecords", "InPatientDrugRecordID");
        }
    }
}
