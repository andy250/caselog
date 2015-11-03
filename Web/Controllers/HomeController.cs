using andy250.CaseLog.Core.Configuration;
using System.Web.Mvc;

namespace andy250.CaseLog.Web.Controllers
{
    public class HomeController : Controller
    {
        private IConfigProvider config;

        public HomeController(IConfigProvider config)
        {
            this.config = config;
        }

        public ActionResult Index()
        {
            var model = config.GetHosts();
            return View(model);
        }
    }
}