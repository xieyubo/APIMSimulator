using System.Xml.Linq;

namespace APIMSimulator.Policies;

internal class OtherwisePolicy : Policy
{
    public OtherwisePolicy(XElement element, PolicyBuilder builder)
        : base(element, builder)
    {
    }
}
