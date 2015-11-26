using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using andy250.CaseLog.Core.FileIO;
using andy250.CaseLog.Core.Interfaces;
using andy250.CaseLog.Web.Models;

namespace andy250.CaseLog.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfigProvider configProvider;
        private readonly ILogReader logReader;
        private readonly IFileSystem fileSystem;

        public HomeController(IConfigProvider configProvider, ILogReader logReader, IFileSystem fileSystem)
        {
            this.configProvider = configProvider;
            this.logReader = logReader;
            this.fileSystem = fileSystem;
        }

        public ViewResult Inspector(string hostName, string folderName)
        {
            var model = new LogInspectorModel();
            model.Hosts = configProvider.Config.Hosts;
            model.SelectedHost = hostName;
            model.SelectedFolder = folderName;
            return View(model);
        }

        public ViewResult Dashboard()
        {
            var model = new DashboardModel();
            model.Hosts = configProvider.Config.Hosts;
            return View(model);
        }

        public ActionResult Folders(string host, string selectedFolder)
        {
            var folders = configProvider.Config.GetHost(host).Folders;
            var model = folders.Select(x =>
                new SelectListItem
                {
                    Value = x.Name,
                    Text = x.Name,
                    Selected = selectedFolder != null && string.Equals(selectedFolder, x.Name, StringComparison.OrdinalIgnoreCase)
                });
            return View("_SelectMenu", model);
        }

        public ActionResult Files(string host, string folder)
        {
            var hostInfo = configProvider.Config.GetHost(host);
            var folderInfo = hostInfo.GetFolder(folder);
            var files = fileSystem.GetFiles(hostInfo, folderInfo);
            var model = files.OrderByDescending(x => x.LastWriteTime).Select(x => new SelectListItem { Value = x.FullName, Text = x.Name });

            return View("_SelectMenu", model);
        }

        public ActionResult LogLevels(string host, string folder)
        {
            var hostInfo = configProvider.Config.GetHost(host);
            var folderInfo = hostInfo.GetFolder(folder);

            var model = new List<SelectListItem> { new SelectListItem { Value = "ALL", Text = "ALL" } };
            if (folderInfo.Levels != null)
            {
                model.AddRange(folderInfo.Levels.Select(x => new SelectListItem { Value = x.Name, Text = x.Name }));
            }

            return View("_SelectMenu", model);
        }

        public ActionResult ReadFile(string host, string folder, string file)
        {
            var folderInfo = configProvider.Config.GetHost(host).GetFolder(folder);

            var source = new FileLogSource(file, folderInfo);
            var logs = logReader.ReadFromEnd(source);
            var model = logs.Select(x => new LogEntryModel(x));

            return View("_Logs", model);
        }

        public EmptyResult ReloadConfig()
        {
            configProvider.Reload();
            return new EmptyResult();
        }
    }
}