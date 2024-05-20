using System.Xml.Linq;

namespace APIMSimulator.Policies;

internal class FragmentPolicy : Policy
{
    public FragmentPolicy(XElement element, PolicyBuilder builder)
        : base(element, builder)
    {
    }
}
