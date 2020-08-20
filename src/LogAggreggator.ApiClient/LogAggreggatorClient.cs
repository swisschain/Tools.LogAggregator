using Swisschain.Tools.LogAggreggator.ApiClient.Common;
using Swisschain.Tools.LogAggreggator.ApiContract;

namespace Swisschain.Tools.LogAggreggator.ApiClient
{
    public class LogAggreggatorClient : BaseGrpcClient, ILogAggreggatorClient
    {
        public LogAggreggatorClient(string serverGrpcUrl) : base(serverGrpcUrl)
        {
            Monitoring = new Monitoring.MonitoringClient(Channel);
        }

        public Monitoring.MonitoringClient Monitoring { get; }
    }
}
