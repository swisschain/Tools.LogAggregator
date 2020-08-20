using Swisschain.Tools.LogAggregator.ApiClient.Common;
using Swisschain.Tools.LogAggregator.ApiContract;

namespace Swisschain.Tools.LogAggregator.ApiClient
{
    public class LogAggregatorClient : BaseGrpcClient, ILogAggregatorClient
    {
        public LogAggregatorClient(string serverGrpcUrl) : base(serverGrpcUrl)
        {
            Monitoring = new Monitoring.MonitoringClient(Channel);
            LogCollector = new LogCollector.LogCollectorClient(Channel);
        }

        public Monitoring.MonitoringClient Monitoring { get; }
        public LogCollector.LogCollectorClient LogCollector { get; }
    }
}
