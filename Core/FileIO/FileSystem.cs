using System.Collections.Generic;
using System.IO;
using System.Linq;
using andy250.CaseLog.Core.Interfaces;

namespace andy250.CaseLog.Core.FileIO
{
    public class FileSystem : IFileSystem
    {
        public List<string> GetFiles(string directory, string filter)
        {
            return 
                (string.IsNullOrWhiteSpace(filter) ?
                    Directory.GetFiles(directory) :
                    Directory.GetFiles(directory,filter)).ToList();
        }
    }
}
