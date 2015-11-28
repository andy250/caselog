using System.Collections.Generic;
using System.IO;
using System.Linq;
using andy250.CaseLog.Core.Configuration;
using andy250.CaseLog.Core.Interfaces;

namespace andy250.CaseLog.Core.FileIO
{
    public class FileSystem : IFileSystem
    {
        public List<DirectoryInfo> GetFolders(HostInfo host, FolderInfo directory)
        {
            if (!string.IsNullOrWhiteSpace(directory.SubfolderSearchPattern))
            {
                var directoryInfo = GetDirectoryInfo(host, directory);
                if (directoryInfo.Exists)
                {
                    return directoryInfo.GetDirectories(directory.SubfolderSearchPattern).ToList();
                }
            }
            return new List<DirectoryInfo>();
        }

        public bool Exists(HostInfo host, FolderInfo directory)
        {
            return GetDirectoryInfo(host, directory).Exists;
        }

        public List<FileInfo> GetFiles(HostInfo host, FolderInfo directory)
        {
            var directoryInfo = GetDirectoryInfo(host, directory);
            if (directoryInfo.Exists)
            {
                return directoryInfo.GetFiles(directory.Filter ?? "*").ToList();
            }
            return new List<FileInfo>();
        }

        public string ReadTextFile(string path)
        {
            return File.ReadAllText(path);
        }

        public string CombinePath(params string[] paths)
        {
            return Path.Combine(paths);
        }

        private DirectoryInfo GetDirectoryInfo(HostInfo host, FolderInfo directory)
        {
            string directoryPath = GetDirectoryPath(host, directory);
            return new DirectoryInfo(directoryPath);
        }

        private string GetDirectoryPath(HostInfo host, FolderInfo directory)
        {
            return directory.Absolute ? directory.Path : GetUncDirectory(host.Unc, directory.Path);
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
