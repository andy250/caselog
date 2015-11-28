using System.Collections.Generic;

namespace andy250.CaseLog.Core.Configuration
{
    public class FolderInfo : BaseConfig
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public bool Absolute { get; set; }
        public string SubfolderSearchPattern { get; set; }

        internal override IEnumerable<BaseConfig> GetChildConfigurations()
        {
            return null;
        }
    }
}
