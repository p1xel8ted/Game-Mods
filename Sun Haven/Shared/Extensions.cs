namespace Shared;

public static class Extensions
{

    public static void Destroy(this GameObject go, bool immediate = false)
    {
        if (immediate)
        {
            UnityEngine.Object.DestroyImmediate(go);      
        }
        else
        {
            UnityEngine.Object.Destroy(go);   
        }
       
    }
    public static void SetAnchoredPosition(this Transform transform, Vector2 position)
    {
        if (transform == null) return;
        var rectTransform = transform.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = position;
        }
        else
        {
            transform.localPosition = new Vector3(position.x, position.y, transform.localPosition.z);
        }
    }

    public static void SetLocalPosition(this Transform transform, Vector2 position)
    {
        transform.localPosition = new Vector3(position.x, position.y, transform.localPosition.z);
    }

    public static void SetLocalPosition(this GameObject gameObject, Vector2 position)
    {
        gameObject.transform.localPosition = new Vector3(position.x, position.y, gameObject.transform.localPosition.z);
    }

    public static void SetAnchoredPosition(this GameObject gameObject, Vector2 position)
    {
        var rectTransform = gameObject.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = position;
        }
        else
        {
            gameObject.transform.localPosition = new Vector3(position.x, position.y, gameObject.transform.localPosition.z);
        }
    }

#if BepInEx_IL2CPP
    public static void AddListener(this UnityEvent action, Action listener)
    {
        action.AddListener(listener);
    }

    public static void AddListener<T>(this UnityEvent<T> action, Action<T> listener)
    {
        action.AddListener(listener);
    }

    public static void RemoveListener(this UnityEvent action, Action listener)
    {
        action.RemoveListener(listener);
    }

    public static void RemoveListener<T>(this UnityEvent<T> action, Action<T> listener)
    {
        action.RemoveListener(listener);
    }
#endif

    public static string GetGameObjectPath(this GameObject obj)
    {
        var path = obj.name;
        var parent = obj.transform.parent;
        while (parent != null)
        {
            path = parent.name + "/" + path;
            parent = parent.parent;
        }

        return path;
    }

    // Extension method to find all child transforms with the given name.
    public static List<Transform> FindChildrenByName(this Transform parent, string name)
    {
        List<Transform> foundChildren = [];
        FindChildrenByNameRecursive(parent, name, ref foundChildren);
        return foundChildren;
    }

    // Extension method to find the first child transform with the given name.
    public static Transform FindFirstChildByName(this Transform parent, string name)
    {
        return FindFirstChildByNameRecursive(parent, name);
    }

    // Recursive helper function to search for the first child with the given name.
    private static Transform FindFirstChildByNameRecursive(Transform current, string name)
    {
        foreach (var o in current)
        {
#if BepInEx_IL2CPP
            var child = o.TryCast<Transform>();
#else
            var child = (Transform)o;
#endif
            if (child != null && child.name == name)
            {
                return child;
            }

            var foundChild = FindFirstChildByNameRecursive(child, name);
            if (foundChild != null)
            {
                return foundChild;
            }
        }

        // Return null if no child with the given name is found.
        return null;
    }

    // Recursive helper function to search for children with the given name.
    private static void FindChildrenByNameRecursive(Transform current, string name, ref List<Transform> foundChildren)
    {
        foreach (var o in current)
        {
#if BepInEx_IL2CPP
            var child = o.TryCast<Transform>();
#else
            var child = (Transform)o;
#endif
            if (child != null && child.name == name)
            {
                foundChildren.Add(child);
            }

            // Recurse into each child.
            FindChildrenByNameRecursive(child, name, ref foundChildren);
        }
    }

    public static bool TryGetComponentsInChildren<T>(this Transform transform, out List<T> components) where T : Component
    {
        components = [..transform.GetComponentsInChildren<T>()];
        return components != null;
    }


    public static bool TryGetChildren<T>(this Transform transform, out List<T> components) where T : Component
    {
        components = [..transform.GetComponentsInChildren<T>()];
        return components != null;
    }


    public static bool TryGetComponents<T>(this Transform transform, out List<T> components) where T : Component
    {
        components = [..transform.GetComponents<T>()];
        return components != null;
    }

    public static bool TryGetComponentInChildren<T>(this Transform transform, out T component) where T : Component
    {
        component = transform.GetComponentInChildren<T>();
        return component != null;
    }

    public static bool TryGetComponent<T>(this Transform transform, out T component) where T : Component
    {
        component = transform.GetComponent<T>();
        return component != null;
    }

    public static bool TryGetComponentsInChildren<T>(this GameObject gameObject, out List<T> components) where T : Component
    {
        components = [..gameObject.GetComponentsInChildren<T>()];
        return components != null;
    }

    public static bool TryGetComponents<T>(this GameObject gameObject, out List<T> components) where T : Component
    {
        components = [..gameObject.GetComponents<T>()];
        return components != null;
    }

    public static bool TryGetComponentInChildren<T>(this GameObject gameObject, out T component) where T : Component
    {
        component = gameObject.GetComponentInChildren<T>();
        return component != null;
    }

    public static bool TryGetComponent<T>(this GameObject gameObject, out T component) where T : Component
    {
        component = gameObject.GetComponent<T>();
        return component != null;
    }
    
    public static T TryAddComponent<T>(this GameObject gameObject) where T : Component
    {
        var component = gameObject.GetComponent<T>();
        if (component == null)
        {
            component = gameObject.AddComponent<T>();
        }

        return component;
    }

    public static bool Contains(this string source, string toCheck, StringComparison comp = StringComparison.InvariantCultureIgnoreCase)
    {
        return source?.IndexOf(toCheck, comp) >= 0;
    }

    public static bool TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
    {
        if (dictionary == null)
        {
            throw new ArgumentNullException(nameof(dictionary));
        }

        if (dictionary.ContainsKey(key)) return false;
        dictionary.Add(key, value);
        return true;
    }

    public static Dictionary<string, T> ToCaseInsensitiveDictionary<T>(this Dictionary<string, T> dictionary)
    {
        return new Dictionary<string, T>(dictionary, StringComparer.OrdinalIgnoreCase);
    }

    // case-insensitive TryGetValue
    public static bool TryGetValueIgnoreCase<TValue>(this Dictionary<string, TValue> dictionary, string key, out TValue value)
    {
        if (dictionary == null)
        {
            throw new ArgumentNullException(nameof(dictionary));
        }

        foreach (var pair in dictionary.Where(pair => pair.Key.Equals(key, StringComparison.OrdinalIgnoreCase)))
        {
            value = pair.Value;
            return true;
        }

        value = default;
        return false;
    }

    public static string GetCaseSensitiveKey<TValue>(this Dictionary<string, TValue> dictionary, string key)
    {
        if (dictionary == null)
        {
            throw new ArgumentNullException(nameof(dictionary));
        }

        return dictionary.Where(pair => pair.Key.Equals(key, StringComparison.OrdinalIgnoreCase)).Select(pair => pair.Key).FirstOrDefault();
    }
}