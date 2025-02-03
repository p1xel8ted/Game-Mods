using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shared;

/// <summary>
/// This file is shared between multiple projects and should be copied as a link. Any edits in this file will affect all projects using it.
/// </summary>
public static class Extensions
{
    //dictionary tryadd extension
    public static bool TryAdd<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
    {
        if (dictionary.ContainsKey(key))
            return false;
        dictionary.Add(key, value);
        return true;
    }


    public static void AddRange<T>(this SortedSet<T> set, IEnumerable<T> elements)
    {
        foreach (var element in elements)
        {
            set.Add(element);
        }
    }

    public static bool TryReplace(this string source, string oldValue, string newValue, out string result)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (oldValue == null)
            throw new ArgumentNullException(nameof(oldValue));

        // Perform the replacement.
        result = source.Replace(oldValue, newValue);

        // If no changes were made, string.Replace returns the original string reference.
        return !ReferenceEquals(source, result);
    }

    public static string GetPath(this GameObject obj)
    {
        return obj.transform.parent == null ? obj.name : obj.transform.parent.gameObject.GetPath() + "/" + obj.name;
    }

    public static bool Contains(this string source, string toCheck, StringComparison comparison)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (toCheck == null)
            throw new ArgumentNullException(nameof(toCheck));

        return source.IndexOf(toCheck, comparison) >= 0;
    }
}