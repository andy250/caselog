using System;
using System.Collections.Generic;
using System.Linq;

namespace andy250.CaseLog.Core.Configuration
{
    public class HostInfo
    {
        public FolderInfo GetFolder(string folder)
        {
            return folders.SingleOrDefault(x => string.Equals(x.name, folder, StringComparison.OrdinalIgnoreCase));
        }

        public string name { get; set; }
        public List<FolderInfo> folders { get; set; }
    }
}