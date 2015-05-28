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
                new NivelOrganizacional() { ID = 1, Nombre = "Presidencia" },
                new NivelOrganizacional() { ID = 2, Nombre = "VicePresidencia" },
                new NivelOrganizacional() { ID = 3, Nombre = "Unidad Operativa" }
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
                new Permiso() { ID = 1, Nombre = "Administrador" },
                new Permiso() { ID = 2, Nombre = "Captura de Estrategias Generales", Grupo = "Estrategias Generales", Opcion = "Captura" },
                new Permiso() { ID = 3, Nombre = "Listado de Estrategias Generales", Grupo = "Estrategias Generales", Opcion = "Listado" },
                new Permiso() { ID = 4, Nombre = "Captura de Objetivos Estratégicos-Organización", Grupo = "Objetivos Estratégicos-Organización", Opcion = "Captura" },
                new Permiso() { ID = 5, Nombre = "Listado de Objetivos Estratégicos-Organización", Grupo = "Objetivos Estratégicos-Organización", Opcion = "Listado" },
                new Permiso() { ID = 6, Nombre = "Líder de Unidad Operativa" },
                new Permiso() { ID = 7, Nombre = "Captura de Objetivos Estratégicos-Específicos", Grupo = "Objetivos Estratégicos-Específicos", Opcion = "Captura" },
                new Permiso() { ID = 8, Nombre = "Listado de Objetivos Estratégicos-Específicos", Grupo = "Objetivos Estratégicos-Específicos", Opcion = "Listado" },
                new Permiso() { ID = 9, Nombre = "Captura de Estrategias Específicas", Grupo = "Estrategias Específicas", Opcion = "Captura" },
                new Permiso() { ID = 10, Nombre = "Listado de Estrategias Específicas", Grupo = "Estrategias Específicas", Opcion = "Listado" },
                new Permiso() { ID = 11, Nombre = "Captura de Objetivos Operativos", Grupo = "Objetivos Operativos", Opcion = "Captura" },
                new Permiso() { ID = 12, Nombre = "Listado de Objetivos Operativos", Grupo = "Objetivos Operativos", Opcion = "Listado" },

                new Permiso() { ID = 13, Nombre = "Captura de Indicadores Estratégicos-Organización", Grupo = "Indicadores Estratégicos-Organización", Opcion = "Captura" },
                new Permiso() { ID = 14, Nombre = "Listado de Indicadores Estratégicos-Organización", Grupo = "Indicadores Estratégicos-Organización", Opcion = "Listado" },

                new Permiso() { ID = 15, Nombre = "Captura de Indicadores Estratégicos-Específicos", Grupo = "Indicadores Estratégicos-Específicos", Opcion = "Captura" },
                new Permiso() { ID = 16, Nombre = "Listado de Indicadores Estratégicos-Específicos", Grupo = "Indicadores Estratégicos-Específicos", Opcion = "Listado" },

                new Permiso() { ID = 17, Nombre = "Captura de Indicadores Operativos", Grupo = "Indicadores Operativos", Opcion = "Captura" },
                new Permiso() { ID = 18, Nombre = "Listado de Indicadores Operativos", Grupo = "Indicadores Operativos", Opcion = "Listado" }
            );
            // Admin
            ApplicationUserManager userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

            var user = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@isar.com",
                Nombre = "Administrador",
                //Puesto = "IT Consultant",
                Activo = true
            };
            var adminresult = userManager.Create(user, "Admin01");
            // Indicadores
            // Comportamiento
            context.Comportamiento.AddOrUpdate(
                new Comportamiento() { ID = 1, Nombre = "Discreto" },
                new Comportamiento() { ID = 2, Nombre = "Continuo" }
            );
            // Tipo Indicador
            context.TipoIndicador.AddOrUpdate(
                new TipoIndicador() { ID = 1, Nivel = context.NivelesOrganizacionales.Find(1), Nombre = "Visión" },
                new TipoIndicador() { ID = 2, Nivel = context.NivelesOrganizacionales.Find(1), Nombre = "Objetivos Estratégicos-Organización" },
                new TipoIndicador() { ID = 3, Nivel = context.NivelesOrganizacionales.Find(2), Nombre = "Objetivos Estratégicos-Estecifico" },
                new TipoIndicador() { ID = 4, Nivel = context.NivelesOrganizacionales.Find(3), Nombre = "Objetivos Operativos" },
                new TipoIndicador() { ID = 5, Nivel = context.NivelesOrganizacionales.Find(3), Nombre = "Procesos" },
                new TipoIndicador() { ID = 6, Nivel = context.NivelesOrganizacionales.Find(3), Nombre = "Proyectos" }
            );
            // Fecuencia de Medicion
            context.FrecuenciaMedicion.AddOrUpdate(
                new FrecuenciaMedicion() { ID = 1, Nombre = "Diario" },
                new FrecuenciaMedicion() { ID = 2, Nombre = "Semanal" },
                new FrecuenciaMedicion() { ID = 3, Nombre = "Mensual" },
                new FrecuenciaMedicion() { ID = 3, Nombre = "Bimestral" },
                new FrecuenciaMedicion() { ID = 3, Nombre = "Trimestral" },
                new FrecuenciaMedicion() { ID = 3, Nombre = "Semestral" },
                new FrecuenciaMedicion() { ID = 3, Nombre = "Anual" }
            );
        }
    }
}
