namespace ISAR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIndicadorFechaFinal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Indicadores", "FechaFinal", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Indicadores", "FechaFinal");
        }
    }
}
