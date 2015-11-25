using System.Collections.Generic;

namespace andy250.CaseLog.Core.Interfaces
{
    public interface IFileSystem
    {
        List<string> GetFiles(string directory, string filter);
    }
}