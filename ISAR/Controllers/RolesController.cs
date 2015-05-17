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
            ViewBag.Grupos = (from a in db.Permisos
                              where a.Grupo != null
                              select a.Grupo).Distinct();

            ViewBag.Permisos = db.Permisos.ToList();
            return View();
        }

        //
        // POST: /Roles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre")] EditarRol rol, List<int> permisos)
        {
            ApplicationDbContext db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();

            if (ModelState.IsValid)
            {
                var role = new ApplicationRole(rol.Nombre);
                var roleresult = RoleManager.Create(role);
                if (!roleresult.Succeeded)
                {
                    role = RoleManager.FindByName(rol.Nombre);
                    role.Name = rol.Nombre;
                    
                    if (role == null)
                    {
                        ModelState.AddModelError("", roleresult.Errors.First());
                        return View();
                    }
                }

                // TODO: Permisos
                if (role.Permisos != null)
                {
                    role.Permisos.Clear();
                }
                if (permisos != null)
                {
                    

                    permisos.ForEach(item => {
                        Permiso permiso = db.Permisos.FirstOrDefault(p => p.ID == item);

                        if (permiso != null)
                        {
                            if (role.Permisos == null)
                            {
                                role.Permisos = new List<Permiso>();
                            }
                            role.Permisos.Add(permiso);
                        }
                    });
                }
                
                RoleManager.Update(role);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(rol);
        }

        //
        // GET: /Roles/Edit/Admin
        public async Task<ActionResult> Edit(string id)
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
            
            // TODO: Permisos
            ViewBag.Grupos = (from a in db.Permisos
                              where a.Grupo != null
                              select a.Grupo).Distinct();
            ViewBag.Permisos = db.Permisos.ToList();
            EditarRol roleModel = new EditarRol() { Id = role.Id, Nombre = role.Name, Permisos = role.Permisos };
            return View(roleModel);
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
                //db.Permisos.RemoveRange(role.Permisos);
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
                //Migrations.Configuration.SeedPermisos(db);                
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
