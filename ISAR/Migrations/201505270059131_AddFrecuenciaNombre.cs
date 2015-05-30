namespace ISAR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFrecuenciaNombre : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FrecuenciaMedicion", "Nombre", c => c.String());
            DropColumn("dbo.FrecuenciaMedicion", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FrecuenciaMedicion", "Name", c => c.String());
            DropColumn("dbo.FrecuenciaMedicion", "Nombre");
        }
    }
}
