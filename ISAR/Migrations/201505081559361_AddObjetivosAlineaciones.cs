namespace ISAR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddObjetivosAlineaciones : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Objetivos", "Objetivo_ID", "dbo.Objetivos");
            DropIndex("dbo.Objetivos", new[] { "Objetivo_ID" });
            CreateTable(
                "dbo.ObjetivosAlineados",
                c => new
                    {
                        ObjetivoID = c.Int(nullable: false),
                        ObjetivoAlineadoID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ObjetivoID, t.ObjetivoAlineadoID })
                .ForeignKey("dbo.Objetivos", t => t.ObjetivoID)
                .ForeignKey("dbo.Objetivos", t => t.ObjetivoAlineadoID)
                .Index(t => t.ObjetivoID)
                .Index(t => t.ObjetivoAlineadoID);
            
            DropColumn("dbo.Objetivos", "Objetivo_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Objetivos", "Objetivo_ID", c => c.Int());
            DropForeignKey("dbo.ObjetivosAlineados", "ObjetivoAlineadoID", "dbo.Objetivos");
            DropForeignKey("dbo.ObjetivosAlineados", "ObjetivoID", "dbo.Objetivos");
            DropIndex("dbo.ObjetivosAlineados", new[] { "ObjetivoAlineadoID" });
            DropIndex("dbo.ObjetivosAlineados", new[] { "ObjetivoID" });
            DropTable("dbo.ObjetivosAlineados");
            CreateIndex("dbo.Objetivos", "Objetivo_ID");
            AddForeignKey("dbo.Objetivos", "Objetivo_ID", "dbo.Objetivos", "ID");
        }
    }
}
