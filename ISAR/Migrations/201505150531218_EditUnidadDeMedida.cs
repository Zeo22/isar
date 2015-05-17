namespace ISAR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditUnidadDeMedida : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UnidadesDeMedida", "Abreviatura", c => c.String(nullable: false));
            AlterColumn("dbo.UnidadesDeMedida", "Nombre", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UnidadesDeMedida", "Nombre", c => c.String());
            DropColumn("dbo.UnidadesDeMedida", "Abreviatura");
        }
    }
}
