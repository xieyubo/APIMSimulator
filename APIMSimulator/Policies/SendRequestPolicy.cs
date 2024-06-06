using System;
using System.Linq;
using System.Xml.Linq;

namespace APIMSimulator.Policies;

public class SendRequestPolicy : Policy
{
    public interface ICallback
    {
        HttpResponse OnSendRequest(HttpRequest request);
    }

    public ICallback? Callback { get; set; }

    private string _mode;
    private string _responseVariableName;
    private SetUrlPolicy? _setUrl;
    private SetMethodPolicy? _setMethod;
    private SetHeaderPolicy[]? _setHeaders;
    private SetBodyPolicy? _setBody;

    public SendRequestPolicy(XElement element, PolicyBuilder builder)
        : base(element, builder)
    {
        _mode = element.GetAttribute("mode", "new", "copy");
        _responseVariableName = element.GetRequiredAttribute("response-variable-name");

        // Go through child policies.
        int index = 0;
        _setUrl = GetNextChildPolicy<SetUrlPolicy>(ref index, isRequired: _mode != "copy", "set-url is required if mode is not 'copy'");
        _setMethod = GetNextChildPolicy<SetMethodPolicy>(ref index, isRequired: _mode != "copy", "set-method is required is mode is not 'copy'");
        _setHeaders = GetNextChildPolicies<SetHeaderPolicy>(ref index);
        _setBody = GetNextChildPolicy<SetBodyPolicy>(ref index);

        // Shouldn't have other policies.
        if (index < ChildPolicies.Count())
        {
            throw new Exception("Unexpected policy in send-request");
        }
    }

    internal override void Execute(Context context)
    {
        var request = _mode == "copy" ? context.Request.Clone() : new HttpRequest();

        _setUrl?.Caculate(context, request);

        _setMethod?.Calculate(context, request);

        _setHeaders?.ForEach(p => p.Caculate(context, request));

        _setBody?.Caculate(context, request);

        var response = Callback?.OnSendRequest(request) ?? throw new Exception($"No callback for '{Name}' policy.");
        context.Variables.AddOrUpdate(_responseVariableName, response);
    }
}
