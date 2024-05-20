using APIMSimulator.Policies;
using System.Collections.Generic;

namespace APIMSimulator;

public interface IApiOperation
{
    HttpResponse ProcessRequest(HttpRequest request);

    IEnumerable<T> FindPolicy<T>(IReadOnlyDictionary<string, string>? attributes = null)
        where T : Policy;
}
