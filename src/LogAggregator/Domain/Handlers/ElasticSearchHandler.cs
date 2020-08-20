using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LogAggregator.Domain.Clients;
using LogAggregator.Domain.Manager;
using Newtonsoft.Json.Linq;
using Swisschain.Tools.LogAggregator.ApiContract;

namespace LogAggregator.Domain.Handlers
{
    public class ElasticSearchHandler : ILogHandler
    {
        private readonly ElkClient _client;

        public ElasticSearchHandler(ElkClient client)
        {
            _client = client;
        }

        public Task<bool> HandleAsync(string topic, ILogItem log)
        {
            return HandleAsync(topic, new List<ILogItem>() {log});
        }

        public async Task<bool> HandleAsync(string topic, IReadOnlyList<ILogItem> logs)
        {
            var data = new List<(string,string)>();
            foreach (var log in logs)
            {
                data.Add(($"topic-{topic}-{DateTime.UtcNow:yyyy-MM-dd}", log.Document));
            }

            var result = await _client.WriteBulkAsync(data);

            return result;
        }
    }
}
