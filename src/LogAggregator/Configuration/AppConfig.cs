namespace LogAggregator.Configuration
{
    public class AppConfig
    {
        public ElasticSearchSettings ElasticSearchSettings { get; set; }

    }

    public class ElasticSearchSettings
    {
        public string Url { get; set; }
    }
}
