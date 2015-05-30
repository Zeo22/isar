using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISAR.Models;

namespace ISAR.Binders
{
    public class IndicadoresModelBinder : DefaultModelBinder
    {
        private List<Periodo> GetPeriodsBetweenDates(DateTime start, DateTime end, ApplicationDbContext db)
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

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.Model != null)
                return bindingContext.Model;

            var model = base.BindModel(controllerContext, bindingContext);

            var indicador = model as Indicador;
            if (indicador == null)
                return model;

            ApplicationDbContext db = (controllerContext.Controller as Controllers.IndicadoresController).DbContext;

            indicador.Tipo = db.TipoIndicador.Find(indicador.Tipo.ID);
            indicador.Frecuencia = db.FrecuenciaMedicion.Find(indicador.Frecuencia.ID);
            indicador.UnidadDeMedida = db.UnidadesDeMedida.Find(indicador.UnidadDeMedida.ID);
            indicador.Area = db.Areas.Find(indicador.Area.ID);
            indicador.Responsable = db.Users.Find(indicador.Responsable.Id);
            indicador.Comportamiento = db.Comportamiento.Find(indicador.Comportamiento.ID);
            indicador.Periodos = this.GetPeriodsBetweenDates(indicador.FechaInicio, indicador.FechaFinal, db);

            if (indicador.Responsable == null)
            {
                bindingContext.ModelState.Remove("Responsable.Nombre");
                bindingContext.ModelState.AddModelError("Responsable", "El campo Responsable es obligatorio.");
            }
            else
            {
                bindingContext.ModelState.Remove("Responsable.Nombre");
            }
            if (indicador.UnidadDeMedida != null)
            {
                bindingContext.ModelState.Remove("UnidadDeMedida.Nombre");
                bindingContext.ModelState.Remove("UnidadDeMedida.Abreviatura");
            }
            return model;
        }
    }
}