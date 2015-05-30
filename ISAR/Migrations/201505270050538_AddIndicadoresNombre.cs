namespace ISAR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIndicadoresNombre : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.IndicadoresComportamiento", "Nombre", c => c.String());
            AddColumn("dbo.TipoIndicador", "Nombre", c => c.String());
            DropColumn("dbo.IndicadoresComportamiento", "Name");
            DropColumn("dbo.TipoIndicador", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TipoIndicador", "Name", c => c.String());
            AddColumn("dbo.IndicadoresComportamiento", "Name", c => c.String());
            DropColumn("dbo.TipoIndicador", "Nombre");
            DropColumn("dbo.IndicadoresComportamiento", "Nombre");
        }
    }
}
