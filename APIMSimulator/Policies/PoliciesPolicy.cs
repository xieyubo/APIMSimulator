using System;
using System.Linq;
using System.Net;
using System.Xml.Linq;

namespace APIMSimulator.Policies;

public class PoliciesPolicy : Policy
{
    private InboundPolicy _inbound;
    private BackendPolicy _backend;
    private OutboundPolicy _outbond;
    private OnErrorPolicy _onError;

    public PoliciesPolicy(XElement element, PolicyBuilder builder)
        : base(element, builder)
    {
        _inbound = (InboundPolicy)ChildPolicies.Single(p => p is InboundPolicy);
        _backend = (BackendPolicy)ChildPolicies.Single(p => p is BackendPolicy);
        _outbond = (OutboundPolicy)ChildPolicies.Single(p => p is OutboundPolicy);
        _onError = (OnErrorPolicy)ChildPolicies.Single(p => p is OnErrorPolicy);

        if (ChildPolicies.Count() != 4)
        {
            throw new Exception("policies must contain inbound, backend, outbond, onerror policies.");
        }
    }

    internal PoliciesPolicy? Parent
    {
        set
        {
            _inbound.Parent = value?._inbound;
            _backend.Parent = value?._backend;
            _outbond.Parent = value?._outbond;
            _onError.Parent = value?._onError;
        }
    }

    internal override void Execute(Context context)
    {
        try
        {
            _inbound.Execute(context);
            _backend.Execute(context);
            _outbond.Execute(context);
        }
        catch (Exception ex) when (ex is ReturnResponseException)
        {
            throw;
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.Body = new HttpMessageBody() { Body = ex.Message };
            _onError.Execute(context);
        }
    }
}
