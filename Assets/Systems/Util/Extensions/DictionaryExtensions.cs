using System.Collections.Generic;

public static class DictionaryExtensions
{
    public static void AddOrOverwriteKey<T, K>(this Dictionary<T, K> dictionary, T key, K value)
    {
        if (!dictionary.TryAdd(key, value))
        {
            dictionary[key] = value;
        }
    }
}
