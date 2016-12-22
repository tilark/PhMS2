namespace PhMS2dot1Domain.PhMS2dot1DomainContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsRelateToNoParentDrugFeeInDrugRecord : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InPatientDrugRecords", "IsRelateToNoParentDrugFee", c => c.Boolean(nullable: false,defaultValue:false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.InPatientDrugRecords", "IsRelateToNoParentDrugFee");
        }
    }
}
