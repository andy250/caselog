using System.Collections.Generic;
using System.IO;
using andy250.CaseLog.Core.Configuration;

namespace andy250.CaseLog.Core.Interfaces
{
    public interface IFileSystem
    {
        List<FileInfo> GetFiles(HostInfo host, FolderInfo directory);
    }
}
