namespace ISAR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAtribucion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Atribuciones",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(nullable: false),
                        Area_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Areas", t => t.Area_ID)
                .Index(t => t.Area_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Atribuciones", "Area_ID", "dbo.Areas");
            DropIndex("dbo.Atribuciones", new[] { "Area_ID" });
            DropTable("dbo.Atribuciones");
        }
    }
}
