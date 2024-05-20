using System.Linq;
using System.Xml.Linq;

namespace APIMSimulator.Policies;

internal class OutboundPolicy : Policy
{
    public OutboundPolicy? Parent
    {
        set
        {
            ChildPolicies.Where(p => p is BasePolicy).Select(p => ((BasePolicy)p).Parent = value);
        }
    }

    public OutboundPolicy(XElement element, PolicyBuilder builder)
        : base(element, builder)
    {
    }
}
