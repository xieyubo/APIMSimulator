using APIMSimulator.Abstract;
using System;

namespace APIMSimulator;

public class HttpMessageBody : IMessageBody
{
    public string Body { get; set; } = string.Empty;

    public T As<T>(bool preserveContent = false)
    {
        string content = Body;

        if (!preserveContent)
        {
            Body = string.Empty;
        }

        if (typeof(T) == typeof(string))
        {
            return (T)(object)content;
        }

        throw new NotImplementedException();
    }

}
