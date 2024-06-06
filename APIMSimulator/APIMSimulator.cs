using APIMSimulator.Policies;
using System.Xml.Linq;

namespace APIMSimulator;

public class APIMSimulator
{
    public PoliciesPolicy? AllAPIs { get; internal set; }

    private PolicyBuilder _policyBuilder = new PolicyBuilder();

    public void SetAllAPIsPolicy(string xmlPath)
    {
        AllAPIs = _policyBuilder.BuildFromXmlFile<PoliciesPolicy>(xmlPath);
    }

    public IApi AddApi(string? xmlPathForAllOperations = null)
    {
        var allOperationsPolicy = xmlPathForAllOperations != null ?
            _policyBuilder.BuildFromXmlFile<PoliciesPolicy>(xmlPathForAllOperations) :
            (PoliciesPolicy)_policyBuilder.Build(XElement.Parse("""
                <policies>
                    <inbound>
                        <base/>
                    </inbound>
                    <backend>
                        <base />
                    </backend>
                    <outbound>
                        <base />
                    </outbound>
                    <on-error>
                        <base />
                    </on-error>
                </policies>
            """));
        allOperationsPolicy.Parent = AllAPIs;
        return new Api(this, allOperationsPolicy, _policyBuilder);
    }

    public void AddPolicyFragment(string name, string xmlPath)
    {
        _policyBuilder.Fragments.Add(name, _policyBuilder.BuildFromXmlFile<FragmentPolicy>(xmlPath));
    }

    internal HttpResponse Execute(Policy policy, HttpRequest request)
    {
        try
        {
            var context = new Context(this, request);
            policy.Execute(context);
            return context.Response;
        }
        catch (ReturnResponseException ex)
        {
            return ex.Response;
        }
    }
}
