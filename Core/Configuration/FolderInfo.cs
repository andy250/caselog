using System.Collections.Generic;
using andy250.CaseLog.Core.Models;

namespace andy250.CaseLog.Core.Configuration
{
    public class FolderInfo
    {
        public string name { get; set; }
        public string path { get; set; }
        public string filter { get; set; }
        public string linePattern { get; set; }
        public List<LogLevel> levels { get; set; } 
    }
}
