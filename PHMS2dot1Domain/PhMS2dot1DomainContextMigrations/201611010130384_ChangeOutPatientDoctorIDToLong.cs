namespace PhMS2dot1Domain.PhMS2dot1DomainContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeOutPatientDoctorIDToLong : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.OutPatientPrescriptions", "Origin_YSDM", c => c.Long());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OutPatientPrescriptions", "Origin_YSDM", c => c.Int());
        }
    }
}
