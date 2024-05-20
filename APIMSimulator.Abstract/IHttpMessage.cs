using System.Collections.Generic;

namespace APIMSimulator.Abstract;

public interface IHttpMessage
{
    string Method { get; }

    IReadOnlyDictionary<string, string[]> Headers { get; }

    public IMessageBody? Body { get; }
}
