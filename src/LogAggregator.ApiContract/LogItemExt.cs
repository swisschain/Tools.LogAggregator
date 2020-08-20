namespace Swisschain.Tools.LogAggregator.ApiContract
{
    public partial class LogItem : ILogItem
    {   
    }

    public interface ILogItem
    {
        string Topic { get; }

        string Sender { get; }

        string Level { get; }

        string Document { get; }
    }
}
