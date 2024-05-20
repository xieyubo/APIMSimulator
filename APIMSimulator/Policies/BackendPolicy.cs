using System.Linq;
using System.Xml.Linq;

namespace APIMSimulator.Policies;

internal class BackendPolicy : Policy
{
    public BackendPolicy? Parent
    {
        set
        {
            ChildPolicies.Where(p => p is BasePolicy).Select(p => ((BasePolicy)p).Parent = value);
        }
    }

    public BackendPolicy(XElement element, PolicyBuilder builder)
        : base(element, builder)
    {
    }
}
