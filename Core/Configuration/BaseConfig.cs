using System.Collections.Generic;

namespace andy250.CaseLog.Core.Configuration
{
    public abstract class BaseConfig
    {
        private string filter;
        private string openingLinePattern;
        private List<LogLevel> levels;
        private List<FolderInfo> folders;

        public BaseConfig ParentConfig { get; set; }

        public string Filter
        {
            get
            {
                if (filter == null && ParentConfig != null)
                {
                    return ParentConfig.Filter;
                }
                return filter;
            }
            set { filter = value; }
        }

        public string OpeningLinePattern
        {
            get
            {
                if (openingLinePattern == null && ParentConfig != null)
                {
                    return ParentConfig.OpeningLinePattern;
                }
                return openingLinePattern;
            }
            set { openingLinePattern = value; }
        }

        public List<LogLevel> Levels
        {
            get
            {
                if (levels == null && ParentConfig != null)
                {
                    return ParentConfig.Levels;
                }
                return levels;
            }
            set { levels = value; }
        }

        public List<FolderInfo> Folders
        {
            get
            {
                if (folders == null && ParentConfig != null)
                {
                    return ParentConfig.Folders;
                }
                return folders;
            }
            set { folders = value; }
        }

        public abstract IEnumerable<BaseConfig> GetChildConfigurations();

        public void PropagateRelations(BaseConfig parent)
        {
            ParentConfig = parent;

            var childConfigurations = GetChildConfigurations();
            if (childConfigurations != null)
            {
                foreach (var childConfiguration in childConfigurations)
                {
                    childConfiguration.PropagateRelations(this);
                }
            }
        }
    }
}