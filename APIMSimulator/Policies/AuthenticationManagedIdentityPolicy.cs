using System;
using System.Xml.Linq;

namespace APIMSimulator.Policies;

public class AuthenticationManagedIdentityPolicy : Policy
{
    public interface ICallback
    {
        string OnAuthenticate(string resource, string? clientId);
    }

    public ICallback? Callback;

    private Expression _resource;
    private string? _clientId { get; set; }
    private string? _outputTokenVariableName { get; set; }
    private string _ignoreError { get; set; }

    public AuthenticationManagedIdentityPolicy(XElement element, PolicyBuilder builder)
        : base(element, builder)
    {
        _resource = new Expression(element.GetRequiredAttribute("resource"));
        _clientId = element.GetAttribute("client-id");
        _outputTokenVariableName = element.GetAttribute("output-token-variable-name");
        _ignoreError = element.GetAttribute("ignore-error", "false", "true");
    }

    internal override void Execute(Context context)
    {
        var resource = _resource.Execute(context) as string ?? throw new Exception($"resource of '{Name}' policy can't be null.");

        string token;
        try
        {
            token = Callback?.OnAuthenticate(resource, _clientId) ?? throw new Exception($"No callback fro '{Name}' policy.");
        }
        catch
        {
            if (!bool.Parse(_ignoreError))
            {
                throw;
            }
            else
            {
                return;
            }
        }

        if (!string.IsNullOrEmpty(_outputTokenVariableName))
        {
            context.Variables.AddOrUpdate(_outputTokenVariableName!, token);
        }
    }
}
