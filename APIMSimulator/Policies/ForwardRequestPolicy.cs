using System.Xml.Linq;

namespace APIMSimulator.Policies;

internal class ForwardRequestPolicy : Policy
{
    public ForwardRequestPolicy(XElement element, PolicyBuilder builder)
        : base(element, builder, parseChildPolicies: false)
    {
    }
}
