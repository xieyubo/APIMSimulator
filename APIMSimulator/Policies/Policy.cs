using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace APIMSimulator.Policies;

public abstract class Policy
{
    public IEnumerable<T> FindPolicy<T>(IReadOnlyDictionary<string, string>? attributes = null)
        where T : Policy
    {
        var results = new List<T>();
        var trace = new List<string>();
        FindPolicy(results, attributes);
        return results;
    }

    public virtual void FindPolicy<T>(List<T> results, IReadOnlyDictionary<string, string>? attributes)
        where T : Policy
    {
        if (GetType() == typeof(T) &&
            (attributes == null ||
             attributes.All(targetAttr => Element.Attributes().Any(attr => attr.Name.ToString() == targetAttr.Key && attr.Value == targetAttr.Value))))
        {
            results.Add((T)this);
        }

        ChildPolicies.ForEach(p => p.FindPolicy(results, attributes));
    }

    public string Name { get; }

    internal XElement Element { get; }

    internal virtual void Execute(Context context)
    {
        ChildPolicies.ForEach(e => e.Execute(context));
    }

    protected List<Policy> ChildPolicies { get; private set; }

    protected Policy(XElement element, PolicyBuilder builder)
        : this(element, builder, parseChildPolicies: true)
    {
    }

    protected Policy(XElement element, PolicyBuilder builder, bool parseChildPolicies)
    {
        Element = element;
        Name = element.Name.ToString();
        ChildPolicies = parseChildPolicies ? element.Elements().Select(e => builder.Build((XElement)e)).ToList() : new ();
    }

    protected T? GetNextChildPolicy<T>(ref int index)
        where T : Policy
    {
        if (index >= ChildPolicies.Count())
        {
            return null;
        }

        var policy = ChildPolicies[index] as T;
        if (policy != null)
        {
            ++index;
        }
        return policy;
    }

    protected T? GetNextChildPolicy<T>(ref int index, bool isRequired = false, string? errorMessage = null)
        where T : Policy
    {
        var policy = GetNextChildPolicy<T>(ref index);
        if (policy == null && isRequired)
        {
            throw new Exception(errorMessage);
        }
        return policy;
    }

    protected T[]? GetNextChildPolicies<T>(ref int index, bool isRequired = false, string? errorMessage = null)
        where T: Policy
    {
        var policies = new List<T>();
        for (var p = GetNextChildPolicy<T>(ref index); p != null; p = GetNextChildPolicy<T>(ref index))
        {
            policies.Add(p);
        }

        if (policies.Count() == 0)
        {
            if (isRequired)
            {
                throw new Exception(errorMessage);
            }
            return null;
        }
        return policies.ToArray();
    }
}
