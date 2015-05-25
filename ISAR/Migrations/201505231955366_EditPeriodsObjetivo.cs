namespace ISAR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditPeriodsObjetivo : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Objetivos", "Periodo_ID", "dbo.Periodos");
            DropIndex("dbo.Objetivos", new[] { "Periodo_ID" });
            CreateTable(
                "dbo.Objetivos_Periodos",
                c => new
                    {
                        ObjetivoID = c.Int(nullable: false),
                        PeriodoID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ObjetivoID, t.PeriodoID })
                .ForeignKey("dbo.Objetivos", t => t.ObjetivoID, cascadeDelete: true)
                .ForeignKey("dbo.Periodos", t => t.PeriodoID, cascadeDelete: true)
                .Index(t => t.ObjetivoID)
                .Index(t => t.PeriodoID);
            
            DropColumn("dbo.Objetivos", "Periodo_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Objetivos", "Periodo_ID", c => c.Int());
            DropForeignKey("dbo.Objetivos_Periodos", "PeriodoID", "dbo.Periodos");
            DropForeignKey("dbo.Objetivos_Periodos", "ObjetivoID", "dbo.Objetivos");
            DropIndex("dbo.Objetivos_Periodos", new[] { "PeriodoID" });
            DropIndex("dbo.Objetivos_Periodos", new[] { "ObjetivoID" });
            DropTable("dbo.Objetivos_Periodos");
            CreateIndex("dbo.Objetivos", "Periodo_ID");
            AddForeignKey("dbo.Objetivos", "Periodo_ID", "dbo.Periodos", "ID");
        }
    }
}
