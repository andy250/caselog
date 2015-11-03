using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace andy250.CaseLog.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
