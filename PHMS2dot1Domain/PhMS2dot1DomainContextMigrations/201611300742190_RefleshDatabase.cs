namespace PhMS2dot1Domain.PhMS2dot1DomainContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RefleshDatabase : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.OutPatients", "CancelChargeTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OutPatients", "CancelChargeTime", c => c.DateTime(nullable: false));
        }
    }
}
