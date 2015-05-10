namespace ISAR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAtribucionesToObjetivo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Atribuciones", "Objetivo_ID", c => c.Int());
            AlterColumn("dbo.Objetivos", "Nombre", c => c.String(nullable: false));
            AlterColumn("dbo.Objetivos", "Descripcion", c => c.String(nullable: false));
            CreateIndex("dbo.Atribuciones", "Objetivo_ID");
            AddForeignKey("dbo.Atribuciones", "Objetivo_ID", "dbo.Objetivos", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Atribuciones", "Objetivo_ID", "dbo.Objetivos");
            DropIndex("dbo.Atribuciones", new[] { "Objetivo_ID" });
            AlterColumn("dbo.Objetivos", "Descripcion", c => c.String());
            AlterColumn("dbo.Objetivos", "Nombre", c => c.String());
            DropColumn("dbo.Atribuciones", "Objetivo_ID");
        }
    }
}
