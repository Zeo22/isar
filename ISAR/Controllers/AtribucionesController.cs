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
    public class AtribucionesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Atribuciones
        public ActionResult Index()
        {
            return View(db.Atribuciones.ToList());
        }

        // GET: Atribuciones/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Atribucion atribucion = db.Atribuciones.Find(id);
            if (atribucion == null)
            {
                return HttpNotFound();
            }
            return View(atribucion);
        }

        // GET: Atribuciones/Create
        public ActionResult Create()
        {
            ViewBag.Niveles = db.NivelesOrganizacionales.ToList();
            return View();
        }

        // POST: Atribuciones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Descripcion")] Atribucion atribucion, string Area)
        {
            if (ModelState.IsValid)
            {
                if (Area != null && Area != "-1")
                {
                    int areaId = int.Parse(Area);
                    atribucion.Area = db.Areas.FirstOrDefault(item => item.ID == areaId);
                }
                db.Atribuciones.Add(atribucion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Niveles = db.NivelesOrganizacionales.ToList();
            return View(atribucion);
        }

        // GET: Atribuciones/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Atribucion atribucion = db.Atribuciones.Find(id);
            if (atribucion == null)
            {
                return HttpNotFound();
            }
            ViewBag.Niveles = db.NivelesOrganizacionales.ToList();
            return View(atribucion);
        }

        // POST: Atribuciones/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Descripcion")] Atribucion atribucion, string Area)
        {
            if (ModelState.IsValid)
            {
                atribucion = db.Atribuciones.Find(atribucion.ID);
                if (Area != null && Area != "-1")
                {
                    int areaId = int.Parse(Area);
                    atribucion.Area = db.Areas.FirstOrDefault(item => item.ID == areaId);
                }
                db.Entry(atribucion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Niveles = db.NivelesOrganizacionales.ToList();
            return View(atribucion);
        }

        // GET: Atribuciones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Atribucion atribucion = db.Atribuciones.Find(id);
            if (atribucion == null)
            {
                return HttpNotFound();
            }
            return View(atribucion);
        }

        // POST: Atribuciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Atribucion atribucion = db.Atribuciones.Find(id);
            db.Atribuciones.Remove(atribucion);
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
