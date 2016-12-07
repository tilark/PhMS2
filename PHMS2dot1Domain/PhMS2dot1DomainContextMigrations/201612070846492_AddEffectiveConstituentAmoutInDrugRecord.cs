namespace PhMS2dot1Domain.PhMS2dot1DomainContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEffectiveConstituentAmoutInDrugRecord : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InPatientDrugRecords", "EffectiveConstituentAmount", c => c.Decimal(nullable: false, precision: 18, scale: 4, defaultValue:0M));
        }
        
        public override void Down()
        {
            DropColumn("dbo.InPatientDrugRecords", "EffectiveConstituentAmount");
        }
    }
}
