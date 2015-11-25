using System.Collections.Generic;
using andy250.CaseLog.Core.Models;

namespace andy250.CaseLog.Web.Models
{
    public class LogEntryModel
    {
        public LogEntryModel(LogEntry item)
        {
            Lines = item.Lines;
            LogLevel = item.LogLevel;
        }

        public string LogLevel { get; }
        public List<string> Lines { get; }
    }
}