using System.Linq;
using System.Xml.Linq;

namespace APIMSimulator.Policies;

internal class InboundPolicy : Policy
{
    public InboundPolicy? Parent
    {
        set
        {
            ChildPolicies.Where(p => p is BasePolicy).ForEach(p => ((BasePolicy)p).Parent = value);
        }
    }

    public InboundPolicy(XElement element, PolicyBuilder builder)
        : base(element, builder)
    {
    }
}
