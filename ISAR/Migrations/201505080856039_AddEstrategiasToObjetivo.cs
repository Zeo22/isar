namespace ISAR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEstrategiasToObjetivo : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Objetivos", "Estrategia_ID", "dbo.Estrategias");
            DropIndex("dbo.Objetivos", new[] { "Estrategia_ID" });
            CreateTable(
                "dbo.Objetivos_Estrategias",
                c => new
                    {
                        ObjetivoID = c.Int(nullable: false),
                        EstrategiaID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ObjetivoID, t.EstrategiaID })
                .ForeignKey("dbo.Objetivos", t => t.ObjetivoID, cascadeDelete: true)
                .ForeignKey("dbo.Estrategias", t => t.EstrategiaID, cascadeDelete: true)
                .Index(t => t.ObjetivoID)
                .Index(t => t.EstrategiaID);
            
            DropColumn("dbo.Objetivos", "Estrategia_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Objetivos", "Estrategia_ID", c => c.Int());
            DropForeignKey("dbo.Objetivos_Estrategias", "EstrategiaID", "dbo.Estrategias");
            DropForeignKey("dbo.Objetivos_Estrategias", "ObjetivoID", "dbo.Objetivos");
            DropIndex("dbo.Objetivos_Estrategias", new[] { "EstrategiaID" });
            DropIndex("dbo.Objetivos_Estrategias", new[] { "ObjetivoID" });
            DropTable("dbo.Objetivos_Estrategias");
            CreateIndex("dbo.Objetivos", "Estrategia_ID");
            AddForeignKey("dbo.Objetivos", "Estrategia_ID", "dbo.Estrategias", "ID");
        }
    }
}
