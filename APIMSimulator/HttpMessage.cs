using APIMSimulator.Abstract;
using System.Collections.Generic;

namespace APIMSimulator;

public class HttpMessage : IHttpMessage
{
    #region IHttpMessage
    IMessageBody? IHttpMessage.Body => Body;
    IReadOnlyDictionary<string, string[]> IHttpMessage.Headers => Headers;
    #endregion

    public string Method { get; set; } = "GET";

    public HttpMessageBody? Body { get; set; }

    public Dictionary<string, string[]> Headers { get; set; } = new();

    
}
