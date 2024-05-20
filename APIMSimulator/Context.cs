using APIMSimulator.Abstract;
using System.Collections.Generic;

namespace APIMSimulator;

internal class Context : IContext
{
    #region IContext
    IRequest IContext.Request => Request;

    IResponse IContext.Response => Response;

    IReadOnlyDictionary<string, object?> IContext.Variables => Variables;
    #endregion

    internal HttpRequest Request { get; }

    internal HttpResponse Response { get; } = new HttpResponse();

    internal Dictionary<string, object?> Variables { get; set; } = new Dictionary<string, object?>();

    internal APIMSimulator Simulator { get; }

    public Context(APIMSimulator simulator, HttpRequest request)
    {
        Simulator = simulator;
        Request = request;
    }
}
