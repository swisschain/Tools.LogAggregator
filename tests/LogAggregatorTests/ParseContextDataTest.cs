using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace LogAggregatorTests
{
    public class ParseContextDataTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public ParseContextDataTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void ConvertToKeyValue()
        {
            var raw = new
            {
                Id = 1,
                Message = "Hello world",
                code = new
                {
                    Number = 3,
                    Text = "Three"
                }
            };

            var contextRaw = new
            {
                Path = "/api/test",
                Data = new {Key = 112}
            };

            var json = JsonConvert.SerializeObject(raw);

            var jsonCtx = JsonConvert.SerializeObject(contextRaw);

            var jo = JObject.Parse(json);

            var joCtx = JObject.Parse(jsonCtx);


            jo.Add(new JProperty("Table", "test-Table"));
            jo.Add(new JProperty("TableId", 334));
            jo.Add(new JProperty("TableScore", 0.223m));
            jo.Add(new JProperty("Enable", true));
            jo.Add("Ctx", joCtx);


            _testOutputHelper.WriteLine(jo.ToString());

            //var data = JsonConvert.DeserializeObject<Dictionary<string, string>>()

        }

        
    }
}
