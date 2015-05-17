using System.Linq;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.Owin;

namespace ISAR.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage="El campo Nombre es requerido")]
        public string Nombre { get; set; }

        [Display(Name = "Área")]
        public virtual Area UsuarioArea { get; set; }

        public virtual Puesto Puesto { get; set; }

        [Display(Name = "Activo / Inactivo")]
        public bool Activo { get; set; }

        public int AccessCount { get; set; }

        public bool TieneNivel(params int[] Niveles)
        {
            //return this.UsuarioArea.Nivel.ID == Nivel;
            List<int> niveles = new List<int>(Niveles);

            return niveles.Contains(this.UsuarioArea.Nivel.ID);
        }

        public bool TienePermiso(params int[] Permisos)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<int> PermisoId = new List<int>(Permisos);

            foreach (IdentityUserRole item in this.Roles)
            {
                ApplicationRole role = (ApplicationRole)db.Roles.Find(item.RoleId);

                foreach (Permiso p in role.Permisos)
                {
                    if (PermisoId.Contains(p.ID))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public virtual List<Objetivo> Objetivos { get; set; }

        public bool PuedeEliminar()
        {
            return !(this.Objetivos.Count() > 0);
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationRole : IdentityRole
    {
        public virtual List<Permiso> Permisos { get; set; }

        public bool PuedeEliminar()
        {
            return !(this.Users.Count() > 0);
        }

        public ApplicationRole() : base() { }

        public ApplicationRole(string name) : base(name) { }

    }

    // Clases para editar un rol con permisos
    public class EditarRol
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public List<Permiso> Permisos { get; set; }
    }
}