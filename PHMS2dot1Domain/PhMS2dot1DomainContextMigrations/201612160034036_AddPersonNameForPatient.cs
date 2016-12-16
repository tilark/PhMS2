namespace PhMS2dot1Domain.PhMS2dot1DomainContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPersonNameForPatient : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InPatients", "PatientName", c => c.String());
            AddColumn("dbo.Patients", "PatientName", c => c.String());
            AddColumn("dbo.OutPatients", "PatientName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OutPatients", "PatientName");
            DropColumn("dbo.Patients", "PatientName");
            DropColumn("dbo.InPatients", "PatientName");
        }
    }
}
