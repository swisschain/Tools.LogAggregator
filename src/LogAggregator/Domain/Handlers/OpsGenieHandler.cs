using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LogAggregator.Configuration;
using LogAggregator.Domain.Manager;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Swisschain.Tools.LogAggregator.ApiContract;

namespace LogAggregator.Domain.Handlers
{
    public class OpsGenieHandler : ILogHandler
    {
        private readonly OpsGenieSettings _settings;

        private ConcurrentDictionary<string, (string, int, DateTime)> _senderFilter = new ConcurrentDictionary<string, (string, int, DateTime)>();
        private ConcurrentDictionary<string, (string, int, DateTime)> _documentFilter = new ConcurrentDictionary<string, (string, int, DateTime)>();
        private HttpClient _opsgenieClient;

        public OpsGenieHandler(OpsGenieSettings settings)
        {
            _settings = settings;

            _opsgenieClient = new HttpClient();
            _opsgenieClient.DefaultRequestHeaders.Accept.Clear();
            _opsgenieClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _opsgenieClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("GenieKey", settings.ApiKey);
            _opsgenieClient.BaseAddress = new Uri(settings.OpsGenieUrl);

            Console.WriteLine($"DocumentIgnores: {settings.Ignore?.DocumentIgnores?.Count}; SenderIgnores: {settings.Ignore?.SenderIgnores?.Count}");
        }

        public Task<bool> HandleAsync(string topic, ILogItem log)
        {
            return HandleAsync(topic, new List<ILogItem>() {log});
        }

        public async Task<bool> HandleAsync(string topic, IReadOnlyList<ILogItem> logs)
        {
            if (!_settings.IsActive)
                return true;

            foreach (var logItem in logs.Where(e => e.Level == "error" || e.Level == "ERROR"))
            {
                var needIgnore = false;

                foreach (var ignore in _settings.Ignore.SenderIgnores)
                {
                    if (Regex.IsMatch(logItem.Sender, ignore.RegExp))
                    {
                        if (!_senderFilter.TryGetValue(ignore.RegExp, out var item))
                        {
                            item = (ignore.RegExp, 0, DateTime.UtcNow);
                            _senderFilter[ignore.RegExp] = item;
                        }

                        if (ignore.Period.TotalSeconds > 0 && (DateTime.UtcNow - item.Item3).TotalSeconds > ignore.Period.TotalSeconds)
                        {
                            item = (ignore.RegExp, 0, DateTime.UtcNow);
                            _senderFilter[ignore.RegExp] = item;
                        }

                        item.Item2++;
                        
                        if (item.Item2 < ignore.IgnoreCount)
                        {
                            Console.WriteLine($"Ignore message: {logItem.Sender}; {logItem.Level}; {logItem.Topic}");
                            Console.WriteLine($"Ignore regexp: {ignore.RegExp}; CountInPeriod: {item.Item2}");
                            needIgnore = true;
                            break;
                        }
                    }
                }

                if (!needIgnore)
                    foreach (var ignore in _settings.Ignore.DocumentIgnores)
                    {
                        if (Regex.IsMatch(logItem.Document, ignore.RegExp))
                        {
                            if (!_documentFilter.TryGetValue(ignore.RegExp, out var item))
                            {
                                item = (ignore.RegExp, 0, DateTime.UtcNow);
                                _documentFilter[ignore.RegExp] = item;
                            }

                            if ((DateTime.UtcNow - item.Item3).Seconds > ignore.Period.Seconds)
                            {
                                item = (ignore.RegExp, 0, DateTime.UtcNow);
                                _documentFilter[ignore.RegExp] = item;
                            }

                            item.Item2++;

                            if (item.Item2 < ignore.IgnoreCount)
                            {
                                Console.WriteLine($"Ignore message: {logItem.Sender}; {logItem.Level}; {logItem.Topic}");
                                Console.WriteLine($"Ignore regexp: {ignore.RegExp}; CountInPeriod: {item.Item2}");
                                needIgnore = true;
                                break;
                            }
                        }
                    }

                if (!needIgnore)
                    await SendToOpsgenie(logItem);
            }

            return true;
        }

        private async Task SendToOpsgenie(ILogItem log)
        {
            try
            {
                var doc = JObject.Parse(log.Document);
                
                var msg = doc["Msg"];
                var message = doc["Message"];
                var sndr = msg != null
                    ? msg.ToString(Formatting.None)
                    : message != null
                        ? message.ToString()
                        : "";

                if (sndr.ToLower() == "null")
                    sndr = "";
                
                var componentJo = doc["Component"];
                var component = componentJo != null ? componentJo.ToString(Formatting.None) : string.Empty;
                
                var sender = !string.IsNullOrEmpty(component) ? component : log.Sender;
                await PostRequest(sender, $"{log.Sender}: {sndr}", doc.ToString(Formatting.Indented));
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Cannot delivery to OpsGenie error message. Sender: {log.Sender}");
                Console.WriteLine(ex);
            }
        }

        public async Task PostRequest(string sender, string message, string description)
        {
            var json = JsonConvert.SerializeObject(new
            {
                message = message,
                alias = sender,
                description = description,
                priority = "P1"
            });


            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _opsgenieClient.PostAsync("", content);

            if (response.StatusCode != HttpStatusCode.Accepted)
            {
                var body = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Cannot send error message to opsgenie. StatusCode: {response.StatusCode}; Body: {body}");

            }
        }
    }
}
