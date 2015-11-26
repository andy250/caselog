using System.Collections.Generic;
using System.Text.RegularExpressions;
using andy250.CaseLog.Core.Configuration;
using andy250.CaseLog.Core.Interfaces;

namespace andy250.CaseLog.Core.FileIO
{
    public class FileLogSource : ILogSource
    {
        public FileLogSource(string fullPath, FolderInfo folderInfo)
        {
            FullPath = fullPath;
            OpeningLinePattern = folderInfo.OpeningLinePattern;

            if (folderInfo.Levels != null)
            {
                LogLevels = new Dictionary<string, Regex>();
                foreach (var level in folderInfo.Levels)
                {
                    LogLevels.Add(level.Name, new Regex(level.Regex, GetOptions(level)));
                }
            }
        }

        private RegexOptions GetOptions(LogLevel level)
        {
            var opts = RegexOptions.Compiled;
            if (level.IgnoreCase)
            {
                opts = opts | RegexOptions.IgnoreCase;
            }
            return opts;
        }

        public string FullPath { get; }
        public string OpeningLinePattern { get; }
        public Dictionary<string, Regex> LogLevels { get; }
    }
}
