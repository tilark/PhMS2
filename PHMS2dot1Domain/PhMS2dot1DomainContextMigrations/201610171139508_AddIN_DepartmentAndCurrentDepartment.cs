namespace PhMS2dot1Domain.PhMS2dot1DomainContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIN_DepartmentAndCurrentDepartment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InPatients", "Origin_IN_DEPT", c => c.Long(nullable: false));
            AddColumn("dbo.InPatients", "Origin_DEPT_ID", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.InPatients", "Origin_DEPT_ID");
            DropColumn("dbo.InPatients", "Origin_IN_DEPT");
        }
    }
}
