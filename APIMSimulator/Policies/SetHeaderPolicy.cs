using System;
using System.Linq;
using System.Xml.Linq;

namespace APIMSimulator.Policies;

internal class SetHeaderPolicy : Policy
{
    private string _name;
    private string _existsAction;

    public SetHeaderPolicy(XElement element, PolicyBuilder builder)
        : base(element, builder)
    {
        _name = element.GetRequiredAttribute("name");
        _existsAction = element.GetAttribute("exists-action", "override", "skip", "append", "delete");

        // All children must be value policy.
        if (ChildPolicies.Any(p => !(p is ValuePolicy)))
        {
            throw new Exception("Only value element is allowed for set-header");
        }
    }

    public void Caculate(Context context, HttpMessage message)
    {
        var headers = message.Headers;

        if (headers.ContainsKey(_name))
        {
            if (_existsAction == "override" || _existsAction == "delete")
            {
                headers.Remove(_name);
            }
            else if (_existsAction == "skip")
            {
                return;
            }
        }

        var values = ChildPolicies.Select(p => ((ValuePolicy)p).CaculateRequired<string>(context)).ToArray();
        if (!headers.ContainsKey(_name))
        {
            headers.Add(_name, values);
        }
        else
        {
            headers[_name] = headers[_name].Union(values).ToArray();
        }
    }
}
