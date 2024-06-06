using System;
using System.Xml.Linq;

namespace APIMSimulator.Policies;

public class ForwardRequestPolicy : Policy
{
    public interface ICallback
    {
        HttpResponse OnSendRequest(HttpRequest request);
    }

    public ICallback? Callback { get; set; }

    public ForwardRequestPolicy(XElement element, PolicyBuilder builder)
        : base(element, builder, parseChildPolicies: false)
    {
    }

    internal override void Execute(Context context)
    {
        if (!string.IsNullOrEmpty(context.BackendServiceUri))
        {
            var originUri = new UriBuilder(context.Request.Url);
            var ub = new UriBuilder(context.BackendServiceUri)
            {
                Path = originUri.Path,
                Query = originUri.Query
            };
            context.Request.Url = ub.Uri.ToString();
            context.Response = Callback?.OnSendRequest(context.Request) ?? throw new Exception($"No callback for '{Name}' policy.");
        }
    }
}
