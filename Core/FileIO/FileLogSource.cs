using System.Collections.Generic;
using System.Text.RegularExpressions;
using andy250.CaseLog.Core.Interfaces;
using andy250.CaseLog.Core.Models;

namespace andy250.CaseLog.Core.FileIO
{
    public class FileLogSource : ILogSource
    {
        public FileLogSource(string path, string logLinePattern, List<LogLevel> logLevels)
        {
            Path = path;
            LogLinePattern = logLinePattern;

            if (logLevels != null)
            {
                LogLevels = new Dictionary<string, Regex>();
                foreach (var level in logLevels)
                {
                    LogLevels.Add(level.name, new Regex(level.regex, RegexOptions.Compiled | RegexOptions.IgnoreCase));
                }
            }
        }

        public string Path { get; }
        public string LogLinePattern { get; }
        public Dictionary<string, Regex> LogLevels { get; }
    }
}
