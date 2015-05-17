namespace ISAR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPuesto : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Puestos",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Usuarios", "Puesto_ID", c => c.Int());
            CreateIndex("dbo.Usuarios", "Puesto_ID");
            AddForeignKey("dbo.Usuarios", "Puesto_ID", "dbo.Puestos", "ID");
            DropColumn("dbo.Usuarios", "Puesto");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Usuarios", "Puesto", c => c.String());
            DropForeignKey("dbo.Usuarios", "Puesto_ID", "dbo.Puestos");
            DropIndex("dbo.Usuarios", new[] { "Puesto_ID" });
            DropColumn("dbo.Usuarios", "Puesto_ID");
            DropTable("dbo.Puestos");
        }
    }
}
