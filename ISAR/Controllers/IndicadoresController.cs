﻿using System;
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
    public class IndicadoresController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ApplicationDbContext DbContext
        {
            get
            {
                return db;
            }
        }

        private void FillViewBagCreate(dynamic ViewBag, string lvl)
        {
            ApplicationUser usuario = (ApplicationUser)db.Users.FirstOrDefault(item => item.UserName == User.Identity.Name);
            int nivel = int.Parse(lvl);

            ViewBag.usuario = usuario;
            ViewBag.Nivel = lvl;
            ViewBag.Unidades = db.UnidadesDeMedida.ToList();
            ViewBag.Comportamiento = db.Comportamiento.ToList();
            ViewBag.Tipo = db.TipoIndicador.Where(item => item.Nivel.ID == nivel).ToList();
            ViewBag.Frecuencia = db.FrecuenciaMedicion.ToList();
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

        // GET: Indicadores
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
                return View(db.Indicadores.Where(item => item.Tipo.Nivel.ID == nivel && item.Periodos.Any(p => p.ID == periodId)).ToList());
            }
            if ((usuario.TieneNivel(2) && nivel == 2) || (usuario.TieneNivel(3) && nivel == 3))
            {
                return View(db.Indicadores.Where(item => item.Tipo.Nivel.ID == nivel && item.Area.ID == usuario.UsuarioArea.ID && item.Periodos.Any(p => p.ID == periodId)).ToList());
            }
            if ((usuario.TieneNivel(3) && usuario.TienePermiso(8) && nivel == 2) || (usuario.TieneNivel(2) && usuario.TienePermiso(5) && nivel == 1))
            {
                return View(db.Indicadores.Where(item => item.Tipo.Nivel.ID == nivel && item.Area.ID == usuario.UsuarioArea.AreaPadre.ID && item.Periodos.Any(p => p.ID == periodId)).ToList());
            }
            return View(db.Indicadores.Where(item => item.Tipo.Nivel.ID == nivel && item.Periodos.Any(p => p.ID == periodId)).ToList());
        }

        // GET: Indicadores/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Indicador indicador = db.Indicadores.Find(id);
            if (indicador == null)
            {
                return HttpNotFound();
            }
            return View(indicador);
        }

        // GET: Indicadores/Create
        public ActionResult Create(string lvl)
        {
            FillViewBagCreate(ViewBag, lvl);
            return View();
        }

        // POST: Indicadores/Create
        // Las validaciones estan en el Binders.IndicadoresModelBinder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Indicador indicador, string lvl)
        {
            if (ModelState.IsValid)
            {
                db.Indicadores.Add(indicador);
                db.SaveChanges();
                return RedirectToAction("Index", new { lvl = lvl });
            }
            FillViewBagCreate(ViewBag, lvl);
            return View(indicador);
        }

        // GET: Indicadores/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Indicador indicador = db.Indicadores.Find(id);
            if (indicador == null)
            {
                return HttpNotFound();
            }
            return View(indicador);
        }

        // POST: Indicadores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nombre,Descripcion,FechaInicio,Formula,FuenteInforacion")] Indicador indicador)
        {
            if (ModelState.IsValid)
            {
                db.Entry(indicador).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(indicador);
        }

        // GET: Indicadores/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Indicador indicador = db.Indicadores.Find(id);
            if (indicador == null)
            {
                return HttpNotFound();
            }
            return View(indicador);
        }

        // POST: Indicadores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Indicador indicador = db.Indicadores.Find(id);
            db.Indicadores.Remove(indicador);
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
