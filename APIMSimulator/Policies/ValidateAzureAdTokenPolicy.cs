using System.Xml.Linq;

namespace APIMSimulator.Policies;

internal class ValidateAzureAdTokenPolicy : Policy
{
    public ValidateAzureAdTokenPolicy(XElement element, PolicyBuilder builder)
        : base(element, builder, parseChildPolicies: false)
    {
    }
}
