using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace andy250.CaseLog.Core.Configuration
{
    public class ConfigProvider : IConfigProvider
    {
        private List<HostInfo> hosts;

        public ConfigProvider()
        {
            Reload();
        }

        public List<HostInfo> GetHosts()
        {
            return hosts;
        }

        public void Reload()
        {
            hosts = JsonConvert.DeserializeObject<List<HostInfo>>(File.ReadAllText(
                Path.Combine(AppDomain.CurrentDomain.GetData("DataDirectory").ToString(), "config.json")));
        }
    }
}
