using APIMSimulator.Policies;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace APIMSimulator;

public class APIMSimulator
{
    private Dictionary<string, FragmentPolicy> _fragments = new ();
    private PoliciesPolicy? _allAPIs;
    private PolicyBuilder _policyBuilder;

    public APIMSimulator(IServiceProvider sp)
    {
        _policyBuilder = new PolicyBuilder(sp);
    }

    public void SetAllAPIsPolicy(string xmlPath)
    {
        var policies = _policyBuilder.Build(XElement.Parse(File.ReadAllText(xmlPath))) as PoliciesPolicy;
        if (policies == null)
        {
            throw new Exception($"'{xmlPath}' doesn't contain valid policies policy.");
        }
        _allAPIs = policies;
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
        allOperationsPolicy.Parent = _allAPIs;
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
