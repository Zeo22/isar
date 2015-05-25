namespace ISAR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveObjetivosAlineados : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ObjetivosAlineados", "ObjetivoID", "dbo.Objetivos");
            DropForeignKey("dbo.ObjetivosAlineados", "ObjetivoAlineadoID", "dbo.Objetivos");
            DropIndex("dbo.ObjetivosAlineados", new[] { "ObjetivoID" });
            DropIndex("dbo.ObjetivosAlineados", new[] { "ObjetivoAlineadoID" });
            DropTable("dbo.ObjetivosAlineados");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ObjetivosAlineados",
                c => new
                    {
                        ObjetivoID = c.Int(nullable: false),
                        ObjetivoAlineadoID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ObjetivoID, t.ObjetivoAlineadoID });
            
            CreateIndex("dbo.ObjetivosAlineados", "ObjetivoAlineadoID");
            CreateIndex("dbo.ObjetivosAlineados", "ObjetivoID");
            AddForeignKey("dbo.ObjetivosAlineados", "ObjetivoAlineadoID", "dbo.Objetivos", "ID");
            AddForeignKey("dbo.ObjetivosAlineados", "ObjetivoID", "dbo.Objetivos", "ID");
        }
    }
}
