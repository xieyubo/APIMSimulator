using APIMSimulator.Policies;
using System.Collections.Generic;

namespace APIMSimulator;

internal class ApiOperation : IApiOperation
{
    private APIMSimulator _apim;
    private PoliciesPolicy _policies;

    public ApiOperation(APIMSimulator apim, PoliciesPolicy policies)
    {
        _apim = apim;
        _policies = policies;
    }

    public IEnumerable<T> FindPolicy<T>(IReadOnlyDictionary<string, string>? attributes = null)
        where T : Policy
    {
        return _policies.FindPolicy<T>(attributes);
    }

    public HttpResponse ProcessRequest(HttpRequest request)
    {
        return _apim.Execute(_policies, request);
    }
}
