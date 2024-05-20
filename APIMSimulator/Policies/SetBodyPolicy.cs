using System.Xml.Linq;

namespace APIMSimulator.Policies;

internal class SetBodyPolicy : Policy
{
    private Expression _expression;

    public SetBodyPolicy(XElement element, PolicyBuilder builder)
        : base(element, builder)
    {
        _expression = new Expression(element.Value);
    }

    public void Caculate(Context context, HttpMessage message)
    {
        message.Body = new HttpMessageBody() { Body = _expression.Execute(context) as string ?? string.Empty };
    }
}
