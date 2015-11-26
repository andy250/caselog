using System;
using System.Collections.Generic;
using System.Linq;

namespace andy250.CaseLog.Core.Configuration
{
    public class CaseLogConfig : BaseConfig
    {
        public HostInfo GetHost(string host)
        {
            return Hosts.SingleOrDefault(x => string.Equals(x.Name, host, StringComparison.OrdinalIgnoreCase));
        }

        public List<HostInfo> Hosts { get; set; }

        public override IEnumerable<BaseConfig> GetChildConfigurations()
        {
            return Hosts;
        }
    }
}
