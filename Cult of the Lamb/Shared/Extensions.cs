using System;
using System.Collections.Generic;

namespace Shared;

/// <summary>
/// This file is shared between multiple projects and should be copied as a link. Any edits in this file will affect all projects using it.
/// </summary>
public static class Extensions
{
    public static void AddRange<T>(this SortedSet<T> set, IEnumerable<T> elements)
    {
        foreach (var element in elements)
        {
            set.Add(element);
        }
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