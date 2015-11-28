using System.Collections.Generic;
using System.Linq;

namespace andy250.CaseLog.Core.Configuration
{
    public abstract class BaseConfigWithFolders : BaseConfig
    {
        public List<FolderInfo> Folders { get; set; }
        public bool OverwriteWithParentFolders { get; set; }

        internal override void PropagateRelations(BaseConfig parent)
        {
            var parentWithFolders = parent as BaseConfigWithFolders;
            if (parentWithFolders != null && parentWithFolders.Folders != null)
            {
                if (Folders == null || OverwriteWithParentFolders)
                {
                    Folders = parentWithFolders.Folders.Select(x => x.Copy<FolderInfo>()).ToList();
                }
                else
                {
                    Folders.AddRange(parentWithFolders.Folders.Select(x => x.Copy<FolderInfo>()));
                }
            }

            base.PropagateRelations(parent);
        }
    }
}