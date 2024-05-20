using System.Collections.Generic;

namespace APIMSimulator.Abstract;

public static class IReadOnlyDictionaryExtensions
{
    public static string GetValueOrDefault(this IReadOnlyDictionary<string, string[]> dict, string key, string defaultValue)
    {
        var val = dict.TryGetValue(key, out var value) ? string.Join(",", value) : defaultValue;
        return val;
    }
}
