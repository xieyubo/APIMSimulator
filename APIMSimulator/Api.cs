using APIMSimulator.Policies;

namespace APIMSimulator;

internal class Api : IApi
{
    private APIMSimulator _apim;
    private PoliciesPolicy _allOperationsPolicy;
    private PolicyBuilder _builder;

    public Api(APIMSimulator simulator, PoliciesPolicy allOperationsPolicy, PolicyBuilder builder)
    {
        _apim = simulator;
        _allOperationsPolicy = allOperationsPolicy;
        _builder = builder;
    }

    public IApiOperation AddOperation(string xmlPolicyPath)
    {
        var policies = _builder.BuildFromXmlFile<PoliciesPolicy>(xmlPolicyPath);
        policies.Parent = _allOperationsPolicy;
        return new ApiOperation(_apim, policies);
    }
}
