namespace ISAR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEstrategiasArea : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Estrategias", "Area_ID", c => c.Int());
            CreateIndex("dbo.Estrategias", "Area_ID");
            AddForeignKey("dbo.Estrategias", "Area_ID", "dbo.Areas", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Estrategias", "Area_ID", "dbo.Areas");
            DropIndex("dbo.Estrategias", new[] { "Area_ID" });
            DropColumn("dbo.Estrategias", "Area_ID");
        }
    }
}
