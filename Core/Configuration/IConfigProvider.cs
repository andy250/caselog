using System.Collections.Generic;

namespace andy250.CaseLog.Core.Configuration
{
    public interface IConfigProvider
    {
        List<HostInfo> GetHosts();

        void Reload();
    }
}
