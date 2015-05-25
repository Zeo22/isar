using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ISAR.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        #region Models

        //public DbSet<ApplicationUser> AppUsuarios { get; set; }
        //public DbSet<ApplicationRole> AppRoles { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<NivelOrganizacional> NivelesOrganizacionales { get; set; }
        public DbSet<Permiso> Permisos { get; set; }
        public DbSet<Periodo> Periodos { get; set; }
        public DbSet<UnidadDeMedida> UnidadesDeMedida { get; set; }
        public DbSet<Estrategia> Estrategias { get; set; }
        public DbSet<TipoObjetivo> TipoObjetivo { get; set; }
        public DbSet<CategoriaObjetivo> CategoriaObjetivo { get; set; }
        public DbSet<TipoAlineacion> TipoAlineacion { get; set; }
        public DbSet<Objetivo> Objetivos { get; set; }
        public DbSet<Atribucion> Atribuciones { get; set; }
        public DbSet<Puesto> Puestos { get; set; }
        public DbSet<Comportamiento> Comportamiento { get; set; }
        public DbSet<Indicador> Indicadores { get; set; }

        #endregion

        public ApplicationDbContext()
            : base("DatabaseContext", throwIfV1Schema: false)
        {
        }

        static ApplicationDbContext()
        {
            Database.SetInitializer<ApplicationDbContext>(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Migrations.Configuration>());
        }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            // ASP.NET Identity
            //modelBuilder.Entity<IdentityUser>().ToTable("Usuarios");
            modelBuilder.Entity<ApplicationUser>().ToTable("Usuarios");
            modelBuilder.Entity<IdentityUserRole>().ToTable("RolesUsuario");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("IniciosUsuario");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("PeticionesUsuario");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<ApplicationRole>().ToTable("Roles");
            // Models
            modelBuilder.Entity<ApplicationRole>().HasMany(t => t.Permisos).WithMany(t => t.Roles).Map(m =>
            {
                m.ToTable("Roles_Permisos");
                m.MapLeftKey("RolID");
                m.MapRightKey("PermisoID");
            });

            modelBuilder.Entity<Objetivo>().HasMany(t => t.Estrategias).WithMany(t => t.ObjetivoAlineado).Map(m =>
            {
                m.ToTable("Objetivos_Estrategias");
                m.MapLeftKey("ObjetivoID");
                m.MapRightKey("EstrategiaID");
            });

            //modelBuilder.Entity<Objetivo>().HasMany(t => t.ObjetivosAlineados).WithMany().Map(m =>
            //{
            //    m.ToTable("ObjetivosAlineados");
            //    m.MapLeftKey("ObjetivoID");
            //    m.MapRightKey("ObjetivoAlineadoID");
            //});

            modelBuilder.Entity<Atribucion>().HasMany(t => t.Objetivos).WithMany(p => p.Atribuciones).Map(m => {
                m.ToTable("Atribuciones_Objetivos");
                m.MapLeftKey("AtribucionID");
                m.MapRightKey("ObjetivoID");
            });

            modelBuilder.Entity<Objetivo>().HasMany(t => t.Periodos).WithMany(p => p.Objetivos).Map(m =>
            {
                m.ToTable("Objetivos_Periodos");
                m.MapLeftKey("ObjetivoID");
                m.MapRightKey("PeriodoID");
            });

            modelBuilder.Entity<Estrategia>().HasMany(t => t.Periodos).WithMany(p => p.Estrategias).Map(m =>
            {
                m.ToTable("Estrategias_Periodos");
                m.MapLeftKey("EstrategiaID");
                m.MapRightKey("PeriodoID");
            });

            //modelBuilder.Entity<GrupoPantalla>().HasMany(g => g.Pantallas).WithRequired(p => p.Grupo);

        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}