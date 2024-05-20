using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace APIMSimulator;

internal static class Extensions
{
    public static Dictionary<TKey, TValue> AddOrUpdate<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        where TKey : notnull
    {
        if (dict.Keys.Contains(key))
        {
            dict.Add(key, value);
        }
        else
        {
            dict[key] = value;
        }
        return dict;
    }

    public static void ForEach<T>(this IEnumerable<T> items, Action<T> act)
    {
        foreach (var item in items)
        {
            act(item);
        }
    }

    public static string GetRequiredAttribute(this XElement node, string attributeName)
    {
        var value = node.Attribute(attributeName)?.Value;
        if (string.IsNullOrEmpty(value))
        {
            throw new Exception($"attribute {attributeName} is missing");
        }
        return value!;
    }

    public static string? GetAttribute(this XElement node, string attributeName)
    {
        return node.Attribute(attributeName)?.Value;
    }

    public static string GetAttribute(this XElement node, string attributeName, string defaultValue, params string[] otherAllowedValue)
    {
        var value = node.Attribute(attributeName)?.Value;
        if (value == null)
        {
            return defaultValue;
        }

        if (value != defaultValue && otherAllowedValue?.Contains(value) != true)
        {
            throw new Exception($"Value '{value}' is not allowed in attribute '{attributeName}'");
        }
        return value;
    }
}
