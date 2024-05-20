namespace APIMSimulator.Abstract;

public interface IResponse : IHttpMessage
{
    int StatusCode { get; }

    string StatusReason { get; }
}
