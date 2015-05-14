namespace ISAR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditPermiso : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Permisos", "Lectura");
            DropColumn("dbo.Permisos", "Escritura");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Permisos", "Escritura", c => c.Boolean(nullable: false));
            AddColumn("dbo.Permisos", "Lectura", c => c.Boolean(nullable: false));
        }
    }
}
