using Swisschain.Tools.LogAggregator.ApiContract;

namespace Swisschain.Tools.LogAggregator.ApiClient
{
    public interface ILogAggregatorClient
    {
        Monitoring.MonitoringClient Monitoring { get; }

        LogCollector.LogCollectorClient LogCollector { get; }
    }
}
