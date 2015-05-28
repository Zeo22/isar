namespace ISAR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditIndicadorFuente : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Indicadores", "FuenteInformacion", c => c.String());
            AlterColumn("dbo.Indicadores", "Nombre", c => c.String(nullable: false));
            AlterColumn("dbo.Indicadores", "Descripcion", c => c.String(nullable: false));
            DropColumn("dbo.Indicadores", "FuenteInforacion");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Indicadores", "FuenteInforacion", c => c.String());
            AlterColumn("dbo.Indicadores", "Descripcion", c => c.String());
            AlterColumn("dbo.Indicadores", "Nombre", c => c.String());
            DropColumn("dbo.Indicadores", "FuenteInformacion");
        }
    }
}
