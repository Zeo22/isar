namespace ISAR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class temp : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Roles_Permisos", "RolID", "dbo.Roles");
            DropForeignKey("dbo.Roles_Permisos", "PermisoID", "dbo.Permisos");
            DropIndex("dbo.Roles_Permisos", new[] { "RolID" });
            DropIndex("dbo.Roles_Permisos", new[] { "PermisoID" });
            AddColumn("dbo.Permisos", "ApplicationRole_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Permisos", "ApplicationRole_Id");
            AddForeignKey("dbo.Permisos", "ApplicationRole_Id", "dbo.Roles", "Id");
            DropTable("dbo.Roles_Permisos");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Roles_Permisos",
                c => new
                    {
                        RolID = c.String(nullable: false, maxLength: 128),
                        PermisoID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RolID, t.PermisoID });
            
            DropForeignKey("dbo.Permisos", "ApplicationRole_Id", "dbo.Roles");
            DropIndex("dbo.Permisos", new[] { "ApplicationRole_Id" });
            DropColumn("dbo.Permisos", "ApplicationRole_Id");
            CreateIndex("dbo.Roles_Permisos", "PermisoID");
            CreateIndex("dbo.Roles_Permisos", "RolID");
            AddForeignKey("dbo.Roles_Permisos", "PermisoID", "dbo.Permisos", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Roles_Permisos", "RolID", "dbo.Roles", "Id", cascadeDelete: true);
        }
    }
}
