using System.Collections.Generic;
using System.Linq;
using andy250.CaseLog.Core.Interfaces;

namespace andy250.CaseLog.Core.Configuration
{
    public abstract class BaseConfig
    {
        public string Filter { get; set; }

        public string OpeningLinePattern { get; set; }

        public List<LogLevel> Levels { get; set; }
        public bool OverwriteWithParentLevels { get; set; }

        internal abstract IEnumerable<BaseConfig> GetChildConfigurations();

        internal virtual void PropagateRelations(BaseConfig parent)
        {
            if (parent != null)
            {
                if (Filter == null)
                {
                    Filter = parent.Filter;
                }

                if (OpeningLinePattern == null)
                {
                    OpeningLinePattern = parent.OpeningLinePattern;
                }

                if (parent.Levels != null)
                {
                    if (Levels == null || OverwriteWithParentLevels)
                    {
                        Levels = parent.Levels.Select(x => x.Copy()).ToList();
                    }
                    else
                    {
                        Levels.AddRange(parent.Levels);
                    }
                }
            }

            var childConfigurations = GetChildConfigurations();
            if (childConfigurations != null)
            {
                foreach (var childConfiguration in childConfigurations)
                {
                    childConfiguration.PropagateRelations(this);
                }
            }
        }

        internal T Copy<T>()
        {
            return (T) MemberwiseClone();
        }

        internal virtual void ManageFolders(IFileSystem fileSystem)
        {
        }
    }
}