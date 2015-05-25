namespace ISAR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPeriodosEstrategias : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Estrategias", "Periodo_ID", "dbo.Periodos");
            DropIndex("dbo.Estrategias", new[] { "Periodo_ID" });
            CreateTable(
                "dbo.Estrategias_Periodos",
                c => new
                    {
                        EstrategiaID = c.Int(nullable: false),
                        PeriodoID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.EstrategiaID, t.PeriodoID })
                .ForeignKey("dbo.Estrategias", t => t.EstrategiaID, cascadeDelete: true)
                .ForeignKey("dbo.Periodos", t => t.PeriodoID, cascadeDelete: true)
                .Index(t => t.EstrategiaID)
                .Index(t => t.PeriodoID);
            
            DropColumn("dbo.Estrategias", "Periodo_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Estrategias", "Periodo_ID", c => c.Int());
            DropForeignKey("dbo.Estrategias_Periodos", "PeriodoID", "dbo.Periodos");
            DropForeignKey("dbo.Estrategias_Periodos", "EstrategiaID", "dbo.Estrategias");
            DropIndex("dbo.Estrategias_Periodos", new[] { "PeriodoID" });
            DropIndex("dbo.Estrategias_Periodos", new[] { "EstrategiaID" });
            DropTable("dbo.Estrategias_Periodos");
            CreateIndex("dbo.Estrategias", "Periodo_ID");
            AddForeignKey("dbo.Estrategias", "Periodo_ID", "dbo.Periodos", "ID");
        }
    }
}
