namespace GYKHelper;

public static class Extensions
{

    //dictionary T tryAdd
    public static bool TryAdd<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
    {
        if (dictionary.ContainsKey(key)) return false;
        dictionary.Add(key, value);
        return true;
    }
    
    //component try add
    public static T TryAddComponent<T>(this GameObject gameObject) where T : Component
    {
        var component = gameObject.GetComponent<T>();
        if (component == null)
        {
            component = gameObject.AddComponent<T>();
        }
        return component;
    }

    private static CultureInfo LanguageCultureInfo
    {
        get
        {
            var systemLanguage = CultureInfo.CurrentCulture;
            var gameLang = GameSettings.GetCurrentLanguage().Replace('_', '-').ToLower(CultureInfo.InvariantCulture).Trim();
            var culture = string.IsNullOrWhiteSpace(gameLang) ? systemLanguage : new CultureInfo(gameLang);
            return culture;
        }
    }

    /// <summary>
    /// Checks if a string contains another string, taking into account the current game language. 
    /// If the game language is not set, the system language is used.
    /// </summary>
    /// <param name="source">The string to search within.</param>
    /// <param name="toCheck">The string to search for.</param>
    /// <returns>True if the 'source' string contains the 'toCheck' string, considering the specified or default culture; otherwise, false.</returns>
    public static bool ContainsByLanguage(this string source, string toCheck)
    {
        return LanguageCultureInfo.CompareInfo.IndexOf(source, toCheck, CompareOptions.IgnoreCase) >= 0;
    }

    /// <summary>
    /// Checks if two strings are equal, based on the current game language. If the game language is null, it uses the system language.
    /// </summary>
    /// <param name="source">The first string to compare.</param>
    /// <param name="toCompare">The second string to compare.</param>
    /// <returns>True if the two strings are equal considering the specified or default culture; otherwise, false.</returns>
    public static bool EqualsByLanguage(this string source, string toCompare)
    {
        return LanguageCultureInfo.CompareInfo.Compare(source, toCompare, CompareOptions.IgnoreCase) == 0;
    }

    public static bool StartsWithByLanguage(this string source, string toCompare)
    {
        return source.StartsWith(toCompare, true, LanguageCultureInfo);
    }

    public static bool EndsWithByLanguage(this string source, string toCompare)
    {
        return source.EndsWith(toCompare, true, LanguageCultureInfo);
    }

}