using System;
using System.Linq;
using System.Xml.Linq;

namespace APIMSimulator.Policies;

internal class SetMethodPolicy : ValuePolicy
{
    public SetMethodPolicy(XElement element, PolicyBuilder builder)
        : base(element, builder)
    {
        if (ChildPolicies.Count() != 0)
        {
            throw new Exception("set-method can't have other policies.");
        }
    }

    public void Calculate(Context context, HttpMessage message)
    {
        message.Method = CaculateRequired<string>(context);
    }
}
