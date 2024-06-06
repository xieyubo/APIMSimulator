using System;
using System.Xml.Linq;

namespace APIMSimulator.Policies;

internal class SetStatusPolicy : Policy
{
    private Expression _code;
    private Expression _reason;

    public SetStatusPolicy(XElement element, PolicyBuilder builder)
        : base(element, builder)
    {
        _code = new Expression(element.GetRequiredAttribute("code"));
        _reason = new Expression(element.GetRequiredAttribute("reason"));

        if (element.HasElements)
        {
            throw new Exception("set-status policy can't contains other policies");
        }
    }

    public void Caculate(Context context, HttpResponse message)
    {
        var obj = _code.Execute(context) ?? throw new Exception($"'code' of '{Name}' can't be null");
        if (!(obj is int) && !(obj is string))
        {
            throw new Exception($"'code' of '{Name}' is invalid.");
        }
        message.StatusCode = obj is string ? int.Parse((string)obj) : (int)obj;
        message.StatusReason = _reason.Execute(context) as string ?? throw new Exception("reason of set-status can't be null.");
    }
}
