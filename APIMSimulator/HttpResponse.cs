using APIMSimulator.Abstract;

namespace APIMSimulator;

public class HttpResponse : HttpMessage, IResponse
{
    public int StatusCode { get; set; }

    public string StatusReason { get; set; } = string.Empty;
}
