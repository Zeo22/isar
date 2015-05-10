namespace ISAR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEstrategias : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Estrategias", "Titulo", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Estrategias", "Titulo", c => c.String());
        }
    }
}
