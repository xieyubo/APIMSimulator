using System.Linq;
using System.Xml.Linq;

namespace APIMSimulator.Policies;

internal class OnErrorPolicy : Policy
{
    public OnErrorPolicy? Parent
    {
        set
        {
            ChildPolicies.Where(p => p is BasePolicy).ForEach(p => ((BasePolicy)p).Parent = value);
        }
    }

    public OnErrorPolicy(XElement element, PolicyBuilder builder)
        : base(element, builder)
    {
    }
}
