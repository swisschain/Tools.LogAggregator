using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LogAggregator.Domain.Manager;
using Swisschain.Tools.LogAggregator.ApiContract;

namespace LogAggregator.Domain.Handlers
{
    public class SimpleHandler : ILogHandler
    {
        public Task<bool> HandleAsync(string topic, ILogItem log)
        {
            Console.WriteLine($"Receive 1 log event, topic {topic}");
            return Task.FromResult(true);
        }

        public Task<bool> HandleAsync(string topic, IReadOnlyList<ILogItem> logs)
        {
            Console.WriteLine($"Receive {logs.Count} log event, topic {topic}");
            return Task.FromResult(true);
        }
    }
}
