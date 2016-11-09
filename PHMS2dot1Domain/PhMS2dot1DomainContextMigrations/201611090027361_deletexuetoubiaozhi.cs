namespace PhMS2dot1Domain.PhMS2dot1DomainContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deletexuetoubiaozhi : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.InPatients", "IsHemodialysis");
        }
        
        public override void Down()
        {
            AddColumn("dbo.InPatients", "IsHemodialysis", c => c.Boolean(nullable: false));
        }
    }
}
