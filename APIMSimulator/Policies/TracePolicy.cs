using System.Xml.Linq;

namespace APIMSimulator.Policies;

internal class TracePolicy : Policy
{
    public TracePolicy(XElement element, PolicyBuilder builder)
        : base(element, builder, parseChildPolicies: false)
    {
        // TODO
    }
}
