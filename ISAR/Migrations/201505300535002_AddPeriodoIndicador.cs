namespace ISAR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPeriodoIndicador : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Periodos", "Indicador_ID", "dbo.Indicadores");
            DropIndex("dbo.Periodos", new[] { "Indicador_ID" });
            CreateTable(
                "dbo.Indicadores_Periodos",
                c => new
                    {
                        IndicadorID = c.Int(nullable: false),
                        PeriodoID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.IndicadorID, t.PeriodoID })
                .ForeignKey("dbo.Indicadores", t => t.IndicadorID, cascadeDelete: true)
                .ForeignKey("dbo.Periodos", t => t.PeriodoID, cascadeDelete: true)
                .Index(t => t.IndicadorID)
                .Index(t => t.PeriodoID);
            
            DropColumn("dbo.Periodos", "Indicador_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Periodos", "Indicador_ID", c => c.Int());
            DropForeignKey("dbo.Indicadores_Periodos", "PeriodoID", "dbo.Periodos");
            DropForeignKey("dbo.Indicadores_Periodos", "IndicadorID", "dbo.Indicadores");
            DropIndex("dbo.Indicadores_Periodos", new[] { "PeriodoID" });
            DropIndex("dbo.Indicadores_Periodos", new[] { "IndicadorID" });
            DropTable("dbo.Indicadores_Periodos");
            CreateIndex("dbo.Periodos", "Indicador_ID");
            AddForeignKey("dbo.Periodos", "Indicador_ID", "dbo.Indicadores", "ID");
        }
    }
}
