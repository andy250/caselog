using System.Collections.Generic;
using andy250.CaseLog.Core.Models;

namespace andy250.CaseLog.Core.Interfaces
{
    public interface ILogReader
    {
        List<LogEntry> ReadFromEnd(ILogSource source);
    }
}