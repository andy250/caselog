using System.Collections.Generic;
using System.Text.RegularExpressions;
using andy250.CaseLog.Core.FileIO;
using andy250.CaseLog.Core.Models;

namespace andy250.CaseLog.Core.LogIO
{
    internal class FileLogReader
    {
        private readonly FileLogSource logSource;
        private readonly Regex regex;

        internal FileLogReader(FileLogSource logSource)
        {
            this.logSource = logSource;
            if (!string.IsNullOrWhiteSpace(logSource.LogLinePattern))
            {
                regex = new Regex(logSource.LogLinePattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            }
        }

        internal List<LogEntry> ReadFromEnd(int amountOfLogs)
        {
            // TODO: allow to read from any position in file (to continue reading backwards)
            List<string> lines;
            using (var fr = new FileReverseReader(logSource.Path))
            {
                lines = fr.Read(amountOfLogs);
                lines.Reverse();
            }

            // TODO: read backwards continuously to get at least amountOfLogs or EOF
            return ConvertToLogs(lines);
        }

        private List<LogEntry> ConvertToLogs(List<string> lines)
        {
            var result = new List<LogEntry>();

            LogEntry currentEntry = null;

            for (int index = 0; index < lines.Count; index++)
            {
                if (index == 0 || IsNewEntry(lines[index], lines[index - 1]))
                {
                    currentEntry = new LogEntry();
                    currentEntry.LogLevel = GetLogLevel(lines[index]);
                    result.Add(currentEntry);
                }
                currentEntry.AddLine(lines[index]);
            }
            
            return result;
        }

        private string GetLogLevel(string line)
        {
            if (logSource.LogLevels != null)
            {
                foreach (var level in logSource.LogLevels)
                {
                    if (level.Value.IsMatch(line))
                    {
                        return level.Key;
                    }
                }
            }

            return null;
        }

        private bool IsNewEntry(string nextLine, string prevLine)
        {
            if (regex != null)
            {
                return regex.IsMatch(nextLine);
            }
            else
            {
                return true;
            }
        }
    }
}
