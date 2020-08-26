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

        private bool _isActive = false;

        public ElkClient(ElasticSearchSettings config)
        {
            if (string.IsNullOrEmpty(config?.Url))
            {
                _isActive = false;
            }
            else
            {
                _isActive = true;

                var settings = new ConnectionConfiguration(new Uri(config.Url))
                    .RequestTimeout(TimeSpan.FromMinutes(2));

                _lowlevelClient = new ElasticLowLevelClient(settings);
            }
        }

        public async Task<bool> WriteBulkAsync(List<(string,string)> index_data)
        {
            if (!_isActive)
                return true;

            try
            {
                var content = new List<string>();
                foreach (var item in index_data)
                {
                    content.Add(JsonConvert.SerializeObject(new
                    {
                        index = new {
                            _index = item.Item1,
                            _type = "_doc",
                            _id = Guid.NewGuid().ToString("N")
                        }
                    }));
                    content.Add(item.Item2);
                }

                var res = await _lowlevelClient.BulkAsync<BulkResponse>(PostData.MultiJson(content));

                if (!res.IsValid)
                {
                    Console.WriteLine($"CANNOT DELIVERY: {res.DebugInformation}");
                    foreach (var item in res.ItemsWithErrors)
                    {
                        Console.WriteLine($"  -item: {item.Result}");
                    }
                }

                if (res.Errors)
                {
                    Console.WriteLine($"HAS ERRORS: {res.DebugInformation}");
                    foreach (var item in res.ItemsWithErrors)
                    {
                        Console.WriteLine($"  -item: {item.Result}");
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"CANNOT DELIVERY EXCEPTION:");
                Console.WriteLine(e);
                return false;
            }

        }
    }
}
