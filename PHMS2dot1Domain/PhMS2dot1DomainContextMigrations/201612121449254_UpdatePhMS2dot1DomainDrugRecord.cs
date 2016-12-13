namespace PhMS2dot1Domain.PhMS2dot1DomainContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePhMS2dot1DomainDrugRecord : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AntibioticLevels", "Origin_KSSDJID", c => c.Int());
            AddColumn("dbo.AntibioticLevels", "IsSpecial", c => c.Boolean(nullable: false));
            AddColumn("dbo.InPatientDrugRecords", "Origin_KSSDJID", c => c.Int());
            AddColumn("dbo.InPatientDrugRecords", "Origin_DEPT_ID", c => c.Long(nullable: false));
            AddColumn("dbo.InPatientDrugRecords", "IsWesternMedicine", c => c.Boolean(nullable: false));
            AddColumn("dbo.InPatientDrugRecords", "IsTraditionalChineseMedicine", c => c.Boolean(nullable: false));
            AddColumn("dbo.InPatientDrugRecords", "IsChinesePatentMedicine", c => c.Boolean(nullable: false));
            AddColumn("dbo.OutPatientDrugRecords", "Origin_KSSDJID", c => c.Int());
            AddColumn("dbo.OutPatientDrugRecords", "IsTraditionalChineseMedicine", c => c.Boolean(nullable: false));
            AddColumn("dbo.OutPatientDrugRecords", "EffectiveConstituentAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.AntibioticLevels", "Origin_KSSDJ");
            DropColumn("dbo.InPatientDrugRecords", "Origin_KSSDJ");
            DropColumn("dbo.OutPatientDrugRecords", "Origin_KSSDJ");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OutPatientDrugRecords", "Origin_KSSDJ", c => c.Int());
            AddColumn("dbo.InPatientDrugRecords", "Origin_KSSDJ", c => c.Int());
            AddColumn("dbo.AntibioticLevels", "Origin_KSSDJ", c => c.Int());
            DropColumn("dbo.OutPatientDrugRecords", "EffectiveConstituentAmount");
            DropColumn("dbo.OutPatientDrugRecords", "IsTraditionalChineseMedicine");
            DropColumn("dbo.OutPatientDrugRecords", "Origin_KSSDJID");
            DropColumn("dbo.InPatientDrugRecords", "IsChinesePatentMedicine");
            DropColumn("dbo.InPatientDrugRecords", "IsTraditionalChineseMedicine");
            DropColumn("dbo.InPatientDrugRecords", "IsWesternMedicine");
            DropColumn("dbo.InPatientDrugRecords", "Origin_DEPT_ID");
            DropColumn("dbo.InPatientDrugRecords", "Origin_KSSDJID");
            DropColumn("dbo.AntibioticLevels", "IsSpecial");
            DropColumn("dbo.AntibioticLevels", "Origin_KSSDJID");
        }
    }
}
