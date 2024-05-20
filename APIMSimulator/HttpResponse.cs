using APIMSimulator.Abstract;

namespace APIMSimulator;

public class HttpResponse : HttpMessage, IResponse
{
    public int StatusCode { get; set; } = 200;

    public string StatusReason { get; set; } = string.Empty;
}
