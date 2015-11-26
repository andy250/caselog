using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;
using andy250.CaseLog.Core.Interfaces;

namespace andy250.CaseLog.Core.Configuration
{
    public class ConfigProvider : IConfigProvider
    {
        public ConfigProvider()
        {
            Reload();
        }

        public CaseLogConfig Config { get; private set; }

        public void Reload()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.GetData("DataDirectory").ToString(), ConfigurationManager.AppSettings["configName"]);
            var fileContent = File.ReadAllText(path);
            Config = JsonConvert.DeserializeObject<CaseLogConfig>(fileContent);
            Config.PropagateRelations(null);
        }
    }
}
