using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ISAR.Models;
using ISAR.Extensions;

namespace ISAR.Controllers
{
    public class PeriodosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Periodos
        public ActionResult Index()
        {
            return View(db.Periodos.ToList());
        }

        // GET: Periodos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Periodo periodo = db.Periodos.Find(id);
            if (periodo == null)
            {
                return HttpNotFound();
            }
            return View(periodo);
        }

        // GET: Periodos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Periodos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Nombre,FechaInicio,FechaFin,Activo")] Periodo periodo)
        {
            if (ModelState.IsValid)
            {
                if (periodo.Activo)
                {
                    if (db.Periodos.Where(item => item.Activo).ToList().Count() == 2)
                    {
                        ModelState.AddModelError("Activo", "Solo se permite un máximo de 2 periodos activos.");
                        return View(periodo);
                    }
                }
                foreach (Periodo item in db.Periodos.ToList())
                {
                    if (periodo.FechaInicio.IsBetween(item.FechaInicio, item.FechaFin))
                    {
                        ModelState.AddModelError("Traslape", "El periodo tiene traslape de fechas con otro periodo.");
                        return View(periodo);
                    }
                    if (periodo.FechaFin.IsBetween(item.FechaInicio, item.FechaFin))
                    {
                        ModelState.AddModelError("Traslape", "El periodo tiene traslape de fechas con otro periodo.");
                        return View(periodo);
                    }
                }
                db.Periodos.Add(periodo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(periodo);
        }

        // GET: Periodos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Periodo periodo = db.Periodos.Find(id);
            if (periodo == null)
            {
                return HttpNotFound();
            }
            return View(periodo);
        }

        // POST: Periodos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nombre,FechaInicio,FechaFin,Activo")] Periodo periodo)
        {
            if (ModelState.IsValid)
            {
                if (periodo.Activo)
                {
                    if (db.Periodos.Where(item => item.Activo && item.ID != periodo.ID).ToList().Count() == 2)
                    {
                        ModelState.AddModelError("Activo", "Solo se permite un máximo de 2 periodos activos.");
                        return View(periodo);
                    }
                }
                foreach (Periodo item in db.Periodos.Where(item => item.ID != periodo.ID))
                {
                    if (periodo.FechaInicio.IsBetween(item.FechaInicio, item.FechaFin))
                    {
                        ModelState.AddModelError("Traslape", "El periodo tiene traslape de fechas con otro periodo.");
                        return View(periodo);
                    }
                    if (periodo.FechaFin.IsBetween(item.FechaInicio, item.FechaFin))
                    {
                        ModelState.AddModelError("Traslape", "El periodo tiene traslape de fechas con otro periodo.");
                        return View(periodo);
                    }
                }
                db.Entry(periodo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(periodo);
        }

        // GET: Periodos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Periodo periodo = db.Periodos.Find(id);
            if (periodo == null)
            {
                return HttpNotFound();
            }
            return View(periodo);
        }

        // POST: Periodos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Periodo periodo = db.Periodos.Find(id);
            db.Periodos.Remove(periodo);
            db.SaveChanges();
            return RedirectToAction("Index");
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
