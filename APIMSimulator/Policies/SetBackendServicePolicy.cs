using System;
using System.Xml.Linq;

namespace APIMSimulator.Policies;

internal class SetBackendServicePolicy : Policy
{
    private Expression? _baseUrl;
    private Expression? _backendId;

    public SetBackendServicePolicy(XElement element, PolicyBuilder builder)
        : base(element, builder)
    {
        var baseUrlValue = element.GetAttribute("base-url");
        if (baseUrlValue != null)
        {
            _baseUrl = new Expression(baseUrlValue);
        }

        var backendValue = element.GetAttribute("backend-id");
        if (backendValue != null)
        {
            _backendId = new Expression(backendValue);
        }
        if (_baseUrl == null && _backendId == null)
        {
            throw new Exception("One of 'base-url' or 'backend-id' must be present.");
        }
    }

    internal override void Execute(Context context)
    {
        if (_baseUrl != null)
        {
            context.BackendServiceUri = _baseUrl.Execute(context) as string ?? throw new Exception($"base-url of '{Name}' can't be null.");
        }

        if (_backendId != null)
        {
            context.BackendServiceUri = _backendId.Execute(context) as string ?? throw new Exception($"backend-id of '{Name}' can't be null.");
        }
    }
}
