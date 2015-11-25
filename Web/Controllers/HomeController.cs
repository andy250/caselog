using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using andy250.CaseLog.Core.FileIO;
using andy250.CaseLog.Core.Interfaces;
using andy250.CaseLog.Web.Models;

namespace andy250.CaseLog.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfigProvider config;
        private readonly ILogReader logReader;
        private readonly IFileSystem fileSystem;

        public HomeController(IConfigProvider config, ILogReader logReader, IFileSystem fileSystem)
        {
            this.config = config;
            this.logReader = logReader;
            this.fileSystem = fileSystem;
        }

        public ActionResult Index()
        {
            var model = config.GetHosts();
            return View(model);
        }

        public ActionResult Folders(string host)
        {
            var folders = config.GetHost(host).folders;
            var model = folders.Select(x => new SelectListItem { Value = x.name, Text = x.name });
            return View("_SelectMenu", model);
        }

        public ActionResult Files(string host, string folder)
        {
            var hostInfo = config.GetHost(host);
            var folderInfo = hostInfo.GetFolder(folder);
            var files = fileSystem.GetFiles(folderInfo.path, folderInfo.filter);
            var model = files.Select(x =>
            {
                var fileInfo = new FileInfo(x);
                return new SelectListItem { Value = fileInfo.FullName, Text = fileInfo.Name };
            });
            return View("_SelectMenu", model);
        }

        public ActionResult LogLevels(string host, string folder)
        {
            var hostInfo = config.GetHost(host);
            var folderInfo = hostInfo.GetFolder(folder);

            var model =
                folderInfo.levels != null ?
                    folderInfo.levels.Select(x => { return new SelectListItem { Value = x.name, Text = x.name }; }).ToList() :
                    new List<SelectListItem>();

            model.Insert(0, new SelectListItem { Value = "ALL", Text = "ALL" });
            return View("_SelectMenu", model);
        }

        public ActionResult ReadFile(string host, string folder, string file)
        {
            var hostInfo = config.GetHost(host);
            var folderInfo = hostInfo.GetFolder(folder);

            var source = new FileLogSource(file, folderInfo.linePattern, folderInfo.levels);
            var logs = logReader.ReadFromEnd(source);
            var model = logs.Select(x => new LogEntryModel(x));
            return View("_Log", model);
        }

        public EmptyResult ReloadConfig()
        {
            config.Reload();
            return new EmptyResult();
        }
    }
}