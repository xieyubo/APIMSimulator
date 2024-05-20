using System;
using System.Linq;
using System.Xml.Linq;

namespace APIMSimulator.Policies;

internal class WhenPolicy : Policy
{
    private Expression _expression;

    public WhenPolicy(XElement element, PolicyBuilder builder)
        : base(element, builder)
    {
        var condition = element.Attributes().FirstOrDefault(v => v.Name == "condition")?.Value;
        if (string.IsNullOrEmpty(condition))
        {
            throw new Exception("condition attribute is missing");
        }
        _expression = new Expression(condition!);
    }

    public bool Caculate(Context context)
    {
        if ((_expression.Execute(context) as bool? ?? throw new Exception("condition value can't be null.")) == true)
        {
            base.Execute(context);
            return true;
        }
        return false;
    }
}
