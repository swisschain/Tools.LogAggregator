using Swisschain.Tools.LogAggreggator.ApiContract;

namespace Swisschain.Tools.LogAggreggator.ApiClient
{
    public interface ILogAggreggatorClient
    {
        Monitoring.MonitoringClient Monitoring { get; }
    }
}
