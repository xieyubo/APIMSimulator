using System;
using System.Xml.Linq;

namespace APIMSimulator.Policies;

internal class ReturnResponsePolicy : Policy
{
    private string? _responseVariableName;
    private SetStatusPolicy? _setStatus;
    private SetHeaderPolicy[]? _setHeaders;
    private SetBodyPolicy? _setBody;

    public ReturnResponsePolicy(XElement element, PolicyBuilder builder)
        : base(element, builder, parseChildPolicies: false)
    {
        _responseVariableName = element.GetAttribute("response-variable-name");

        // Go through child policies.
        var childElement = element.FirstNode as XElement;
        _setStatus = builder.TryParseNextPolicy<SetStatusPolicy>(ref childElement);
        _setHeaders = builder.TryParseNextPolicies<SetHeaderPolicy>(ref childElement);
        _setBody = builder.TryParseNextPolicy<SetBodyPolicy>(ref childElement);

        // Shouldn't have other policies.
        if (childElement != null)
        {
            throw new Exception("Unexpected policy in return-response-policy");
        }
    }

    internal override void Execute(Context context)
    {
        var response = _responseVariableName != null ? (HttpResponse)context.Variables[_responseVariableName]! : new HttpResponse();

        _setStatus?.Caculate(context, response);

        _setHeaders?.ForEach(p => p.Caculate(context, response));

        _setBody?.Caculate(context, response);

        throw new ReturnResponseException(response);
    }
}
