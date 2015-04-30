using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ISAR.Models;

namespace ISAR.Controllers
{
    [Authorize]
    public class UnidadDeMedidaController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: UnidadDeMedida
        public ActionResult Index()
        {
            return View(db.UnidadesDeMedida.ToList());
        }

        // GET: UnidadDeMedida/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnidadDeMedida unidadDeMedida = db.UnidadesDeMedida.Find(id);
            if (unidadDeMedida == null)
            {
                return HttpNotFound();
            }
            return View(unidadDeMedida);
        }

        // GET: UnidadDeMedida/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UnidadDeMedida/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Nombre")] UnidadDeMedida unidadDeMedida)
        {
            if (ModelState.IsValid)
            {
                db.UnidadesDeMedida.Add(unidadDeMedida);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(unidadDeMedida);
        }

        // GET: UnidadDeMedida/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnidadDeMedida unidadDeMedida = db.UnidadesDeMedida.Find(id);
            if (unidadDeMedida == null)
            {
                return HttpNotFound();
            }
            return View(unidadDeMedida);
        }

        // POST: UnidadDeMedida/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nombre")] UnidadDeMedida unidadDeMedida)
        {
            if (ModelState.IsValid)
            {
                db.Entry(unidadDeMedida).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(unidadDeMedida);
        }

        // GET: UnidadDeMedida/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnidadDeMedida unidadDeMedida = db.UnidadesDeMedida.Find(id);
            if (unidadDeMedida == null)
            {
                return HttpNotFound();
            }
            return View(unidadDeMedida);
        }

        // POST: UnidadDeMedida/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UnidadDeMedida unidadDeMedida = db.UnidadesDeMedida.Find(id);
            db.UnidadesDeMedida.Remove(unidadDeMedida);
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
