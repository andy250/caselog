using andy250.CaseLog.Core.Configuration;

namespace andy250.CaseLog.Core.Interfaces
{
    public interface IConfigProvider
    {
        CaseLogConfig Config { get; }
        void Reload();
    }
}
