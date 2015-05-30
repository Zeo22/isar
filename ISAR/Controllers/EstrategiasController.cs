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
    public class EstrategiasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private List<Periodo> GetPeriodsByObjetives(Estrategia estrategia)
        {
            List<Periodo> periods = new List<Models.Periodo>();
            IEnumerable<Periodo> res;

            if (estrategia.ObjetivoAlineado == null)
            {
                res = db.Periodos.Where(item => item.Activo);
            }
            else
            {
                res = (from a in estrategia.ObjetivoAlineado
                      from b in a.Periodos
                      select b).Distinct();
            }

            periods.AddRange(res.ToList());

            foreach (Periodo p in db.Periodos.Where(item => item.Activo))
            {
                if (!periods.Exists(item => item.ID == p.ID))
                {
                    periods.Add(p);
                }
            }

            return periods;
        }

        // GET: Estrategias
        public ActionResult Index(string lvl, int? areaId)
        {
            ApplicationUser usuario = (ApplicationUser)db.Users.FirstOrDefault(item => item.UserName == User.Identity.Name);
            int nivel = int.Parse(lvl);
            int periodId = (int)Session["SelectedPeriod"];
            Periodo period = db.Periodos.Find(periodId);
            Area area;

            if (areaId == null)
            {
                if (usuario.TienePermiso(1) || (nivel == 1 && usuario.TienePermiso(3)) || usuario.TieneNivel(1)) // Administrador
                {
                    area = db.Areas.Where(item => item.Nivel.ID == nivel).First();
                }
                else if (usuario.TieneNivel(3) && usuario.TienePermiso(10))
                {
                    area = usuario.UsuarioArea.AreaPadre;
                }
                else
                {
                    area = usuario.UsuarioArea;
                }
            }
            else
            {
                area = db.Areas.Where(item => item.Nivel.ID == nivel).FirstOrDefault(item => item.ID == areaId);
            }
            ViewBag.usuario = usuario;
            ViewBag.CurrentArea = area;
            ViewBag.Nivel = lvl;
            ViewBag.Areas = db.Areas.Where(item => item.Nivel.ID == nivel).ToList();
            ViewBag.PeriodoSeleccionado = period;
            ViewBag.EstrategiasNoAlineadas = db.Estrategias.Where(item => item.ObjetivoAlineado.Count() == 0 && (item.Area == null ? true : item.Area.ID == areaId) && item.Periodos.Any(p => p.ID == periodId)).ToList();
            return View(db.Objetivos.Include("Estrategias").Where(item => item.Area.ID == area.ID && item.Periodos.Any(p => p.ID == periodId)));
        }

        // GET: Estrategias/Details/5
        public ActionResult Details(int? id, string lvl)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estrategia estrategia = db.Estrategias.Find(id);
            if (estrategia == null)
            {
                return HttpNotFound();
            }
            FillViewBag(ViewBag, lvl, null);
            var json_objetivos = from a in estrategia.ObjetivoAlineado
                                 select new
                                 {
                                     id = a.ID,
                                     text = a.Nombre
                                 };
            ViewBag.JSONObjetivos = new JavaScriptSerializer().Serialize(json_objetivos);
            return View(estrategia);
        }

        private void FillViewBag(dynamic ViewBag, string lvl, int? areaId)
        {
            ApplicationUser usuario = (ApplicationUser)db.Users.FirstOrDefault(item => item.UserName == User.Identity.Name);
            int nivel = int.Parse(lvl);

            ViewBag.usuario = usuario;
            ViewBag.Nivel = lvl;
            if (usuario.TienePermiso(1)) // Administrador
            {
                if (areaId != null)
                {
                    ViewBag.Objetivos = db.Objetivos.Where(item => item.Area.ID == areaId).ToList();
                }
                else
                {
                    ViewBag.Objetivos = db.Objetivos.Where(item => item.Area.Nivel.ID == nivel).ToList();
                }
            }
            else
            {
                ViewBag.Objetivos = db.Objetivos.Where(item => item.Area.ID == usuario.UsuarioArea.ID).ToList();
            }
        }

        // GET: Estrategias/Create
        public ActionResult Create(string lvl, int? areaId)
        {
            ViewBag.areaId = areaId;
            FillViewBag(ViewBag, lvl, areaId);
            return View();
        }

        // POST: Estrategias/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Titulo")] Estrategia estrategia, string lvl, string ObjetivosIds, int? areaId)
        {
            if (ModelState.IsValid)
            {
                List<int> Objetivos = new List<int>();

                foreach (string item in ObjetivosIds.Split(','))
                {
                    if (item != "")
                    {
                        Objetivos.Add(int.Parse(item));
                    }
                }
                estrategia.Area = db.Areas.Find(areaId);
                estrategia.ObjetivoAlineado = db.Objetivos.Where(item => Objetivos.Contains(item.ID)).ToList();
                estrategia.Periodos = this.GetPeriodsByObjetives(estrategia);
                db.Estrategias.Add(estrategia);
                db.SaveChanges();
                return RedirectToAction("Index", new { lvl = lvl, areaId = areaId });
            }
            ViewBag.areaId = areaId;
            FillViewBag(ViewBag, lvl, areaId);
            return View(estrategia);
        }

        // GET: Estrategias/Edit/5
        public ActionResult Edit(int? id, string lvl, int? areaId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estrategia estrategia = db.Estrategias.Find(id);
            if (estrategia == null)
            {
                return HttpNotFound();
            }
            ViewBag.areaId = areaId;
            FillViewBag(ViewBag, lvl, areaId);
            var json_objetivos = from a in estrategia.ObjetivoAlineado
                                 select new
                                 {
                                     id = a.ID,
                                     text = a.Nombre,
                                     Eliminar = "<a href=\"javascript: nop(void);\" style=\"color:red\" class=\"fa fa-minus\"></a>"
                                 };
            ViewBag.JSONObjetivos = new JavaScriptSerializer().Serialize(json_objetivos);
            return View(estrategia);
        }

        // POST: Estrategias/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Titulo")] Estrategia estrategia, string lvl, string ObjetivosIds, int? areaId)
        {
            if (ModelState.IsValid)
            {
                List<int> Objetivos = new List<int>();

                foreach (string item in ObjetivosIds.Split(','))
                {
                    if (item != "")
                    {
                        Objetivos.Add(int.Parse(item));
                    }
                }
                var estrategia_tmp = db.Estrategias.Include("ObjetivoAlineado").FirstOrDefault(item => item.ID == estrategia.ID);
                estrategia_tmp.Titulo = estrategia.Titulo;
                estrategia_tmp.Area = db.Areas.Find(areaId);
                estrategia_tmp.ObjetivoAlineado.Clear();
                estrategia_tmp.ObjetivoAlineado = db.Objetivos.Where(item => Objetivos.Contains(item.ID)).ToList();
                //estrategia_tmp.Periodo = db.Periodos.FirstOrDefault(item => item.Activo);
                estrategia_tmp.Periodos.Clear();
                estrategia_tmp.Periodos = this.GetPeriodsByObjetives(estrategia_tmp);
                db.Entry(estrategia_tmp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { lvl = lvl, areaId = areaId });
            }
            ViewBag.areaId = areaId;
            FillViewBag(ViewBag, lvl, areaId);
            return View(estrategia);
        }

        // GET: Estrategias/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estrategia estrategia = db.Estrategias.Find(id);
            if (estrategia == null)
            {
                return HttpNotFound();
            }
            return View(estrategia);
        }

        // POST: Estrategias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, string lvl, int? areaId)
        {
            Estrategia estrategia = db.Estrategias.Find(id);
            db.Estrategias.Remove(estrategia);
            db.SaveChanges();
            return RedirectToAction("Index", new { lvl = lvl, areaId = areaId });
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
