namespace ISAR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditDelete : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Atribuciones", "Objetivo_ID", "dbo.Objetivos");
            DropIndex("dbo.Atribuciones", new[] { "Objetivo_ID" });
            CreateTable(
                "dbo.Atribuciones_Objetivos",
                c => new
                    {
                        AtribucionID = c.Int(nullable: false),
                        ObjetivoID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AtribucionID, t.ObjetivoID })
                .ForeignKey("dbo.Atribuciones", t => t.AtribucionID, cascadeDelete: true)
                .ForeignKey("dbo.Objetivos", t => t.ObjetivoID, cascadeDelete: true)
                .Index(t => t.AtribucionID)
                .Index(t => t.ObjetivoID);
            
            DropColumn("dbo.Atribuciones", "Objetivo_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Atribuciones", "Objetivo_ID", c => c.Int());
            DropForeignKey("dbo.Atribuciones_Objetivos", "ObjetivoID", "dbo.Objetivos");
            DropForeignKey("dbo.Atribuciones_Objetivos", "AtribucionID", "dbo.Atribuciones");
            DropIndex("dbo.Atribuciones_Objetivos", new[] { "ObjetivoID" });
            DropIndex("dbo.Atribuciones_Objetivos", new[] { "AtribucionID" });
            DropTable("dbo.Atribuciones_Objetivos");
            CreateIndex("dbo.Atribuciones", "Objetivo_ID");
            AddForeignKey("dbo.Atribuciones", "Objetivo_ID", "dbo.Objetivos", "ID");
        }
    }
}
