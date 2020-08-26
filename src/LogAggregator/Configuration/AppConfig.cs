using System.Collections.Generic;
using System.Runtime.InteropServices;
using Google.Protobuf.WellKnownTypes;

namespace LogAggregator.Configuration
{
    public class AppConfig
    {
        public ElasticSearchSettings ElasticSearchSettings { get; set; } = new ElasticSearchSettings();

        public OpsGenieSettings OpsGenie { get; set; } = new OpsGenieSettings();
        

    }

    public class OpsGenieSettings
    {
        public bool IsActive { get; set; } = false;

        public string ApiKey { get; set; }

        public string OpsGenieUrl { get; set; } = "https://api.eu.opsgenie.com/v2/alerts";
        public IgnoreSettings Ignore { get; set; } = new IgnoreSettings();
    }

    public class IgnoreSettings
    {
        public List<IgnoreParams> SenderIgnores { get; set; } = new List<IgnoreParams>();
        public List<IgnoreParams> DocumentIgnores { get; set; } = new List<IgnoreParams>();

        public class IgnoreParams
        {
            public string RegExp { get; set; }
            public int IgnoreCount { get; set; }
            public Timestamp Period { get; set; }
        }
    }



    public class ElasticSearchSettings
    {
        public string Url { get; set; }
    }
}
