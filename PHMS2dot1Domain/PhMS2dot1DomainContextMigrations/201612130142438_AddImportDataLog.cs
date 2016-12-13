namespace PhMS2dot1Domain.PhMS2dot1DomainContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddImportDataLog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ImportDataLogs",
                c => new
                    {
                        ImportDataLogsID = c.Long(nullable: false, identity: true),
                        SourceDatabaseName = c.String(),
                        SourceTableName = c.String(),
                        LocalTableName = c.String(),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        SourceRecordCount = c.Long(nullable: false),
                        SuccessImportRecordCount = c.Long(nullable: false),
                        ReadStartTime = c.DateTime(nullable: false),
                        ReadEndTime = c.DateTime(nullable: false),
                        WriteStartTime = c.DateTime(nullable: false),
                        WriteEndTime = c.DateTime(nullable: false),
                        ErrorMessage = c.String(),
                        Remarks = c.String(),
                    })
                .PrimaryKey(t => t.ImportDataLogsID);
            
            AlterColumn("dbo.OutPatientDrugRecords", "EffectiveConstituentAmount", c => c.Decimal(nullable: false, precision: 18, scale: 4));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OutPatientDrugRecords", "EffectiveConstituentAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropTable("dbo.ImportDataLogs");
        }
    }
}
