namespace ISAR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Areas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        AreaPadre_ID = c.Int(),
                        Nivel_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Areas", t => t.AreaPadre_ID)
                .ForeignKey("dbo.NivelOrganizacional", t => t.Nivel_ID)
                .Index(t => t.AreaPadre_ID)
                .Index(t => t.Nivel_ID);
            
            CreateTable(
                "dbo.NivelOrganizacional",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.CategoriaObjetivo",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Estrategias",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Titulo = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Objetivos",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Descripcion = c.String(),
                        FechaInicio = c.DateTime(nullable: false),
                        FechaFinal = c.DateTime(nullable: false),
                        Alineacion_ID = c.Int(),
                        Area_ID = c.Int(),
                        Categoria_ID = c.Int(),
                        Objetivo_ID = c.Int(),
                        Responsable_Id = c.String(maxLength: 128),
                        Tipo_ID = c.Int(),
                        Estrategia_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.TipoAlineacion", t => t.Alineacion_ID)
                .ForeignKey("dbo.Areas", t => t.Area_ID)
                .ForeignKey("dbo.CategoriaObjetivo", t => t.Categoria_ID)
                .ForeignKey("dbo.Objetivos", t => t.Objetivo_ID)
                .ForeignKey("dbo.Usuarios", t => t.Responsable_Id)
                .ForeignKey("dbo.TipoObjetivo", t => t.Tipo_ID)
                .ForeignKey("dbo.Estrategias", t => t.Estrategia_ID)
                .Index(t => t.Alineacion_ID)
                .Index(t => t.Area_ID)
                .Index(t => t.Categoria_ID)
                .Index(t => t.Objetivo_ID)
                .Index(t => t.Responsable_Id)
                .Index(t => t.Tipo_ID)
                .Index(t => t.Estrategia_ID);
            
            CreateTable(
                "dbo.TipoAlineacion",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Usuarios",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Nombre = c.String(nullable: false),
                        Puesto = c.String(),
                        Activo = c.Boolean(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        UsuarioArea_ID = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Areas", t => t.UsuarioArea_ID)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.UsuarioArea_ID);
            
            CreateTable(
                "dbo.PeticionesUsuario",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Usuarios", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.IniciosUsuario",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.Usuarios", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.RolesUsuario",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Usuarios", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.TipoObjetivo",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.GrupoPantalla",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Pantallas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        URL = c.String(),
                        Nombre = c.String(),
                        Grupo_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.GrupoPantalla", t => t.Grupo_ID)
                .Index(t => t.Grupo_ID);
            
            CreateTable(
                "dbo.Periodos",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        FechaInicio = c.DateTime(nullable: false),
                        FechaFin = c.DateTime(nullable: false),
                        Activo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Permisos",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Lectura = c.Boolean(nullable: false),
                        Escritura = c.Boolean(nullable: false),
                        Pantalla_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Pantallas", t => t.Pantalla_ID)
                .Index(t => t.Pantalla_ID);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.UnidadesDeMedida",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Roles_Permisos",
                c => new
                    {
                        RolID = c.String(nullable: false, maxLength: 128),
                        PermisoID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RolID, t.PermisoID })
                .ForeignKey("dbo.Roles", t => t.RolID, cascadeDelete: true)
                .ForeignKey("dbo.Permisos", t => t.PermisoID, cascadeDelete: true)
                .Index(t => t.RolID)
                .Index(t => t.PermisoID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RolesUsuario", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Roles_Permisos", "PermisoID", "dbo.Permisos");
            DropForeignKey("dbo.Roles_Permisos", "RolID", "dbo.Roles");
            DropForeignKey("dbo.Permisos", "Pantalla_ID", "dbo.Pantallas");
            DropForeignKey("dbo.Pantallas", "Grupo_ID", "dbo.GrupoPantalla");
            DropForeignKey("dbo.Objetivos", "Estrategia_ID", "dbo.Estrategias");
            DropForeignKey("dbo.Objetivos", "Tipo_ID", "dbo.TipoObjetivo");
            DropForeignKey("dbo.Objetivos", "Responsable_Id", "dbo.Usuarios");
            DropForeignKey("dbo.Usuarios", "UsuarioArea_ID", "dbo.Areas");
            DropForeignKey("dbo.RolesUsuario", "UserId", "dbo.Usuarios");
            DropForeignKey("dbo.IniciosUsuario", "UserId", "dbo.Usuarios");
            DropForeignKey("dbo.PeticionesUsuario", "UserId", "dbo.Usuarios");
            DropForeignKey("dbo.Objetivos", "Objetivo_ID", "dbo.Objetivos");
            DropForeignKey("dbo.Objetivos", "Categoria_ID", "dbo.CategoriaObjetivo");
            DropForeignKey("dbo.Objetivos", "Area_ID", "dbo.Areas");
            DropForeignKey("dbo.Objetivos", "Alineacion_ID", "dbo.TipoAlineacion");
            DropForeignKey("dbo.Areas", "Nivel_ID", "dbo.NivelOrganizacional");
            DropForeignKey("dbo.Areas", "AreaPadre_ID", "dbo.Areas");
            DropIndex("dbo.Roles_Permisos", new[] { "PermisoID" });
            DropIndex("dbo.Roles_Permisos", new[] { "RolID" });
            DropIndex("dbo.Roles", "RoleNameIndex");
            DropIndex("dbo.Permisos", new[] { "Pantalla_ID" });
            DropIndex("dbo.Pantallas", new[] { "Grupo_ID" });
            DropIndex("dbo.RolesUsuario", new[] { "RoleId" });
            DropIndex("dbo.RolesUsuario", new[] { "UserId" });
            DropIndex("dbo.IniciosUsuario", new[] { "UserId" });
            DropIndex("dbo.PeticionesUsuario", new[] { "UserId" });
            DropIndex("dbo.Usuarios", new[] { "UsuarioArea_ID" });
            DropIndex("dbo.Usuarios", "UserNameIndex");
            DropIndex("dbo.Objetivos", new[] { "Estrategia_ID" });
            DropIndex("dbo.Objetivos", new[] { "Tipo_ID" });
            DropIndex("dbo.Objetivos", new[] { "Responsable_Id" });
            DropIndex("dbo.Objetivos", new[] { "Objetivo_ID" });
            DropIndex("dbo.Objetivos", new[] { "Categoria_ID" });
            DropIndex("dbo.Objetivos", new[] { "Area_ID" });
            DropIndex("dbo.Objetivos", new[] { "Alineacion_ID" });
            DropIndex("dbo.Areas", new[] { "Nivel_ID" });
            DropIndex("dbo.Areas", new[] { "AreaPadre_ID" });
            DropTable("dbo.Roles_Permisos");
            DropTable("dbo.UnidadesDeMedida");
            DropTable("dbo.Roles");
            DropTable("dbo.Permisos");
            DropTable("dbo.Periodos");
            DropTable("dbo.Pantallas");
            DropTable("dbo.GrupoPantalla");
            DropTable("dbo.TipoObjetivo");
            DropTable("dbo.RolesUsuario");
            DropTable("dbo.IniciosUsuario");
            DropTable("dbo.PeticionesUsuario");
            DropTable("dbo.Usuarios");
            DropTable("dbo.TipoAlineacion");
            DropTable("dbo.Objetivos");
            DropTable("dbo.Estrategias");
            DropTable("dbo.CategoriaObjetivo");
            DropTable("dbo.NivelOrganizacional");
            DropTable("dbo.Areas");
        }
    }
}
