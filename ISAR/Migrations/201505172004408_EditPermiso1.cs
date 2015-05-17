namespace ISAR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditPermiso1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Permisos", "Grupo", c => c.String());
            AddColumn("dbo.Permisos", "Opcion", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Permisos", "Opcion");
            DropColumn("dbo.Permisos", "Grupo");
        }
    }
}
