using System.Collections.Generic;
using System.Threading.Tasks;
using Swisschain.Tools.LogAggregator.ApiContract;

namespace LogAggregator.Domain.Manager
{
    public interface ILogHandler
    {
        Task<bool> HandleAsync(string topic, ILogItem log);
        Task<bool> HandleAsync(string topic, IReadOnlyList<ILogItem> logs);
    }
}
