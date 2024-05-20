using System;
using System.Xml.Linq;

namespace APIMSimulator.Policies;

internal class CacheLookupValuePolicy : Policy
{
    private string _cachingType;
    private Expression? _defaultValue;
    private Expression _key;
    private string _variableName;

    public CacheLookupValuePolicy(XElement element, PolicyBuilder builder)
        : base(element, builder)
    {
        _cachingType = element.GetAttribute("caching-type", "prefer-external", "internal", "external");

        var defaultValue = element.GetAttribute("default-value");
        if (defaultValue != null)
        {
            _defaultValue = new Expression(defaultValue);
        }

        _key = new Expression(element.GetRequiredAttribute("key"));

        _variableName = element.GetRequiredAttribute("variable-name");
    }

    internal override void Execute(Context context)
    {
        if (_defaultValue != null)
        {
            var defaultValue = _defaultValue.Execute(context) ?? throw new Exception("default value of cache-lookup-value can't be null");
            context.Variables.AddOrUpdate(_variableName, defaultValue);
        }
    }
}
