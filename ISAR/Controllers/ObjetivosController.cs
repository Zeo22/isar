using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ISAR.Models;
using System.Web.Script.Serialization;

namespace ISAR.Controllers
{
    public class ObjetivosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // Catalogs
        // GET: Objetivos/Atribuciones
        public JsonResult Atribuciones(string search, int AreaId)
        {
            var json = from a in db.Atribuciones
                       where a.Area.ID == AreaId
                       orderby a.Area.ID
                       select new
                       {
                           id = a.ID,
                           text = a.Descripcion,
                           area = a.Area.Nombre
                       };
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        // GET: Objetivos/Objetivos
        public JsonResult Objetivos(string search, int alineacionId, int areaId)
        {
            var json = new object();
            Area area = db.Areas.Find(areaId);
            TipoAlineacion alineación = db.TipoAlineacion.Find(alineacionId);

            if (alineacionId == 1) // Vertical
            {
                json = from a in db.Objetivos
                       where a.Area.ID == area.AreaPadre.ID
                       orderby a.Area.AreaPadre.ID
                       select new
                       {
                           id = a.ID,
                           text = a.Area.Nombre + " - " + a.Nombre,
                           alineacion = alineación.Nombre
                       };
            }
            if (alineacionId == 2) // Matricial
            {
                List<Area> pares = new List<Area>();
                List<int> paresIds = new List<int>();

                //pares = area.AreaPadre.AreaPadre.AreasHijas.Where(item => item.ID != areaId).ToList();
                pares = db.Areas.Where(item => item.Nivel.ID == 2).Where(item => item.ID != area.AreaPadre.ID).ToList();
                pares.ForEach(par =>
                {
                    paresIds.Add(par.ID);
                });
                json = from a in db.Objetivos
                       where paresIds.Contains(a.Area.ID)
                       orderby a.Area.AreaPadre.ID
                       select new
                       {
                           id = a.ID,
                           text = a.Area.Nombre + " - " + a.Nombre,
                           alineacion = alineación.Nombre
                       };
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        // GET: Objetivos
        public ActionResult Index(string lvl)
        {
            ApplicationUser usuario = (ApplicationUser)db.Users.FirstOrDefault(item => item.UserName == User.Identity.Name);
            int nivel;

            if (lvl == null)
            {
                nivel = usuario.UsuarioArea.Nivel.ID;
            }
            else
            {
                nivel = int.Parse(lvl);
            }
            ViewBag.usuario = usuario;
            ViewBag.Nivel = lvl;
            return View(db.Objetivos.Where(item => item.Tipo.ID == nivel).ToList());
        }

        // GET: Objetivos/Details/5
        public ActionResult Details(int? id, string lvl)
        {
            int nivel = int.Parse(lvl);
            Area area;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Objetivo objetivo = db.Objetivos.Find(id);
            if (objetivo == null)
            {
                return HttpNotFound();
            }
            FillViewBagCreate(ViewBag, lvl);
            area = objetivo.Area;
            // Atribuciones
            var json = from a in objetivo.Atribuciones
                       select new
                       {
                           id = a.ID,
                           text = a.Descripcion,
                           area = a.Area.Nombre,
                           Eliminar = "<a href=\"javascript: nop(void);\" style=\"color:red\" class=\"fa fa-minus\"></a>"
                       };
            // Objetivos
            var json_objetivos = from a in objetivo.ObjetivosAlineados
                                 select new
                                 {
                                     id = a.ID,
                                     text = a.Area.Nombre + " - " + a.Nombre,
                                     alineacion = nivel == 1 || nivel == 2 ? "Vertical" : a.Area.ID == area.AreaPadre.ID ? "Vertical" : "Matricial",
                                     Eliminar = "<a href=\"javascript: nop(void);\" style=\"color:red\" class=\"fa fa-minus\"></a>"
                                 };
            ViewBag.JSONAtribuciones = new JavaScriptSerializer().Serialize(json);
            ViewBag.JSONObjetivos = new JavaScriptSerializer().Serialize(json_objetivos);
            return View(objetivo);
        }

        private void FillViewBagCreate(dynamic ViewBag, string lvl)
        {
            ApplicationUser usuario = (ApplicationUser)db.Users.FirstOrDefault(item => item.UserName == User.Identity.Name);
            int nivel = int.Parse(lvl);

            ViewBag.usuario = usuario;
            ViewBag.Nivel = lvl;
            ViewBag.Categorias = db.CategoriaObjetivo.Where(item => item.Nivel.ID == usuario.UsuarioArea.Nivel.ID).ToList();
            // Alineación
            if (nivel == 2)
            {
                ViewBag.Alineacion = db.TipoAlineacion.Where(item => item.ID == 1).ToList(); // Alineación Vertical
            }
            else
            {
                ViewBag.Alineacion = db.TipoAlineacion.ToList();
            }
            // Tipo de Objetivo
            //if (usuario.TienePermiso(1)) // Administrador
            //{
            //    ViewBag.Tipos = db.TipoObjetivo.ToList();
            //}
            //else
            //{
            //    ViewBag.Tipos = db.TipoObjetivo.Where(item => item.ID == usuario.UsuarioArea.Nivel.ID).ToList();
            //}
            ViewBag.Tipos = db.TipoObjetivo.Where(item => item.ID == nivel).ToList();
            // Areas
            if (usuario.TienePermiso(1)) // Administrador
            {
                ViewBag.Areas = db.Areas.Where(item => item.Nivel.ID == nivel).OrderBy(item => item.AreaPadre.ID).ToList();
            }
            else if (usuario.TienePermiso(6) && usuario.TieneNivel(3)) // Lider Unidad Operativa y Nivel Unidad Operativa
            {
                List<Area> Areas = new List<Area>();

                Areas.Add(usuario.UsuarioArea);
                Areas.AddRange(usuario.UsuarioArea.AreasHijas.ToList());
                ViewBag.Areas = Areas.OrderBy(item => item.AreaPadre.ID);
            }
            else
            {
                List<Area> Areas = new List<Area>();

                Areas.Add(usuario.UsuarioArea);
                ViewBag.Areas = Areas;
            }
            // Responsables
            if (usuario.TienePermiso(1)) // Administrador
            {
                ViewBag.Responsables = db.Users.ToList<ApplicationUser>();
            }
            else if (usuario.TienePermiso(6) && usuario.TieneNivel(3)) // Lider Unidad Operativa y Nivel Unidad Operativa
            {
                List<ApplicationUser> Responsables = new List<ApplicationUser>();
                List<Area> Areas = new List<Area>();

                Areas.Add(usuario.UsuarioArea);
                Areas.AddRange(usuario.UsuarioArea.AreasHijas.ToList());
                Responsables = db.Users.Where(user => Areas.Contains(usuario.UsuarioArea)).ToList();
                ViewBag.Responsables = Responsables;
            }
            else
            {
                List<ApplicationUser> Responsables = new List<ApplicationUser>();

                Responsables.Add(usuario);
                ViewBag.Responsables = Responsables;
            }
        }

        // GET: Objetivos/Create
        public ActionResult Create(string lvl)
        {
            FillViewBagCreate(ViewBag, lvl);
            return View();
        }

        // POST: Objetivos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Nombre,Descripcion,FechaInicio,FechaFinal")] Objetivo objetivo, string lvl, int Tipo, int Categoria, int Area, string Responsable, string AtribucionesIds, string ObjetivosIds)
        {
            if (ModelState.IsValid)
            {
                List<int> Atribuciones = new List<int>();
                List<int> Objetivos = new List<int>();

                foreach (string item in AtribucionesIds.Split(','))
                {
                    if (item != "")
                    {
                        Atribuciones.Add(int.Parse(item));
                    }
                }
                foreach (string item in ObjetivosIds.Split(','))
                {
                    if (item != "")
                    {
                        Objetivos.Add(int.Parse(item));
                    }
                }
                objetivo.Tipo = db.TipoObjetivo.Find(Tipo);
                objetivo.Categoria = db.CategoriaObjetivo.Find(Categoria);
                objetivo.Area = db.Areas.Find(Area);
                objetivo.Responsable = db.Users.Find(Responsable);
                objetivo.Atribuciones = db.Atribuciones.Where(item => Atribuciones.Contains(item.ID)).ToList();
                objetivo.ObjetivosAlineados = db.Objetivos.Where(item => Objetivos.Contains(item.ID)).ToList();
                objetivo.Periodo = db.Periodos.FirstOrDefault(item => item.Activo);
                db.Objetivos.Add(objetivo);
                db.SaveChanges();
                return RedirectToAction("Index", new { lvl = lvl });
            }
            FillViewBagCreate(ViewBag, lvl);
            return View(objetivo);
        }

        // GET: Objetivos/Edit/5
        public ActionResult Edit(int? id, string lvl)
        {
            int nivel = int.Parse(lvl);
            Area area;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Objetivo objetivo = db.Objetivos.Include("ObjetivosAlineados").FirstOrDefault(item => item.ID == id);
            if (objetivo == null)
            {
                return HttpNotFound();
            }
            FillViewBagCreate(ViewBag, lvl);
            area = objetivo.Area;
            // Atribuciones
            var json = from a in objetivo.Atribuciones
                       select new
                       {
                           id = a.ID,
                           text = a.Descripcion,
                           area = a.Area.Nombre,
                           Eliminar = "<a href=\"javascript: nop(void);\" style=\"color:red\" class=\"fa fa-minus\"></a>"
                       };
            // Objetivos
            var json_objetivos = from a in objetivo.ObjetivosAlineados
                                 select new
                                 {
                                     id = a.ID,
                                     text = a.Area.Nombre + " - " + a.Nombre,
                                     alineacion = nivel == 1 || nivel == 2 ? "Vertical" : a.Area.ID == area.AreaPadre.ID ? "Vertical" : "Matricial",
                                     Eliminar = "<a href=\"javascript: nop(void);\" style=\"color:red\" class=\"fa fa-minus\"></a>"
                                 };
            ViewBag.JSONAtribuciones = new JavaScriptSerializer().Serialize(json);
            ViewBag.JSONObjetivos = new JavaScriptSerializer().Serialize(json_objetivos);
            return View(objetivo);
        }

        // POST: Objetivos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nombre,Descripcion,FechaInicio,FechaFinal")] Objetivo objetivo, string lvl, int Tipo, int Categoria, int Area, string Responsable, string AtribucionesIds, string ObjetivosIds)
        {
            if (ModelState.IsValid)
            {
                List<int> Atribuciones = new List<int>();
                List<int> Objetivos = new List<int>();

                foreach (string item in AtribucionesIds.Split(','))
                {
                    if (item != "")
                    {
                        Atribuciones.Add(int.Parse(item));
                    }
                }
                foreach (string item in ObjetivosIds.Split(','))
                {
                    if (item != "")
                    {
                        Objetivos.Add(int.Parse(item));
                    }
                }
                var objetivo_tmp = db.Objetivos.Include("ObjetivosAlineados").FirstOrDefault(item => item.ID == objetivo.ID);
                objetivo_tmp.Nombre = objetivo.Nombre;
                objetivo_tmp.Descripcion = objetivo.Descripcion;
                objetivo_tmp.FechaInicio = objetivo.FechaInicio;
                objetivo_tmp.FechaFinal = objetivo.FechaFinal;
                objetivo_tmp.Tipo = db.TipoObjetivo.Find(Tipo);
                objetivo_tmp.Categoria = db.CategoriaObjetivo.Find(Categoria);
                objetivo_tmp.Area = db.Areas.Find(Area);
                objetivo_tmp.Responsable = db.Users.Find(Responsable);
                objetivo_tmp.Atribuciones = db.Atribuciones.Where(item => Atribuciones.Contains(item.ID)).ToList();
                objetivo_tmp.ObjetivosAlineados.Clear();
                objetivo_tmp.ObjetivosAlineados = db.Objetivos.Where(item => Objetivos.Contains(item.ID)).ToList();
                objetivo_tmp.Periodo = db.Periodos.FirstOrDefault(item => item.Activo);
                db.Entry(objetivo_tmp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { lvl = lvl});
            }
            FillViewBagCreate(ViewBag, lvl);
            return View(objetivo);
        }

        // GET: Objetivos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Objetivo objetivo = db.Objetivos.Find(id);
            if (objetivo == null)
            {
                return HttpNotFound();
            }
            return View(objetivo);
        }

        // POST: Objetivos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, string lvl)
        {
            Objetivo objetivo = db.Objetivos.Find(id);
            db.Objetivos.Remove(objetivo);
            db.SaveChanges();
            return RedirectToAction("Index", new { lvl = lvl });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
