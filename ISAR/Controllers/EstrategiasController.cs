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

        // GET: Estrategias
        public ActionResult Index(string lvl, int? areaId)
        {
            ApplicationUser usuario = (ApplicationUser)db.Users.FirstOrDefault(item => item.UserName == User.Identity.Name);
            int nivel = int.Parse(lvl);
            Area area;

            if (areaId == null)
            {
                if (usuario.TienePermiso(1)) // Administrador
                {
                    area = db.Areas.Where(item => item.Nivel.ID == nivel).First();
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
            ViewBag.EstrategiasNoAlineadas = db.Estrategias.Where(item => item.ObjetivoAlineado.Count() == 0).ToList();
            return View(db.Objetivos.Include("Estrategias").Where(item => item.Area.ID == area.ID));
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
            FillViewBag(ViewBag, lvl);
            var json_objetivos = from a in estrategia.ObjetivoAlineado
                                 select new
                                 {
                                     id = a.ID,
                                     text = a.Nombre
                                 };
            ViewBag.JSONObjetivos = new JavaScriptSerializer().Serialize(json_objetivos);
            return View(estrategia);
        }

        private void FillViewBag(dynamic ViewBag, string lvl)
        {
            ApplicationUser usuario = (ApplicationUser)db.Users.FirstOrDefault(item => item.UserName == User.Identity.Name);
            int nivel = int.Parse(lvl);

            ViewBag.usuario = usuario;
            ViewBag.Nivel = lvl;
            if (usuario.TienePermiso(1)) // Administrador
            {
                ViewBag.Objetivos = db.Objetivos.Where(item => item.Area.Nivel.ID == nivel).ToList();
            }
            else
            {
                ViewBag.Objetivos = db.Objetivos.Where(item => item.Area.ID == usuario.UsuarioArea.ID).ToList();
            }
        }

        // GET: Estrategias/Create
        public ActionResult Create(string lvl)
        {
            FillViewBag(ViewBag, lvl);
            return View();
        }

        // POST: Estrategias/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Titulo")] Estrategia estrategia, string lvl, string ObjetivosIds)
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
                estrategia.ObjetivoAlineado = db.Objetivos.Where(item => Objetivos.Contains(item.ID)).ToList();
                estrategia.Periodo = db.Periodos.FirstOrDefault(item => item.Activo && (DateTime.Now >= item.FechaInicio && DateTime.Now <= item.FechaFin));
                db.Estrategias.Add(estrategia);
                db.SaveChanges();
                return RedirectToAction("Index", new { lvl = lvl });
            }
            FillViewBag(ViewBag, lvl);
            return View(estrategia);
        }

        // GET: Estrategias/Edit/5
        public ActionResult Edit(int? id, string lvl)
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
            FillViewBag(ViewBag, lvl);
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
        public ActionResult Edit([Bind(Include = "ID,Titulo")] Estrategia estrategia, string lvl, string ObjetivosIds)
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
                estrategia_tmp.ObjetivoAlineado.Clear();
                estrategia_tmp.ObjetivoAlineado = db.Objetivos.Where(item => Objetivos.Contains(item.ID)).ToList();
                //estrategia_tmp.Periodo = db.Periodos.FirstOrDefault(item => item.Activo);
                db.Entry(estrategia_tmp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { lvl = lvl });
            }
            FillViewBag(ViewBag, lvl);
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
        public ActionResult DeleteConfirmed(int id, string lvl)
        {
            Estrategia estrategia = db.Estrategias.Find(id);
            db.Estrategias.Remove(estrategia);
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
