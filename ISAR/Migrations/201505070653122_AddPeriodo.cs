namespace ISAR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPeriodo : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Periodos", "Nombre", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Periodos", "Nombre", c => c.String());
        }
    }
}
