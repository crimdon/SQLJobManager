using JobManager.Filters;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace JobManager
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            // Register global filter
            GlobalFilters.Filters.Add(new ChromeOnlyAttribute());

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
