using System;
using System.Collections.Generic;
using System.Linq;
using andy250.CaseLog.Core.Interfaces;

namespace andy250.CaseLog.Core.Configuration
{
    public class HostInfo : BaseConfigWithFolders
    {
        public FolderInfo GetFolder(string folder)
        {
            return Folders.SingleOrDefault(x => string.Equals(x.Name, folder, StringComparison.OrdinalIgnoreCase));
        }

        public string Name { get; set; }
        public string Unc { get; set; }
        
        internal override void ManageFolders(IFileSystem fileSystem)
        {
            if (Folders != null)
            {
                // helper list to be able to modify Folders property inside the loop
                var iterate = new List<FolderInfo>(Folders);

                foreach (var folder in iterate)
                {
                    if (fileSystem.Exists(this, folder))
                    {
                        var directoryInfos = fileSystem.GetFolders(this, folder);
                        foreach (var directoryInfo in directoryInfos.OrderByDescending(x => x.Name))
                        {
                            var newFolder = new FolderInfo
                            {
                                Absolute = folder.Absolute,
                                Filter = folder.Filter,
                                Levels = new List<LogLevel>(folder.Levels),
                                Name = $"{folder.Name}_{directoryInfo.Name}",
                                Path = fileSystem.CombinePath(folder.Path, directoryInfo.Name),
                                OpeningLinePattern = folder.OpeningLinePattern
                            };

                            Folders.Insert(Folders.IndexOf(folder) + 1, newFolder);
                        }
                    }
                    else
                    {
                        Folders.Remove(folder);
                    }
                }
            }
        }

        internal override IEnumerable<BaseConfig> GetChildConfigurations()
        {
            return Folders;
        }
    }
}