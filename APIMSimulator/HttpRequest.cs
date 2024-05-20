using APIMSimulator.Abstract;
using System;

namespace APIMSimulator;

public class HttpRequest : HttpMessage, IRequest
{
    public string Url { get; set; } = string.Empty;

    public HttpRequest Clone()
    {
        throw new NotImplementedException();
    }
}
