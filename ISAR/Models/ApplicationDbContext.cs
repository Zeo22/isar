using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ISAR.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        #region Models

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
        public DbSet<GrupoPantalla> GrupoPantalla { get; set; }
        public DbSet<Pantalla> Pantallas { get; set; }

        #endregion
        public ApplicationDbContext()
            : base("DatabaseContext", throwIfV1Schema: false)
        {
        }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ASP.NET Identity
            //modelBuilder.Entity<IdentityUser>().ToTable("Usuarios");
            modelBuilder.Entity<ApplicationUser>().ToTable("Usuarios");
            modelBuilder.Entity<IdentityUserRole>().ToTable("RolesUsuario");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("IniciosUsuario");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("PeticionesUsuario");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<ApplicationRole>().ToTable("Roles");
            // Models
            //modelBuilder.Entity<ApplicationRole>().HasMany(t => t.Permisos).WithMany(t => t.Roles).Map(m => {
            //    m.ToTable("Roles_Permisos");
            //    m.MapLeftKey("RolID");
            //    m.MapRightKey("PermisoID");
            //});

            //modelBuilder.Entity<GrupoPantalla>().HasMany(g => g.Pantallas).WithRequired(p => p.Grupo);

        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}