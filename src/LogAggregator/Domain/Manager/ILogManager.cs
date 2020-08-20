using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;
using Swisschain.Tools.LogAggregator.ApiContract;

namespace LogAggregator.Domain.Manager
{
    public interface ILogManager
    {
        Task<bool> HandleAsync(ILogItem log);
        Task<bool> HandleAsync(IReadOnlyList<ILogItem> logs);
    }
}
