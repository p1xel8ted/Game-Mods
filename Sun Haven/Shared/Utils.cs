using UnityEngine.Experimental.Rendering;

namespace Shared;

public static class Utils
{
    private const float TimeBetweenNotifications = 5f;
    public static float BaseAspect => 16f / 9f;
    public static float CurrentAspect => (float)Screen.width / Screen.height;
    public static bool LargerAspect => CurrentAspect > BaseAspect;
    public static float PositiveScaleFactor => CurrentAspect / BaseAspect;
    public static float NegativeScaleFactor => 1f / PositiveScaleFactor;
    private static float LastNotificationTime { get; set; }
    private static string PreviousMessage { get; set; }

    public static void LogToPlayer(string message)
    {
        if (QuantumConsole.Instance is not null)
        {
            QuantumConsole.Instance._autoScroll = AutoScrollOptions.Never;
            QuantumConsole.Instance._maxStoredLogs = 100000;
            QuantumConsole.Instance.LogToConsoleAsync(message);
        }
    }
    public static bool DevOpsContinueExists(string guid)
    {
        return PluginInfos.Any(plugin => plugin.Key == guid);
    }
    
    public static void LoadTexture(Assembly executing, string button, ref Sprite buttonSprite)
    {
        using var stream = executing.GetManifestResourceStream(button);
        if (stream != null)
        {
            var imageData = new byte[stream.Length];
            var bytes = stream.Read(imageData, 0, imageData.Length);
            if (bytes != imageData.Length)
            {
                Debug.LogError("Error loading {button}!");
            }
            else
            {
                const TextureCreationFlags flags = new();
                var tex = new Texture2D(1, 1, GraphicsFormat.R8G8B8A8_UNorm, flags);
                tex.LoadImage(imageData);
                tex.filterMode = FilterMode.Point;
                tex.wrapMode = TextureWrapMode.Clamp;
                tex.wrapModeU = TextureWrapMode.Clamp;
                tex.wrapModeV = TextureWrapMode.Clamp;
                tex.wrapModeW = TextureWrapMode.Clamp;

                buttonSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
            }
        }
    }

    public static void SetActionBar(int index)
    {
        PlayerInput.AllowChangeActionBarItem = true;
        Player.Instance.PlayerInventory.SetActionBarSlot(index);
        Player.Instance.PlayerInventory.SetIndex(index);
    }

    public static float GetDistance(EnemyAI enemyAI)
    {
        return !Player.Instance ? float.MaxValue : ((Vector2) enemyAI.transform.position - Player.Instance.ExactGraphicsPosition).magnitude;
    }

    public static bool IsInFarmTile()
    {
        return TileManager.Instance.HasTileOrFarmingTile(Player.Instance.Position, ScenePortalManager.ActiveSceneIndex);
    }

    public static void Notify(string message, int id = 0, bool error = false)
    {
        if (message == PreviousMessage && Time.time - LastNotificationTime < TimeBetweenNotifications) return;
        LastNotificationTime = Time.time;
        PreviousMessage = message;
        SingletonBehaviour<NotificationStack>.Instance.SendNotification(message, id, 0, error);
    }

    /// <summary>
    /// Wraps an existing IEnumerator and skips any nested IEnumerator whose
    /// compiler-generated class name starts with “<methodName>d__…”.
    /// </summary>
    public static IEnumerator FilterByMethodName(IEnumerator original, params string[] methodNamesToRemove)
    {
        // Build a lookup of the compiler patterns we want to skip:
        // e.g. "<RunLogoAnimationRoutine>"
        var prefixes = methodNamesToRemove
            .Select(n => $"<{n}>")
            .ToArray();

        while (original.MoveNext())
        {
            var current = original.Current;

            // if this yield is itself an IEnumerator, check its generated name:
            if (current is IEnumerator nested)
            {
                var typeName = nested.GetType().Name;
                // if it matches any of our prefixes, skip it:
                if (prefixes.Any(pfx => typeName.StartsWith(pfx)))
                    continue;
            }

            yield return current;
        }
    }

    internal static IEnumerator FilterEnumerator(IEnumerator original, params Type[] typesToRemove)
    {
        // If you prefer passing in a List<Type>, just change the signature to
        // (IEnumerator original, IEnumerable<Type> typesToRemove) and remove the 'params' keyword.
        var filterSet = new HashSet<Type>(typesToRemove);
        while (original.MoveNext())
        {
            var current = original.Current;
            if (current != null && !filterSet.Contains(current.GetType()))
                yield return current;
        }
    }

    public static string GetNameByID(int id)
    {
        return Database.Instance.ids.FirstOrDefault(a => a.Value == id).Key;
    }

    public static ItemSellInfo GetItemSellInfo(int itemId)
    {
        var instance = SingletonBehaviour<ItemInfoDatabase>.Instance;
        if (instance)
        {
            return instance.allItemSellInfos.TryGetValue(itemId, out var itemSellInfo) ? itemSellInfo : null;
        }

        UnityEngine.Debug.LogError("ItemInfoDatabase instance is null", instance);
        return null;
    }

    public static int GetItemIdByName(string itemName)
    {
        return Database.Instance.ids[itemName];
    }

    public static ItemData GetItemDataByName(string name)
    {
        var id = Database.GetID(name);
        ItemData itemData = null;
        Database.GetData(id, delegate(ItemData data) { itemData = data; });
        return itemData;
    }

    public static ItemData GetItemData(int itemId)
    {
        ItemData item = null;
        Database.GetData(itemId, delegate(ItemData data) { item = data; });
        return item;
    }

    public static T GetItemData<T>(int itemId) where T : ItemData
    {
        T item = null;
        
        Database.GetData(itemId, delegate(T data) 
        { 
            item = data; 
        });

        return item;
    }

    public static bool CanUse(ToolData toolData)
    {
        return SingletonBehaviour<GameSave>.Instance.CurrentSave.characterData.Professions[toolData.profession].level >= toolData.requiredLevel;
    }

    public static void DestroyChildren(Transform transform)
    {
        foreach (Transform child in transform)
        {
            UnityEngine.Object.Destroy(child.gameObject);
        }
    }

    public static void ConfigureCanvasScaler(CanvasScaler canvasScaler, CanvasScaler.ScaleMode scaleMode, float scaleFactor)
    {
        if (canvasScaler is null)
        {
            // Plugin.LOG.LogWarning($"ConfigureCanvasScaler: canvasScaler is null!");
            return;
        }

        canvasScaler.uiScaleMode = scaleMode;
        canvasScaler.scaleFactor = scaleFactor;
    }

    public static void SendNotification(string message)
    {
        if (NotificationStack.Instance is not null)
        {
            SingletonBehaviour<NotificationStack>.Instance.SendNotification(message);
        }
    }
}