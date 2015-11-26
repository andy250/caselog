using System.Collections.Generic;
using andy250.CaseLog.Core.Configuration;

namespace andy250.CaseLog.Web.Models
{
    public class DashboardModel
    {
        public IEnumerable<HostInfo> Hosts { get; set; }
    }
}