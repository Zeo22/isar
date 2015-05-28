namespace ISAR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIndicadorPeriodos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Periodos", "Indicador_ID", c => c.Int());
            AddColumn("dbo.TipoIndicador", "Nivel_ID", c => c.Int());
            CreateIndex("dbo.Periodos", "Indicador_ID");
            CreateIndex("dbo.TipoIndicador", "Nivel_ID");
            AddForeignKey("dbo.Periodos", "Indicador_ID", "dbo.Indicadores", "ID");
            AddForeignKey("dbo.TipoIndicador", "Nivel_ID", "dbo.NivelOrganizacional", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TipoIndicador", "Nivel_ID", "dbo.NivelOrganizacional");
            DropForeignKey("dbo.Periodos", "Indicador_ID", "dbo.Indicadores");
            DropIndex("dbo.TipoIndicador", new[] { "Nivel_ID" });
            DropIndex("dbo.Periodos", new[] { "Indicador_ID" });
            DropColumn("dbo.TipoIndicador", "Nivel_ID");
            DropColumn("dbo.Periodos", "Indicador_ID");
        }
    }
}
