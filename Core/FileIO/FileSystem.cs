using System.Collections.Generic;
using System.IO;
using System.Linq;
using andy250.CaseLog.Core.Configuration;
using andy250.CaseLog.Core.Interfaces;

namespace andy250.CaseLog.Core.FileIO
{
    public class FileSystem : IFileSystem
    {
        public List<FileInfo> GetFiles(HostInfo host, FolderInfo directory)
        {
            string directoryPath = directory.Absolute ? directory.Path : GetUncDirectory(host.Unc, directory.Path);

            var dir = new DirectoryInfo(directoryPath);
            if (dir.Exists)
            {
                return dir.GetFiles(directory.Filter ?? "*").ToList();
            }
            return new List<FileInfo>();
        }

        private string GetUncDirectory(string host, string directory)
        {
            if (!host.StartsWith(@"\\"))
            {
                host = $@"\\{host}";
            }

            return Path.Combine(host, directory);
        }
    }
}
