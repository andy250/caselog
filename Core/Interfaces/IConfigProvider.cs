using System.Collections.Generic;
using andy250.CaseLog.Core.Configuration;

namespace andy250.CaseLog.Core.Interfaces
{
    public interface IConfigProvider
    {
        List<HostInfo> GetHosts();
        HostInfo GetHost(string host);

        void Reload();
    }
}
