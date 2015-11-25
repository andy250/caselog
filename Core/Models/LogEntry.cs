using System.Collections.Generic;

namespace andy250.CaseLog.Core.Models
{
    public class LogEntry
    {
        public LogEntry()
        {
            Lines = new List<string>();
        }

        public List<string>  Lines { get; set; }
        public string LogLevel { get; set; }

        public void AddLine(string line)
        {
            Lines.Add(line);
        }
    }
}
