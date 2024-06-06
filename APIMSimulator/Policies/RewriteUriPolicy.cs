using System;
using System.Collections.Generic;
using System.Web;
using System.Xml.Linq;

namespace APIMSimulator.Policies;

internal class RewriteUriPolicy : Policy
{
    private Expression _template;
    private Expression? _copyUnmatchedParams;

    public RewriteUriPolicy(XElement element, PolicyBuilder builder)
        : base(element, builder)
    {
        _template = new Expression(element.GetRequiredAttribute("template"));

        var attr = element.GetAttribute("copy-unmatched-params");
        if (!string.IsNullOrEmpty(attr))
        {
            _copyUnmatchedParams = new Expression(attr!);
        }
    }

    internal override void Execute(Context context)
    {
        var template = _template.Execute(context) as string ?? throw new Exception($"'template' of '{Name}' can't be null.");

        bool? copyUnmatchedParamsValue = _copyUnmatchedParams?.Execute(context) as bool?;
        bool copyUnmatchedParams = copyUnmatchedParamsValue == null ? true : (bool)copyUnmatchedParamsValue;

        var unmatchedParams = new List<(string, string)>();
        var originalUri = new Uri(context.Request.Url);
        var queries = HttpUtility.ParseQueryString(originalUri.Query);
        foreach (var key in queries.AllKeys)
        {
            var value = queries[key];
            var tempQuery = $"{{{key}}}";
            if (template.IndexOf(tempQuery) >= 0)
            {
                template.Replace(tempQuery, value);
            }
            else
            {
                unmatchedParams.Add((key, value));
            }
        }

        var uri = Uri.IsWellFormedUriString(template, UriKind.Absolute) ? template : new Uri(originalUri, template).ToString();
        if (unmatchedParams.Count > 0 && copyUnmatchedParams)
        {
            uri += (uri.IndexOf("?") < 0 ? "?" : "&");

            foreach (var (key, value) in unmatchedParams)
            {
                uri += $"{Uri.EscapeUriString(key)}={Uri.EscapeUriString(value)}&";
            }

            uri = uri.Substring(0, uri.Length - 1);
        }

        context.Request.Url = uri;
    }
}
