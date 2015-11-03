using System.Collections.Generic;

namespace andy250.CaseLog.Core.Configuration
{
    public class HostInfo
    {
        public string name { get; set; }
        public string unc { get; set; }
        public List<FolderInfo> folders { get; set; }
    }
}