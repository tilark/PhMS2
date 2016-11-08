namespace PhMS2dot1Domain.PhMS2dot1DomainContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addIsHemodialysis : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.InPatientDrugRecords", "InPatientID", "dbo.InPatients");
            DropForeignKey("dbo.OutPatientPrescriptions", "OutPatientID", "dbo.OutPatients");
            AddColumn("dbo.InPatients", "IsHemodialysis", c => c.Boolean(nullable: false));
            AddForeignKey("dbo.InPatientDrugRecords", "InPatientID", "dbo.InPatients", "InPatientID", cascadeDelete: true);
            AddForeignKey("dbo.OutPatientPrescriptions", "OutPatientID", "dbo.OutPatients", "OutPatientID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OutPatientPrescriptions", "OutPatientID", "dbo.OutPatients");
            DropForeignKey("dbo.InPatientDrugRecords", "InPatientID", "dbo.InPatients");
            DropColumn("dbo.InPatients", "IsHemodialysis");
            AddForeignKey("dbo.OutPatientPrescriptions", "OutPatientID", "dbo.OutPatients", "OutPatientID");
            AddForeignKey("dbo.InPatientDrugRecords", "InPatientID", "dbo.InPatients", "InPatientID");
        }
    }
}
