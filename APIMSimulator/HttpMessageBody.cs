using APIMSimulator.Abstract;
using System;

namespace APIMSimulator;

public class HttpMessageBody : IMessageBody
{
    public string Body { get; set; } = string.Empty;

    public T As<T>(bool preserveContent = false)
    {
        throw new NotImplementedException();
    }

}
