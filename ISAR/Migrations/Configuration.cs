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
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Collections.Generic;

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
                new TipoObjetivo() { ID = 1, Nombre = "Estratégicos-Organización" },
                new TipoObjetivo() { ID = 2, Nombre = "Estratégicos-Específicos" },
                new TipoObjetivo() { ID = 3, Nombre = "Operativos" }
            );
            // Categorias Objetivo
            context.CategoriaObjetivo.AddOrUpdate(
                // Nivel 1
                new CategoriaObjetivo() { ID = 1, Nivel = context.NivelesOrganizacionales.FirstOrDefault(item => item.ID == 1), Nombre = "Finanzas" },
                new CategoriaObjetivo() { ID = 2, Nivel = context.NivelesOrganizacionales.FirstOrDefault(item => item.ID == 1), Nombre = "Clientes" },
                new CategoriaObjetivo() { ID = 3, Nivel = context.NivelesOrganizacionales.FirstOrDefault(item => item.ID == 1), Nombre = "Procesos" },
                new CategoriaObjetivo() { ID = 4, Nivel = context.NivelesOrganizacionales.FirstOrDefault(item => item.ID == 1), Nombre = "Desarrollo Humano" },
                new CategoriaObjetivo() { ID = 5, Nivel = context.NivelesOrganizacionales.FirstOrDefault(item => item.ID == 1), Nombre = "Innovación y Crecimiento" },
                // Nivel 2
                new CategoriaObjetivo() { ID = 6, Nivel = context.NivelesOrganizacionales.FirstOrDefault(item => item.ID == 2), Nombre = "Finanzas" },
                new CategoriaObjetivo() { ID = 7, Nivel = context.NivelesOrganizacionales.FirstOrDefault(item => item.ID == 2), Nombre = "Clientes-Ventas" },
                new CategoriaObjetivo() { ID = 8, Nivel = context.NivelesOrganizacionales.FirstOrDefault(item => item.ID == 2), Nombre = "Procesos-Caña" },
                new CategoriaObjetivo() { ID = 9, Nivel = context.NivelesOrganizacionales.FirstOrDefault(item => item.ID == 2), Nombre = "Procesos-Fábrica" },
                new CategoriaObjetivo() { ID = 10, Nivel = context.NivelesOrganizacionales.FirstOrDefault(item => item.ID == 2), Nombre = "Procesos-Soporte" },
                new CategoriaObjetivo() { ID = 11, Nivel = context.NivelesOrganizacionales.FirstOrDefault(item => item.ID == 2), Nombre = "Desarrollo Humano" },
                new CategoriaObjetivo() { ID = 12, Nivel = context.NivelesOrganizacionales.FirstOrDefault(item => item.ID == 2), Nombre = "Innovación y Crecimiento" },
                // Nivel 2
                new CategoriaObjetivo() { ID = 13, Nivel = context.NivelesOrganizacionales.FirstOrDefault(item => item.ID == 3), Nombre = "Finanzas" },
                new CategoriaObjetivo() { ID = 14, Nivel = context.NivelesOrganizacionales.FirstOrDefault(item => item.ID == 3), Nombre = "Clientes-Ventas" },
                new CategoriaObjetivo() { ID = 15, Nivel = context.NivelesOrganizacionales.FirstOrDefault(item => item.ID == 3), Nombre = "Procesos-Caña" },
                new CategoriaObjetivo() { ID = 16, Nivel = context.NivelesOrganizacionales.FirstOrDefault(item => item.ID == 3), Nombre = "Procesos-Fábrica" },
                new CategoriaObjetivo() { ID = 17, Nivel = context.NivelesOrganizacionales.FirstOrDefault(item => item.ID == 3), Nombre = "Procesos-Soporte" },
                new CategoriaObjetivo() { ID = 18, Nivel = context.NivelesOrganizacionales.FirstOrDefault(item => item.ID == 3), Nombre = "Desarrollo Humano" },
                new CategoriaObjetivo() { ID = 19, Nivel = context.NivelesOrganizacionales.FirstOrDefault(item => item.ID == 3), Nombre = "Innovación y Crecimiento" }
            );
            // Tipo Alineacion
            context.TipoAlineacion.AddOrUpdate(
                new TipoAlineacion() { ID = 1, Nombre = "Vertical" },
                new TipoAlineacion() { ID = 2, Nombre = "Matricial" }
            );
            // Permisos
            context.Permisos.AddOrUpdate(
                new Permiso() { ID = 1, Nombre = "Administración" },
                new Permiso() { ID = 2, Nombre = "Captura de Estrategias", Escritura = true },
                new Permiso() { ID = 3, Nombre = "Listado de Estrategias", Lectura = true },
                new Permiso() { ID = 4, Nombre = "Captura de Objetivos", Escritura = true },
                new Permiso() { ID = 5, Nombre = "Listado de Objetivos", Lectura = true },
                new Permiso() { ID = 6, Nombre = "Líder de Unidad Operativa" },
                new Permiso() { ID = 7, Nombre = "Captura de Indicadores", Escritura = true }
            );
            // Admin
            ApplicationUserManager userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

            var user = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@isar.com",
                Nombre = "Administrador",
                Puesto = "IT Consultant",
                Activo = true
            };
            var adminresult = userManager.Create(user, "Admin01");
        }
    }
}
