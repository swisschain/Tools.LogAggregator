using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Swisschain.Sdk.Server.Common;
using Swisschain.Tools.LogAggregator.ApiContract;

namespace LogAggregator.GrpcServices
{
    public class MonitoringService : Monitoring.MonitoringBase
    {
        public override Task<IsAliveResponce> IsAlive(IsAliveRequest request, ServerCallContext context)
        {
            var result = new IsAliveResponce
            {
                Name = ApplicationInformation.AppName,
                Version = ApplicationInformation.AppVersion,
                StartedAt = ApplicationInformation.StartedAt.ToString("yyyy-MM-dd HH:mm:ss")
            };

            return Task.FromResult(result);
        }
    }

    public class LogCollectorService : LogCollector.LogCollectorBase
    {
        public override Task<Responce> collectSingle(LogItem request, ServerCallContext context)
        {
            return base.collectSingle(request, context);
        }

        public override Task<Responce> collectRange(LogItemList request, ServerCallContext context)
        {
            return base.collectRange(request, context);
        }

        public override Task<Empty> collectStream(IAsyncStreamReader<LogItem> requestStream, ServerCallContext context)
        {
            return base.collectStream(requestStream, context);
        }
    }
}
