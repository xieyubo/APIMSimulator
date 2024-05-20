using System;
using System.Linq;
using System.Xml.Linq;

namespace APIMSimulator.Policies;

internal class ValuePolicy : Policy
{
    private Expression _expression;

    public ValuePolicy(XElement element, PolicyBuilder builder)
        : base(element, builder)
    {
        if (ChildPolicies.Count() != 0)
        {
            throw new Exception("set-method can't have other policies.");
        }

        _expression = new Expression(element.Value);
    }

    public T CaculateRequired<T>(Context context)
    {
        return (T)(_expression.Execute(context) ?? throw new Exception("value can't be empty"));
    }
}
