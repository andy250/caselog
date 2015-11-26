using System;
using System.Collections.Generic;
using System.Linq;

namespace andy250.CaseLog.Core.Configuration
{
    public class HostInfo : BaseConfig
    {
        public FolderInfo GetFolder(string folder)
        {
            return Folders.SingleOrDefault(x => string.Equals(x.Name, folder, StringComparison.OrdinalIgnoreCase));
        }

        public string Name { get; set; }
        public string Unc { get; set; }
        public List<FolderInfo> Folders { get; set; }

        public override IEnumerable<BaseConfig> GetChildConfigurations()
        {
            return Folders;
        }
    }
}