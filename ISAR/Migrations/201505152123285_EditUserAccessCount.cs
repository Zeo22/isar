namespace ISAR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditUserAccessCount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Usuarios", "AccessCount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Usuarios", "AccessCount");
        }
    }
}
