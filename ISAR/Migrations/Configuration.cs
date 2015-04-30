namespace ISAR.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Models;

    internal sealed class Configuration : DbMigrationsConfiguration<ISAR.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ISAR.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            
            RouteCollection routes = new RouteCollection();

            ISAR.RouteConfig.RegisterRoutes(routes);
            System.Web.Mvc.UrlHelper Url = new System.Web.Mvc.UrlHelper(new RequestContext(), routes);

            // Niveles Organizacionales
            context.NivelesOrganizacionales.AddOrUpdate(
                new NivelOrganizacional() { ID = 1, Nombre = "Presidencias" },
                new NivelOrganizacional() { ID = 2, Nombre = "VicePresidencias" },
                new NivelOrganizacional() { ID = 3, Nombre = "Unidades Operativas" }
            );
            // Tipo de Objetivos
            context.TipoObjetivo.AddOrUpdate(
                new TipoObjetivo() { ID = 1, Nombre = "Organización" },
                new TipoObjetivo() { ID = 2, Nombre = "Estratégico" },
                new TipoObjetivo() { ID = 3, Nombre = "Operativo" }
            );
            // Categorias Objetivo
            context.CategoriaObjetivo.AddOrUpdate(
                new CategoriaObjetivo() { ID = 1, Nombre = "Finanzas" },
                new CategoriaObjetivo() { ID = 2, Nombre = "Clientes" },
                new CategoriaObjetivo() { ID = 3, Nombre = "Procesos" },
                new CategoriaObjetivo() { ID = 4, Nombre = "Innovación" },
                new CategoriaObjetivo() { ID = 5, Nombre = "Crecimiento" }
            );
            // Tipo Alineacion
            context.TipoAlineacion.AddOrUpdate(
                new TipoAlineacion() { ID = 1, Nombre = "Vertical" },
                new TipoAlineacion() { ID = 2, Nombre = "Matricial" }
            );
            // Grupo Pantalla
            context.GrupoPantalla.AddOrUpdate(
                new GrupoPantalla() { ID = 1, Nombre = "Nivel Superior" },
                new GrupoPantalla() { ID = 2, Nombre = "Nivel Vicepresidencia" },
                new GrupoPantalla() { ID = 3, Nombre = "Nivel Operativo" },
                new GrupoPantalla() { ID = 4, Nombre = "Administración" }
            ); 
        }
    }
}
