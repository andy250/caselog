using Newtonsoft.Json;
using System;
using System.Configuration;
using andy250.CaseLog.Core.Interfaces;

namespace andy250.CaseLog.Core.Configuration
{
    public class ConfigProvider : IConfigProvider
    {
        private readonly IFileSystem fileSystem;

        public ConfigProvider(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;

            Reload();
        }

        public CaseLogConfig Config { get; private set; }

        public void Reload()
        {
            var path = fileSystem.CombinePath(AppDomain.CurrentDomain.GetData("DataDirectory").ToString(), ConfigurationManager.AppSettings["configName"]);
            var fileContent = fileSystem.ReadTextFile(path);
            Config = JsonConvert.DeserializeObject<CaseLogConfig>(fileContent);
            Config.PropagateRelations(null);
            Config.ManageFolders(fileSystem);
        }
    }
}
