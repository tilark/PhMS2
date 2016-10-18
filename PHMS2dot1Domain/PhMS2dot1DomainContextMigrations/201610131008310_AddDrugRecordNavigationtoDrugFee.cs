namespace PhMS2dot1Domain.PhMS2dot1DomainContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDrugRecordNavigationtoDrugFee : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.DrugFees", "DrugRecordID");
            AddForeignKey("dbo.DrugFees", "DrugRecordID", "dbo.DrugRecords", "DrugRecordID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DrugFees", "DrugRecordID", "dbo.DrugRecords");
            DropIndex("dbo.DrugFees", new[] { "DrugRecordID" });
        }
    }
}
