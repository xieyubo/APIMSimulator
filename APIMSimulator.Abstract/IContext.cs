using System.Collections.Generic;

namespace APIMSimulator.Abstract;

public interface IContext
{
    IRequest Request { get; }

    IResponse Response { get; }

    IReadOnlyDictionary<string, object?> Variables { get; }
}
