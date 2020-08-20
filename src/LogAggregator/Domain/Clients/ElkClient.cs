using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Elasticsearch.Net;
using LogAggregator.Configuration;
using Nest;
using Newtonsoft.Json;

namespace LogAggregator.Domain.Clients
{
    public class ElkClient
    {
        private readonly ElasticLowLevelClient _lowlevelClient;

        public ElkClient(ElasticSearchSettings config)
        {
            var settings = new ConnectionConfiguration(new Uri(config.Url))
                .RequestTimeout(TimeSpan.FromMinutes(2));

            _lowlevelClient = new ElasticLowLevelClient(settings);
        }

        public async Task<bool> WriteBulkAsync(List<(string,string)> index_data)
        {
            var content = new List<string>();
            foreach (var item in index_data)
            {
                content.Add(JsonConvert.SerializeObject(new
                {
                    index = new {
                        _index = item.Item1,
                        _type = "logs",
                        _id = Guid.NewGuid().ToString("N")
                    }
                }));
                content.Add(item.Item2);
            }

            var res = await _lowlevelClient.BulkAsync<BulkResponse>(PostData.MultiJson(content));

            return res.IsValid && !res.Errors;
        }
    }
}
