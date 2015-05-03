using ISAR.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;

namespace ISAR.Controllers
{
    [Authorize]
    public class RolesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public RolesController()
        {
        }

        public RolesController(ApplicationUserManager userManager,
            ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            set
            {
                _userManager = value;
            }
        }

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        //
        // GET: /Roles/
        public ActionResult Index()
        {
            return View(RoleManager.Roles);
        }

        //
        // GET: /Roles/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var role = await RoleManager.FindByIdAsync(id);
            // Get the list of Users in this Role
            var users = new List<ApplicationUser>();

            // Get the list of Users in this Role
            foreach (var user in UserManager.Users.ToList())
            {
                if (await UserManager.IsInRoleAsync(user.Id, role.Name))
                {
                    users.Add(user);
                }
            }

            ViewBag.Users = users;
            ViewBag.UserCount = users.Count();
            return View(role);
        }

        //
        // GET: /Roles/Create
        public ActionResult Create()
        {
            ViewBag.Grupos = db.GrupoPantalla.Where(item => item.Pantallas.Count > 0).ToList();
            return View();
        }

        //
        // POST: /Roles/Crear
        [HttpPost]
        public async Task<ActionResult> Crear(EditarRol rol)
        {
            if (ModelState.IsValid)
            {
                ApplicationDbContext db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();

                var role = new ApplicationRole(rol.Nombre);
                var roleresult = await RoleManager.CreateAsync(role);
                if (!roleresult.Succeeded)
                {
                    role = await RoleManager.FindByNameAsync(rol.Nombre);
                    if (role == null)
                    {
                        ModelState.AddModelError("", roleresult.Errors.First());
                        return View();
                    }
                }

                if (role.Permisos != null)
                {
                    db.Permisos.RemoveRange(role.Permisos);
                }
                rol.Permisos.ForEach(item =>
                {
                    db.Permisos.Add(new Permiso()
                    {
                        Pantalla = db.Pantallas.FirstOrDefault(pantalla => pantalla.ID == item.PantallaId),
                        Escritura = item.Escritura,
                        Lectura = item.Lectura,
                        Rol = role
                    });
                });

                db.SaveChanges();

                if (Request.IsAjaxRequest())
                {
                    return JavaScript("document.location.replace('" + Url.Action("Index") + "');");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        //
        // GET: /Roles/Edit/Admin
        public async Task<ActionResult> Edit(string id)
        {
            List<EditarPermiso> permisos = new List<EditarPermiso>();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var role = await RoleManager.FindByIdAsync(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            role.Permisos.ForEach(item =>
            {
                permisos.Add(new EditarPermiso()
                {
                    Grupo = item.Pantalla.Grupo.Nombre,
                    Nombre = item.Pantalla.Nombre,
                    Escritura = item.Escritura,
                    Lectura = item.Lectura,
                    PantallaId = item.Pantalla.ID,
                    Eliminar = "<a href=\"#\" style=\"color:red\" class=\"fa fa-minus\"></a>"
                });
            });
            RoleViewModel roleModel = new RoleViewModel { Id = role.Id, Name = role.Name, Permisos = permisos };
            ViewBag.Grupos = db.GrupoPantalla.Where(item => item.Pantallas.Count > 0).ToList();
            return View(roleModel);
        }

        //
        // POST: /Roles/Editar
        [HttpPost]
        public ActionResult Editar(EditarRol rol)
        {
            if (ModelState.IsValid)
            {
                ApplicationDbContext db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
                var role = RoleManager.FindById(rol.Id);

                role.Name = role.Name;
                RoleManager.Update(role);

                db.Permisos.RemoveRange(role.Permisos);
                rol.Permisos.ForEach(item =>
                {
                    db.Permisos.Add(new Permiso()
                    {
                        Pantalla = db.Pantallas.FirstOrDefault(pantalla => pantalla.ID == item.PantallaId),
                        Escritura = item.Escritura,
                        Lectura = item.Lectura,
                        Rol = role
                    });
                });

                db.SaveChanges();

                if (Request.IsAjaxRequest())
                {
                    return JavaScript("document.location.replace('" + Url.Action("Index") + "');");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        //
        // GET: /Roles/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var role = await RoleManager.FindByIdAsync(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        //
        // POST: /Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id, string deleteUser)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ApplicationDbContext db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
                var role = await RoleManager.FindByIdAsync(id);
                if (role == null)
                {
                    return HttpNotFound();
                }
                db.Permisos.RemoveRange(role.Permisos);
                db.SaveChanges();
                IdentityResult result;
                if (deleteUser != null)
                {
                    result = await RoleManager.DeleteAsync(role);
                }
                else
                {
                    result = await RoleManager.DeleteAsync(role);
                }
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
