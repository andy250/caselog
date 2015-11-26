using System.Collections.Generic;
using andy250.CaseLog.Core.Configuration;

namespace andy250.CaseLog.Web.Models
{
    public class LogInspectorModel
    {
        public IEnumerable<HostInfo> Hosts { get; set; }
        public string SelectedHost { get; set; }
        public string SelectedFolder { get; set; }
    }
}