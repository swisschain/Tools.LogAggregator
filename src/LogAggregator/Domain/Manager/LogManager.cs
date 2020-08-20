using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Swisschain.Tools.LogAggregator.ApiContract;

namespace LogAggregator.Domain.Manager
{
    public class LogManager : ILogManager
    {
        private readonly ILogger<LogManager> _logger;
        private readonly ILogHandler[] _handlers;

        public LogManager(ILogger<LogManager> logger, ILogHandler[] handlers)
        {
            _logger = logger;
            _handlers = handlers;
        }

        public async Task<bool> HandleAsync(ILogItem log)
        {
            var tasks = _handlers.Select(h => HandlerTask(log.Topic, log, h)).ToArray();

            await Task.WhenAll(tasks);

            return tasks.All(t => t.Result);
        }

        public async Task<bool> HandleAsync(IReadOnlyList<ILogItem> logs)
        {
            var res = true;

            foreach (var topic in logs.GroupBy(l => l.Topic))
            {
                res = res && await HandleLogs(topic.Key, topic.ToList());
            }

            return res;
        }

        private async Task<bool> HandleLogs(string topic, IReadOnlyList<ILogItem> logs)
        {
            var tasks = _handlers.Select(h => HandlerTask(topic, logs, h)).ToArray();

            await Task.WhenAll(tasks);

            return tasks.All(t => t.Result);
        }

        private async Task<bool> HandlerTask(string topic, ILogItem log, ILogHandler handler)
        {
            try
            {
                return await handler.HandleAsync(topic, log);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cannot handle log message ({message}) in handler {handler}", $"msg: {JsonConvert.SerializeObject(log)}", handler.GetType().FullName);
                return false;
            }
        }

        private async Task<bool> HandlerTask(string topic, IReadOnlyList<ILogItem> logs, ILogHandler handler)
        {
            try
            {
                return await handler.HandleAsync(topic, logs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cannot handle log message ({message}) in handler {handler}", $"msg: {JsonConvert.SerializeObject(logs)}", handler.GetType().FullName);
                return false;
            }
        }

        
    }
}
