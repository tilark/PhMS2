namespace PhMS2dot1Domain.PhMS2dot1DomainContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeDoctorIDToLong : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Doctors");
            AlterColumn("dbo.Doctors", "DoctorID", c => c.Long(nullable: false, identity: true));
            AlterColumn("dbo.Doctors", "ORIGIN_EMPLOYEE_ID", c => c.Long());
            AlterColumn("dbo.InPatientDrugRecords", "Origin_ORDER_DOC", c => c.Long(nullable: false));
            AddPrimaryKey("dbo.Doctors", "DoctorID");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Doctors");
            AlterColumn("dbo.InPatientDrugRecords", "Origin_ORDER_DOC", c => c.Int(nullable: false));
            AlterColumn("dbo.Doctors", "ORIGIN_EMPLOYEE_ID", c => c.Int());
            AlterColumn("dbo.Doctors", "DoctorID", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Doctors", "DoctorID");
        }
    }
}
