using System;
using System.Xml.Linq;

namespace APIMSimulator.Policies;

internal class SetVariablePolicy : Policy
{
    private string _name;
    private Expression _value;

    public SetVariablePolicy(XElement element, PolicyBuilder builder)
        : base(element, builder)
    {
        _name = element.GetRequiredAttribute("name");
        _value = new Expression(element.GetRequiredAttribute("value"));
    }

    internal override void Execute(Context context)
    {
        context.Variables.AddOrUpdate(_name, _value.Execute(context));
    }
}
