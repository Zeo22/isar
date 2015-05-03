namespace ISAR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class temp1 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Permisos", name: "ApplicationRole_Id", newName: "Rol_Id");
            RenameIndex(table: "dbo.Permisos", name: "IX_ApplicationRole_Id", newName: "IX_Rol_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Permisos", name: "IX_Rol_Id", newName: "IX_ApplicationRole_Id");
            RenameColumn(table: "dbo.Permisos", name: "Rol_Id", newName: "ApplicationRole_Id");
        }
    }
}
