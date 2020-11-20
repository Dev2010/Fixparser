namespace fix_analyzer
{
    using log4net;
    using log4net.Config;
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Web;
    //using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.UI.WebControls;

    public class MvcApplication : HttpApplication
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception baseException = base.Server.GetLastError().GetBaseException();
            object[] objArray = new object[] { "MESSAGE: ", baseException.Message, "\nSOURCE: ", baseException.Source, "\nFORM: ", base.Request.Form.ToString(), "\nQUERYSTRING: ", base.Request.QueryString.ToString(), "\nTARGETSITE: " };
            objArray[9] = baseException.TargetSite;
            objArray[10] = "\nSTACKTRACE: ";
            objArray[11] = baseException.StackTrace;
            EventLog.WriteEntry("FixParser", string.Concat(objArray), EventLogEntryType.Error);
            Logger.Error("Unhandled Exception", baseException);
        }

        protected void Application_Start()
        {
            XmlConfigurator.Configure();
            Logger.Info("Application Start");
            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            //BundleTable.Bundles.RegisterTemplateBundles();
            new WebFixDataDictionary().GetFixDataDictionary();
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            string name = "DefaultApi";
            //routes.MapHttpRoute(name, "api/{controller}/{id}", new { id = RouteParameter.Optional });
            string str3 = "Default";
            routes.MapRoute(str3, "{controller}/{action}/{id}", new { 
                controller = "Home",
                action = "Index",
                id = UrlParameter.Optional
            });
        }
    }
}

