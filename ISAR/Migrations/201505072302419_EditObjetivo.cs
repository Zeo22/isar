namespace ISAR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditObjetivo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CategoriaObjetivo", "Nivel_ID", c => c.Int());
            AddColumn("dbo.Estrategias", "Periodo_ID", c => c.Int());
            AddColumn("dbo.Objetivos", "Periodo_ID", c => c.Int());
            CreateIndex("dbo.CategoriaObjetivo", "Nivel_ID");
            CreateIndex("dbo.Objetivos", "Periodo_ID");
            CreateIndex("dbo.Estrategias", "Periodo_ID");
            AddForeignKey("dbo.CategoriaObjetivo", "Nivel_ID", "dbo.NivelOrganizacional", "ID");
            AddForeignKey("dbo.Objetivos", "Periodo_ID", "dbo.Periodos", "ID");
            AddForeignKey("dbo.Estrategias", "Periodo_ID", "dbo.Periodos", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Estrategias", "Periodo_ID", "dbo.Periodos");
            DropForeignKey("dbo.Objetivos", "Periodo_ID", "dbo.Periodos");
            DropForeignKey("dbo.CategoriaObjetivo", "Nivel_ID", "dbo.NivelOrganizacional");
            DropIndex("dbo.Estrategias", new[] { "Periodo_ID" });
            DropIndex("dbo.Objetivos", new[] { "Periodo_ID" });
            DropIndex("dbo.CategoriaObjetivo", new[] { "Nivel_ID" });
            DropColumn("dbo.Objetivos", "Periodo_ID");
            DropColumn("dbo.Estrategias", "Periodo_ID");
            DropColumn("dbo.CategoriaObjetivo", "Nivel_ID");
        }
    }
}
