using System.Xml.Linq;

namespace APIMSimulator.Policies;

internal class CacheStoreValuePolicy : Policy
{
    public CacheStoreValuePolicy(XElement element, PolicyBuilder builder)
        : base(element, builder, parseChildPolicies: false)
    {
    }
}
