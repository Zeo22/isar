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

        private List<Periodo> GetPeriodsBetweenDates(DateTime start, DateTime end)
        {
            List<Periodo> periods = new List<Models.Periodo>();
            var result = from a in db.Periodos
                         where (a.FechaInicio >= start && a.FechaInicio <= end)
                                || (a.FechaFin >= start && a.FechaFin <= end)
                                || (start >= a.FechaInicio && start <= a.FechaFin)
                                || (end >= a.FechaInicio && end <= a.FechaFin)
                         select a;
            var pActive = db.Periodos.FirstOrDefault(item => item.Activo);

            foreach (Periodo p in result)
            {
                if (p.FechaFin >= pActive.FechaFin)
                {
                    periods.Add(p);
                }
            }

            //foreach (Periodo p in db.Periodos.Where(item => item.Activo))
            //{
            //    if (!periods.Exists(item => item.ID == p.ID))
            //    {
            //        periods.Add(p);
            //    }
            //}

            return periods;
        }

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
        public JsonResult Objetivos(string search, int alineacionId, int? areaId)
        {
            var json = new object();
            Area area = db.Areas.Find(areaId);
            TipoAlineacion alineación = db.TipoAlineacion.Find(alineacionId);

            if (areaId == null)
            {
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
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
                //List<Area> pares = new List<Area>();
                //List<int> paresIds = new List<int>();

                ////pares = area.AreaPadre.AreaPadre.AreasHijas.Where(item => item.ID != areaId).ToList();
                //pares = db.Areas.Where(item => item.Nivel.ID == 2).Where(item => item.ID != area.AreaPadre.ID).ToList();
                //pares.ForEach(par =>
                //{
                //    paresIds.Add(par.ID);
                //});
                json = from a in db.Objetivos
                       where a.Area.ID == area.ID
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

        // GET: Objetivos/Estrategias
        public JsonResult Estrategias(string search, int? objetivoId, int? alineacionId)
        {
            var json = new object();
            TipoAlineacion alineación = db.TipoAlineacion.Find(alineacionId);

            json = from a in db.Estrategias
                   from b in a.ObjetivoAlineado
                   where b.ID == objetivoId
                   select new
                   {
                       id = a.ID,
                       text = a.Titulo,
                       objetivo = b.Nombre,
                       alineacion = alineación.Nombre
                   };
            
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        // GET: Objetivos/Areas
        public JsonResult Areas(string search)
        {
            var json = new object();
            ApplicationUser usuario = (ApplicationUser)db.Users.FirstOrDefault(item => item.UserName == User.Identity.Name);
            int areaId = 0;

            if (usuario.UsuarioArea.AreaPadre != null)
            {
                areaId = usuario.UsuarioArea.AreaPadre.ID;
            }
            json = from a in db.Areas
                   where a.ID != areaId && a.Nivel.ID == 2
                   select new
                   {
                       id = a.ID,
                       text = a.Nombre
                   };

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        private void FillViewBagCreate(dynamic ViewBag, string lvl)
        {
            ApplicationUser usuario = (ApplicationUser)db.Users.FirstOrDefault(item => item.UserName == User.Identity.Name);
            int nivel = int.Parse(lvl);

            ViewBag.usuario = usuario;
            ViewBag.Nivel = lvl;
            ViewBag.Categorias = db.CategoriaObjetivo.Where(item => item.Nivel.ID == nivel).ToList();
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
                IEnumerable<int> AreasIds = new List<int>();

                Areas.Add(usuario.UsuarioArea);
                Areas.AddRange(usuario.UsuarioArea.AreasHijas.ToList());
                AreasIds = from a in Areas
                           select a.ID;

                Responsables = db.Users.Where(user => AreasIds.Contains(user.UsuarioArea.ID)).ToList();
                ViewBag.Responsables = Responsables;
            }
            else
            {
                List<ApplicationUser> Responsables = new List<ApplicationUser>();

                Responsables.Add(usuario);
                ViewBag.Responsables = Responsables;
            }
        }

        // GET: Objetivos
        public ActionResult Index(string lvl)
        {
            ApplicationUser usuario = (ApplicationUser)db.Users.FirstOrDefault(item => item.UserName == User.Identity.Name);
            int periodId = (int)Session["SelectedPeriod"];
            Periodo period = db.Periodos.Find(periodId);
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
            ViewBag.PeriodoSeleccionado = period;
            if (usuario.TieneNivel(1) || usuario.TienePermiso(1))
            {
                return View(db.Objetivos.Where(item => item.Tipo.ID == nivel && item.Periodos.Any(p => p.ID == periodId)).ToList());
            }
            if ((usuario.TieneNivel(2) && nivel == 2) || (usuario.TieneNivel(3) && nivel == 3))
            {
                return View(db.Objetivos.Where(item => item.Tipo.ID == nivel && item.Area.ID == usuario.UsuarioArea.ID && item.Periodos.Any(p => p.ID == periodId)).ToList());
            }
            if ((usuario.TieneNivel(3) && usuario.TienePermiso(8) && nivel == 2) || (usuario.TieneNivel(2) && usuario.TienePermiso(5) && nivel == 1))
            {
                return View(db.Objetivos.Where(item => item.Tipo.ID == nivel && item.Area.ID == usuario.UsuarioArea.AreaPadre.ID && item.Periodos.Any(p => p.ID == periodId)).ToList());
            }
            return View(db.Objetivos.Where(item => item.Tipo.ID == nivel && item.Periodos.Any(p => p.ID == periodId)).ToList());
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
            //var json_objetivos = from a in objetivo.ObjetivosAlineados
            //                     select new
            //                     {
            //                         id = a.ID,
            //                         text = a.Area.Nombre + " - " + a.Nombre,
            //                         alineacion = nivel == 1 || nivel == 2 ? "Vertical" : a.Area.ID == area.AreaPadre.ID ? "Vertical" : "Matricial",
            //                         Eliminar = "<a href=\"javascript: nop(void);\" style=\"color:red\" class=\"fa fa-minus\"></a>"
            //                     };
            ViewBag.JSONAtribuciones = new JavaScriptSerializer().Serialize(json);
            //ViewBag.JSONObjetivos = new JavaScriptSerializer().Serialize(json_objetivos);
            return View(objetivo);
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
        public ActionResult Create([Bind(Include = "ID,Nombre,Descripcion,FechaInicio,FechaFinal")] Objetivo objetivo, string lvl, int Tipo, int Categoria, int Area, string Responsable, string AtribucionesIds, string EstrategiasIds)
        {
            if (ModelState.IsValid)
            {
                List<int> Atribuciones = new List<int>();
                List<int> Estrategias = new List<int>();

                if (Responsable == "-1")
                {
                    ModelState.AddModelError("Responsable", "El campo Responsable es obligatorio.");
                    FillViewBagCreate(ViewBag, lvl);
                    return View(objetivo);
                }
                foreach (string item in AtribucionesIds.Split(','))
                {
                    if (item != "")
                    {
                        Atribuciones.Add(int.Parse(item));
                    }
                }
                foreach (string item in EstrategiasIds.Split(','))
                {
                    if (item != "")
                    {
                        Estrategias.Add(int.Parse(item));
                    }
                }
                objetivo.Tipo = db.TipoObjetivo.Find(Tipo);
                objetivo.Categoria = db.CategoriaObjetivo.Find(Categoria);
                objetivo.Area = db.Areas.Find(Area);
                objetivo.Responsable = db.Users.Find(Responsable);
                objetivo.Atribuciones = db.Atribuciones.Where(item => Atribuciones.Contains(item.ID)).ToList();
                //objetivo.ObjetivosAlineados = db.Objetivos.Where(item => Objetivos.Contains(item.ID)).ToList();
                objetivo.Estrategias = db.Estrategias.Where(item => Estrategias.Contains(item.ID)).ToList();
                objetivo.Periodos = this.GetPeriodsBetweenDates(objetivo.FechaInicio, objetivo.FechaFinal);
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
            Objetivo objetivo = db.Objetivos.Include("Estrategias").FirstOrDefault(item => item.ID == id);
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
            // Estrategias
            var json_estrategias = from a in objetivo.Estrategias
                                   select new
                                   {
                                       id = a.ID,
                                       text = a.Titulo,
                                       alineacion = nivel == 1 || nivel == 2 ? "Vertical" : a.ObjetivoAlineado.Any(item=>item.Area.ID == area.AreaPadre.ID && item.ID != objetivo.ID)  ? "Vertical" : "Matricial",
                                       Eliminar = "<a href=\"javascript: nop(void);\" style=\"color:red\" class=\"fa fa-minus\"></a>"
                                   };
            //var json_objetivos = from a in objetivo.ObjetivosAlineados
            //                     select new
            //                     {
            //                         id = a.ID,
            //                         text = a.Area.Nombre + " - " + a.Nombre,
            //                         alineacion = nivel == 1 || nivel == 2 ? "Vertical" : a.Area.ID == area.AreaPadre.ID ? "Vertical" : "Matricial",
            //                         Eliminar = "<a href=\"javascript: nop(void);\" style=\"color:red\" class=\"fa fa-minus\"></a>"
            //                     };
            ViewBag.JSONAtribuciones = new JavaScriptSerializer().Serialize(json);
            ViewBag.JSONEstrategias = new JavaScriptSerializer().Serialize(json_estrategias);
            return View(objetivo);
        }

        // POST: Objetivos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nombre,Descripcion,FechaInicio,FechaFinal")] Objetivo objetivo, string lvl, int Tipo, int Categoria, int Area, string Responsable, string AtribucionesIds, string EstrategiasIds)
        {
            if (ModelState.IsValid)
            {
                List<int> Atribuciones = new List<int>();
                List<int> Estrategias = new List<int>();

                foreach (string item in AtribucionesIds.Split(','))
                {
                    if (item != "")
                    {
                        Atribuciones.Add(int.Parse(item));
                    }
                }
                foreach (string item in EstrategiasIds.Split(','))
                {
                    if (item != "")
                    {
                        Estrategias.Add(int.Parse(item));
                    }
                }
                var objetivo_tmp = db.Objetivos.Include("Estrategias").FirstOrDefault(item => item.ID == objetivo.ID);
                objetivo_tmp.Nombre = objetivo.Nombre;
                objetivo_tmp.Descripcion = objetivo.Descripcion;
                objetivo_tmp.FechaInicio = objetivo.FechaInicio;
                objetivo_tmp.FechaFinal = objetivo.FechaFinal;
                objetivo_tmp.Tipo = db.TipoObjetivo.Find(Tipo);
                objetivo_tmp.Categoria = db.CategoriaObjetivo.Find(Categoria);
                objetivo_tmp.Area = db.Areas.Find(Area);
                objetivo_tmp.Responsable = db.Users.Find(Responsable);
                objetivo_tmp.Atribuciones.Clear();
                objetivo_tmp.Atribuciones = db.Atribuciones.Where(item => Atribuciones.Contains(item.ID)).ToList();
                //objetivo_tmp.ObjetivosAlineados.Clear();
                //objetivo_tmp.ObjetivosAlineados = db.Objetivos.Where(item => Objetivos.Contains(item.ID)).ToList();
                if (lvl != "1")
                {
                    objetivo_tmp.Estrategias.Clear();
                    objetivo_tmp.Estrategias = db.Estrategias.Where(item => Estrategias.Contains(item.ID)).ToList();
                }
                objetivo_tmp.Periodos.Clear();
                objetivo_tmp.Periodos = this.GetPeriodsBetweenDates(objetivo.FechaInicio, objetivo.FechaFinal);
                db.Entry(objetivo_tmp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { lvl = lvl});
            }
            FillViewBagCreate(ViewBag, lvl);
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
