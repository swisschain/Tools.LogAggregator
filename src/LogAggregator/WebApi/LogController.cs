using System.Collections.Generic;
using System.Threading.Tasks;
using LogAggregator.Domain.Manager;
using LogAggregator.WebApi.Models.Collector;
using LogAggregator.WebApi.Models.LykkeLogCollector;
using Microsoft.AspNetCore.Mvc;

namespace LogAggregator.WebApi
{
    [ApiController]
    [Route("api/log-collector")]
    public class LogController : ControllerBase
    {
        private readonly ILogManager _logManager;

        public LogController(ILogManager logManager)
        {
            _logManager = logManager;
        }

        [HttpPost("log")]
        public async Task<bool> CollectLogAsync([FromBody] List<LogItemRest> logs)
        {
            var res = await _logManager.HandleAsync(logs);

            if (!res)
                HttpContext.Response.StatusCode = 500;

            return res;
        }
    }
}
