using APIMSimulator.Policies;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace APIMSimulator;

public class PolicyBuilder
{
    internal Dictionary<string, FragmentPolicy> Fragments { get; } = new Dictionary<string, FragmentPolicy>();

    internal Policy Build(XElement element)
    {
        var policyName = GetPolicyName(element)!;
        if (!Policies.TryGetValue(policyName, out var policyType))
        {
            throw new Exception($"Policy '{element.Name}' is not supported.");
        }
        return (Policy)Activator.CreateInstance(policyType, [element, this]);
    }

    public T? TryParseNextPolicy<T>(ref XElement? element)
        where T : Policy
    {
        if (element == null)
        {
            return null;
        }
        var policyName = Policies.First(p => p.Value == typeof(T)).Key;
        if (GetPolicyName(element) != policyName)
        {
            return null;
        }

        var policy = (T)Build(element);
        element = element.NextNode as XElement;
        return policy;
    }

    public T[]? TryParseNextPolicies<T>(ref XElement? element)
        where T : Policy
    {
        var policies = new List<T>();
        for (var p = TryParseNextPolicy<T>(ref element); p != null; p = TryParseNextPolicy<T>(ref element))
        {
            policies.Add(p);
        }

        if (policies.Count() == 0)
        {
            return null;
        }
        return policies.ToArray();
    }

    public T BuildFromXmlFile<T>(string xmlFilePath)
        where T : Policy
    {
        return TryBuildFromXmlString<T>(File.ReadAllText(xmlFilePath)) ?? throw new Exception($"'{xmlFilePath}' doesn't contain valid {typeof(T).Name}.");
    }

    public T? TryBuildFromXmlString<T>(string xmlString)
       where T : Policy
    {
        XElement element;
        try
        {
            element = XElement.Parse(xmlString);
        }
        catch
        {
            element = XElement.Parse(PolicyUtils.ConvertPolicyToStandardXmlString(xmlString));
        }

        return Build(element) as T;
    }

    private string? GetPolicyName(XElement element)
    {
        return element?.Name.ToString().Replace("-", string.Empty).ToLower();
    }

    private static Dictionary<string, Type> Policies =
        Assembly
            .GetAssembly(typeof(Policy))!
            .GetTypes()
            .Where(t => !t.IsAbstract && typeof(Policy).IsAssignableFrom(t))
            .ToDictionary(t => (t.Name.EndsWith("Policy") ? t.Name.Remove(t.Name.Length - "Policy".Length) : t.Name).ToLower(), t => t);
}
