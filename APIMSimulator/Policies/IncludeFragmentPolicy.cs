using System;
using System.Linq;
using System.Xml.Linq;

namespace APIMSimulator.Policies;

internal class IncludeFragmentPolicy : Policy
{
    public IncludeFragmentPolicy(XElement element, PolicyBuilder builder)
        : base(element, builder)
    {
        if (ChildPolicies.Count() > 0)
        {
            throw new Exception("include-fragment can't have other policies.");
        }

        var fragmentId = element.GetRequiredAttribute("fragment-id");
        if (!builder.Fragments.TryGetValue(fragmentId, out var fragment))
        {
            throw new Exception($"Policy fragment with id '{fragmentId}' could not be found.");
        }
        ChildPolicies.Add(fragment);
    }
}
