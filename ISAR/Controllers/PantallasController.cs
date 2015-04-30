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
    public class PantallasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Pantallas
        public ActionResult Index()
        {
            return View(db.Pantallas.ToList());
        }
        
        // GET: Pantallas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pantalla pantalla = db.Pantallas.Find(id);
            if (pantalla == null)
            {
                return HttpNotFound();
            }
            return View(pantalla);
        }

        // GET: Pantallas/Create
        public ActionResult Create()
        {
            ViewBag.Grupos = db.GrupoPantalla.ToList();
            return View();
        }

        // POST: Pantallas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,URL,Nombre")] Pantalla pantalla, int Grupo)
        {
            if (ModelState.IsValid)
            {
                pantalla.Grupo = db.GrupoPantalla.Find(Grupo);
                db.Pantallas.Add(pantalla);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pantalla);
        }

        // GET: Pantallas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pantalla pantalla = db.Pantallas.Find(id);
            if (pantalla == null)
            {
                return HttpNotFound();
            }
            ViewBag.Grupos = db.GrupoPantalla.ToList();
            return View(pantalla);
        }

        // POST: Pantallas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,URL,Nombre")] Pantalla pantalla, int Grupo)
        {
            if (ModelState.IsValid)
            {
                pantalla.Grupo = db.GrupoPantalla.Find(Grupo);
                db.Entry(pantalla).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pantalla);
        }

        // GET: Pantallas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pantalla pantalla = db.Pantallas.Find(id);
            if (pantalla == null)
            {
                return HttpNotFound();
            }
            return View(pantalla);
        }

        // POST: Pantallas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pantalla pantalla = db.Pantallas.Find(id);
            db.Pantallas.Remove(pantalla);
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
