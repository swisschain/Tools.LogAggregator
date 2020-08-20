namespace LogAggregator.Configuration
{
    public class AppConfig
    {
        public DbConfig Db { get; set; }

        public ElasticSearchSettings ElasticSearchSettings { get; set; }

    }

    public class ElasticSearchSettings
    {
        public string Url { get; set; }
    }
}
