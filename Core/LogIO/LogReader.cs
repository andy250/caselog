using System.Collections.Generic;
using andy250.CaseLog.Core.FileIO;
using andy250.CaseLog.Core.Interfaces;
using andy250.CaseLog.Core.Models;

namespace andy250.CaseLog.Core.LogIO
{
    public class LogReader : ILogReader
    {
        public List<LogEntry> ReadFromEnd(ILogSource source)
        {
            // for now let's just read from file
            var fileSource = (FileLogSource) source;
            return new FileLogReader(fileSource).ReadFromEnd(500);
        }
    }
}
