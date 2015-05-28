using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Threading;
using System.Globalization;
using ISAR.Models;

namespace ISAR
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Binders
            ModelBinders.Binders.Add(typeof(Indicador), new Binders.IndicadoresModelBinder());
        }

        //protected void Application_BeginRequest(Object sender, EventArgs e)
        //{
        //    CultureInfo newCulture = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();

        //    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-MX");
        //    Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("es");
        //    newCulture.DateTimeFormat.ShortDatePattern = "dd-MMM-yyyy";
        //    newCulture.DateTimeFormat.DateSeparator = "-";
        //    Thread.CurrentThread.CurrentCulture = newCulture;
        //}

        //protected void Application_PreRequestHandlerExecute()
        //{
        //    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-MX");
        //    Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("es");
        //}

        protected void Session_Start()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Periodo period = db.Periodos.FirstOrDefault(item => item.Activo);

            if (period != null)
            {
                Session["selectedPeriod"] = period.ID;
            }
        }
    }
}
