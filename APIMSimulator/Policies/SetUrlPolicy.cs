using System;
using System.Xml.Linq;

namespace APIMSimulator.Policies;

internal class SetUrlPolicy : ValuePolicy
{
    public SetUrlPolicy(XElement element, PolicyBuilder builder)
        : base(element, builder)
    {
    }

    public void Caculate(Context context, HttpRequest request)
    {
        request.Url = CaculateRequired<string>(context);
    }
}
