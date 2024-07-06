using System.Linq;
using PSS;
using QFSW.QC;
using UnityEngine;
using UnityEngine.UI;
using Wish;


namespace Shared;

public static class Utils
{
    public static float BaseAspect => 16f / 9f;
    public static float CurrentAspect => (float) Screen.width / Screen.height;
    public static bool LargerAspect => CurrentAspect > BaseAspect;
    public static float PositiveScaleFactor => CurrentAspect / BaseAspect;
    public static float NegativeScaleFactor => 1f / PositiveScaleFactor;

    public static void LogToPlayer(string message)
    {
        if (QuantumConsole.Instance is not null)
        {
            QuantumConsole.Instance._autoScroll = AutoScrollOptions.Never;
            QuantumConsole.Instance._maxStoredLogs = 100000;
            QuantumConsole.Instance.LogToConsoleAsync(message);
        }
    }

    public static string GetNameByID(int id)
    {
        return Database.Instance.ids.FirstOrDefault(a => a.Value == id).Key;
    }

    public static ItemSellInfo GetItemSellInfo(int itemId)
    {
        return SingletonBehaviour<ItemInfoDatabase>.Instance.allItemSellInfos.TryGetValue(itemId, out var itemSellInfo) ? itemSellInfo : null;
    }
    public static int GetItemIdByName(string itemName)
    {
        return Database.Instance.ids[itemName];
    }

    public static ItemData GetItemDataByName(string name)
    {
        var id = Database.GetID(name);
        ItemData itemData = null;
        Database.GetData(id, delegate(ItemData data)
        {
            itemData = data;
        });
        return itemData;
    }

    public static ItemData GetItemData(int itemId)
    {
        ItemData item = null;
        Database.GetData(itemId, delegate(ItemData data)
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
            Object.Destroy(child.gameObject);
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