using Swisschain.Tools.LogAggregator.ApiContract;

namespace LogAggregator.WebApi.Models.LykkeLogCollector
{
    public class LykkeLogItem : ILogItem
    {
        public string Topic { get; set; }
        public string Sender { get; set; }
        public string Level { get; set; }
        public string Document { get; set; }
    }
}
