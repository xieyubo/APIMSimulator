using System;
using System.Linq;
using System.Xml.Linq;

namespace APIMSimulator.Policies;

internal class ChoosePolicy : Policy
{
    private WhenPolicy[] _whenPolicies;
    private OtherwisePolicy? _otherwisePolicy;

    public ChoosePolicy(XElement element, PolicyBuilder builder)
        : base(element, builder)
    {
        _whenPolicies = ChildPolicies.Where(p => p is WhenPolicy).Select(p => (WhenPolicy)p).ToArray();
        _otherwisePolicy = ChildPolicies.Where(p => p is OtherwisePolicy).Select(p => (OtherwisePolicy)p).SingleOrDefault();

        if (ChildPolicies.Count() != _whenPolicies.Count() + (_otherwisePolicy != null ? 1 : 0))
        {
            throw new Exception("choose policy can only have when or otherwise policy");
        }
    }

    internal override void Execute(Context context)
    {
        if (!_whenPolicies.Any(p => p.Caculate(context)))
        {
            _otherwisePolicy?.Execute(context);
        }
    }
}
