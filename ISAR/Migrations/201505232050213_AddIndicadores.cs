namespace ISAR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIndicadores : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.IndicadoresComportamiento",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Indicadores",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Descripcion = c.String(),
                        FechaInicio = c.DateTime(nullable: false),
                        Formula = c.String(),
                        FuenteInforacion = c.String(),
                        Area_ID = c.Int(),
                        Comportamiento_ID = c.Int(),
                        Frecuencia_ID = c.Int(),
                        Responsable_Id = c.String(maxLength: 128),
                        Tipo_ID = c.Int(),
                        UmbralAmarillo_ID = c.Int(),
                        UmbralRojo_ID = c.Int(),
                        UmbralVerde_ID = c.Int(),
                        UnidadDeMedida_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Areas", t => t.Area_ID)
                .ForeignKey("dbo.IndicadoresComportamiento", t => t.Comportamiento_ID)
                .ForeignKey("dbo.FrecuenciaMedicion", t => t.Frecuencia_ID)
                .ForeignKey("dbo.Usuarios", t => t.Responsable_Id)
                .ForeignKey("dbo.TipoIndicador", t => t.Tipo_ID)
                .ForeignKey("dbo.Umbrales", t => t.UmbralAmarillo_ID)
                .ForeignKey("dbo.Umbrales", t => t.UmbralRojo_ID)
                .ForeignKey("dbo.Umbrales", t => t.UmbralVerde_ID)
                .ForeignKey("dbo.UnidadesDeMedida", t => t.UnidadDeMedida_ID)
                .Index(t => t.Area_ID)
                .Index(t => t.Comportamiento_ID)
                .Index(t => t.Frecuencia_ID)
                .Index(t => t.Responsable_Id)
                .Index(t => t.Tipo_ID)
                .Index(t => t.UmbralAmarillo_ID)
                .Index(t => t.UmbralRojo_ID)
                .Index(t => t.UmbralVerde_ID)
                .Index(t => t.UnidadDeMedida_ID);
            
            CreateTable(
                "dbo.FrecuenciaMedicion",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.MetasIndicadores",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FechaInicio = c.DateTime(nullable: false),
                        FechaFin = c.DateTime(nullable: false),
                        Meta = c.Single(nullable: false),
                        Resultado = c.Single(nullable: false),
                        Cumplimiento = c.Single(nullable: false),
                        MetaCerrada = c.Boolean(nullable: false),
                        ResultadoCerrado = c.Boolean(nullable: false),
                        Indicador_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Indicadores", t => t.Indicador_ID)
                .Index(t => t.Indicador_ID);
            
            CreateTable(
                "dbo.TipoIndicador",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Umbrales",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Desde = c.Single(nullable: false),
                        Hasta = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Indicadores", "UnidadDeMedida_ID", "dbo.UnidadesDeMedida");
            DropForeignKey("dbo.Indicadores", "UmbralVerde_ID", "dbo.Umbrales");
            DropForeignKey("dbo.Indicadores", "UmbralRojo_ID", "dbo.Umbrales");
            DropForeignKey("dbo.Indicadores", "UmbralAmarillo_ID", "dbo.Umbrales");
            DropForeignKey("dbo.Indicadores", "Tipo_ID", "dbo.TipoIndicador");
            DropForeignKey("dbo.Indicadores", "Responsable_Id", "dbo.Usuarios");
            DropForeignKey("dbo.MetasIndicadores", "Indicador_ID", "dbo.Indicadores");
            DropForeignKey("dbo.Indicadores", "Frecuencia_ID", "dbo.FrecuenciaMedicion");
            DropForeignKey("dbo.Indicadores", "Comportamiento_ID", "dbo.IndicadoresComportamiento");
            DropForeignKey("dbo.Indicadores", "Area_ID", "dbo.Areas");
            DropIndex("dbo.MetasIndicadores", new[] { "Indicador_ID" });
            DropIndex("dbo.Indicadores", new[] { "UnidadDeMedida_ID" });
            DropIndex("dbo.Indicadores", new[] { "UmbralVerde_ID" });
            DropIndex("dbo.Indicadores", new[] { "UmbralRojo_ID" });
            DropIndex("dbo.Indicadores", new[] { "UmbralAmarillo_ID" });
            DropIndex("dbo.Indicadores", new[] { "Tipo_ID" });
            DropIndex("dbo.Indicadores", new[] { "Responsable_Id" });
            DropIndex("dbo.Indicadores", new[] { "Frecuencia_ID" });
            DropIndex("dbo.Indicadores", new[] { "Comportamiento_ID" });
            DropIndex("dbo.Indicadores", new[] { "Area_ID" });
            DropTable("dbo.Umbrales");
            DropTable("dbo.TipoIndicador");
            DropTable("dbo.MetasIndicadores");
            DropTable("dbo.FrecuenciaMedicion");
            DropTable("dbo.Indicadores");
            DropTable("dbo.IndicadoresComportamiento");
        }
    }
}
