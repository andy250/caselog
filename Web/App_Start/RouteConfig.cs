using System.Web.Mvc;
using System.Web.Routing;

namespace andy250.CaseLog.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Inspector",
                url: "show/{hostName}/{folderName}",
                defaults: new { controller = "Home", action = "Inspector", hostName = UrlParameter.Optional, folderName = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Home", action = "Dashboard" }
            );
        }
    }
}
