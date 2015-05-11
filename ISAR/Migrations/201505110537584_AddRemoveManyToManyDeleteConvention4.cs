namespace ISAR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRemoveManyToManyDeleteConvention4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Objetivos_Estrategias", "ObjetivoID", "dbo.Objetivos");
            DropForeignKey("dbo.Objetivos_Estrategias", "EstrategiaID", "dbo.Estrategias");
            DropForeignKey("dbo.Roles_Permisos", "RolID", "dbo.Roles");
            DropForeignKey("dbo.Roles_Permisos", "PermisoID", "dbo.Permisos");
            AddForeignKey("dbo.Objetivos_Estrategias", "ObjetivoID", "dbo.Objetivos", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Objetivos_Estrategias", "EstrategiaID", "dbo.Estrategias", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Roles_Permisos", "RolID", "dbo.Roles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Roles_Permisos", "PermisoID", "dbo.Permisos", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Roles_Permisos", "PermisoID", "dbo.Permisos");
            DropForeignKey("dbo.Roles_Permisos", "RolID", "dbo.Roles");
            DropForeignKey("dbo.Objetivos_Estrategias", "EstrategiaID", "dbo.Estrategias");
            DropForeignKey("dbo.Objetivos_Estrategias", "ObjetivoID", "dbo.Objetivos");
            AddForeignKey("dbo.Roles_Permisos", "PermisoID", "dbo.Permisos", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Roles_Permisos", "RolID", "dbo.Roles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Objetivos_Estrategias", "EstrategiaID", "dbo.Estrategias", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Objetivos_Estrategias", "ObjetivoID", "dbo.Objetivos", "ID", cascadeDelete: true);
        }
    }
}
