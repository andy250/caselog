using System;
using System.Collections.Generic;
using System.Linq;
using andy250.CaseLog.Core.Interfaces;

namespace andy250.CaseLog.Core.Configuration
{
    public class CaseLogConfig : BaseConfigWithFolders
    {
        public HostInfo GetHost(string host)
        {
            return Hosts.SingleOrDefault(x => string.Equals(x.Name, host, StringComparison.OrdinalIgnoreCase));
        }

        public List<HostInfo> Hosts { get; set; }

        internal override void ManageFolders(IFileSystem fileSystem)
        {
            if (Hosts != null)
            {
                foreach (var host in Hosts)
                {
                    host.ManageFolders(fileSystem);
                }
            }
        }

        internal override IEnumerable<BaseConfig> GetChildConfigurations()
        {
            return Hosts;
        }
    }
}
