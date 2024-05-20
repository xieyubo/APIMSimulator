using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace APIMSimulator.Policies;

internal class BasePolicy : Policy
{
    public BasePolicy(XElement element, PolicyBuilder builder)
        : base(element, builder)
    {
        if (ChildPolicies.Count() > 0)
        {
            throw new Exception("base policy can't contains other policies");
        }
    }

    public Policy? Parent { get; set; }

    public override void FindPolicy<T>(List<T> results, IReadOnlyDictionary<string, string>? attributes)
    {
        base.FindPolicy(results, attributes);
        Parent?.FindPolicy(results, attributes);
    }

    internal override void Execute(Context context)
    {
        Parent?.Execute(context);
    }
}
