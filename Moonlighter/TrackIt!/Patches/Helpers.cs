using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace TrackIt.Patches;

public static class Helpers
{
    private static List<SpriteRenderer> EnemyQuestMarkers { get; } = new();
    private static List<SpriteRenderer> ItemStackWorldMarkers { get; } = new();
    private static List<Image> ItemStackInventoryMarkers { get; } = new();
    private const string EnemyParent = "HealthBar(Clone)";

    private static GameObject ItemStackMarkerExists(ItemStack itemStack, string markerName)
    {
        var parentTransform = itemStack.depthOrderer.transform;
        return parentTransform.Find(markerName)?.gameObject;
    }

    internal static void UpdateItemStackMarkerPositions(ItemStack itemStack)
    {
        var wishMarker = ItemStackMarkerExists(itemStack, Plugin.WishIconSpriteString);
        var questMarker = ItemStackMarkerExists(itemStack, Plugin.QuestIcon);

        if (wishMarker is not null && questMarker is not null)
        {
            wishMarker.transform.localPosition = new Vector3(-10, 20, 0);
            questMarker.transform.localPosition = new Vector3(10, 20, 0);
        }
        else if (wishMarker is not null)
        {
            wishMarker.transform.localPosition = new Vector3(0, 20, 0);
        }
        else if (questMarker is not null)
        {
            questMarker.transform.localPosition = new Vector3(0, 20, 0);
        }
    }


    private static void SetItemStackMarkerActiveStatus(ItemStack itemStack, string markerName, bool status)
    {
        var marker = ItemStackMarkerExists(itemStack, markerName);
        marker?.SetActive(status);
    }

    private static void SetItemStackMarkersActive(ItemStack itemStack)
    {
        SetItemStackMarkerActiveStatus(itemStack, Plugin.WishIconSpriteString, true);
        SetItemStackMarkerActiveStatus(itemStack, Plugin.QuestIcon, true);
    }

    internal static void SetItemStackMarkersInactive(ItemStack itemStack)
    {
        SetItemStackMarkerActiveStatus(itemStack, Plugin.WishIconSpriteString, false);
        SetItemStackMarkerActiveStatus(itemStack, Plugin.QuestIcon, false);
    }

    internal static void AttachMarkerToItemStack(ItemStack itemStack)
    {
        var parentTransform = itemStack.depthOrderer.transform;

        var wish = IsWishlisted(itemStack.master);
        var quest = IsQuestItem(itemStack.master);

        if (!wish && !quest) return;

        Plugin.LOG.LogInfo($"{itemStack.master.name} is {(wish && quest ? "both a wish and a quest" : wish ? "a wish" : "a quest")} item!");

        if (wish)
        {
            var w = ItemStackMarkerExists(itemStack, Plugin.WishIconSpriteString);
            if (w is null)
            {
                CreateWishSpriteObjectForItemStack(parentTransform);
            }
            else
            {
                w.SetActive(true);
            }
        }

        if (quest)
        {
            var q = ItemStackMarkerExists(itemStack, Plugin.QuestIcon);
            if (q is null)
            {
                CreateQuestSpriteObjectForItemStack(parentTransform);
            }
            else
            {
                q.SetActive(true);
            }
        }

        UpdateItemStackMarkerPositions(itemStack);
    }

    private static Transform FindDeepChild(this Transform parent, string name)
    {
        foreach (var c in parent)
        {
            var child = (Transform) c;
            if (child is not null && child.name == name)
                return child;
            var result = child.FindDeepChild(name);
            if (result is not null)
                return result;
        }

        return null;
    }

    private static void UpdateQuestMarkerColours()
    {
        try
        {
            var removedEnemyQuestMarkers = EnemyQuestMarkers.RemoveAll(s => s is null);
            var removedItemStackWorldMarkers = ItemStackWorldMarkers.RemoveAll(s => s is null);
            var removedItemStackInventoryMarkers = ItemStackInventoryMarkers.RemoveAll(s => s is null);
            
            Plugin.LOG.LogInfo($"Removed {removedEnemyQuestMarkers} null EnemyQuestMarkers");
            Plugin.LOG.LogInfo($"Removed {removedItemStackWorldMarkers} null ItemStackWorldMarkers");
            Plugin.LOG.LogInfo($"Removed {removedItemStackInventoryMarkers} null ItemStackInventoryMarkers");
            
            var sprite = GetQuestIconSpriteOfChosenColour();
            foreach (var s in EnemyQuestMarkers.ToArray().Where(s => s is not null))
            {
                s.sprite = sprite;
            }

            foreach (var s in ItemStackWorldMarkers.ToArray().Where(s => s is not null))
            {
                s.sprite = sprite;
            }

            foreach (var i in ItemStackInventoryMarkers.ToArray().Where(s => s is not null))
            {
                i.sprite = sprite;
            }
        }
        catch (Exception e)
        {
            Plugin.LOG.LogError("Cannot find quest icon sprite. Did you die and lose the item?");
            Plugin.LOG.LogError(e.StackTrace);
        }
    }


    public static void AttachQuestMarkerToEnemy(Enemy enemy)
    {
        var parent = enemy.transform.FindDeepChild(EnemyParent);
        if (parent is null)
        {
            Plugin.LOG.LogError($"{EnemyParent} not found in enemy GameObject");
            return;
        }

        var spriteObject = new GameObject(Plugin.QuestIcon);
        var spriteRenderer = spriteObject.AddComponent<SpriteRenderer>();


        spriteRenderer.sprite = GetQuestIconSpriteOfChosenColour();
        spriteObject.transform.SetParent(parent, false);
        spriteObject.transform.localPosition = new Vector3(0, 10, 0); // Adjust position as needed
        spriteObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        spriteRenderer.sortingOrder = 5000; // Adjust sorting order to a high value to ensure it's rendered on top
        EnemyQuestMarkers.Add(spriteRenderer);
    }


    private static void CreateQuestSpriteObjectForItemStack(Transform parentTransform)
    {
        var spriteObject = new GameObject(Plugin.QuestIcon);
        var spriteRenderer = spriteObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = GetQuestIconSpriteOfChosenColour();

        var spriteOrder = spriteObject.AddComponent<SpriteDepthOrderRender>();
        spriteOrder.sprite = spriteRenderer;
        spriteOrder.forcedOrderValue = 5000;
        spriteOrder.forceOrderValue = true;

        spriteObject.transform.SetParent(parentTransform);
        spriteObject.transform.localPosition = new Vector3(0, 20, 0);
        spriteObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        spriteRenderer.sortingOrder = 5000;
        ItemStackWorldMarkers.Add(spriteRenderer);
    }

    private static void CreateWishSpriteObjectForItemStack(Transform parentTransform)
    {
        var spriteObject = new GameObject(Plugin.WishIconSpriteString);
        var spriteRenderer = spriteObject.AddComponent<SpriteRenderer>();

        var spriteOrder = spriteObject.AddComponent<SpriteDepthOrderRender>();
        spriteOrder.sprite = spriteRenderer;
        spriteOrder.forcedOrderValue = 5000;
        spriteOrder.forceOrderValue = true;

        spriteRenderer.sprite = ResourceLoader.LoadSpriteFromEmbeddedResourceByName("WishIcon.png");
        spriteObject.transform.SetParent(parentTransform);
        spriteObject.transform.localPosition = new Vector3(0, 20, 0);
        spriteObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    private static bool IsWishlisted(ItemMaster itemMaster)
    {
        return HeroMerchant.Instance.wishlistManager.IsIngredient(itemMaster);
    }


    public static bool IsQuestEnemy(PrefabSpawnInfo enemy)
    {
        return QuestTracker.GetQuests().Select(quest => quest).Any(q => q.quest.killQuestTarget == enemy.name || q.quest.target == enemy.name);
    }

    private static bool IsQuestItem(ItemMaster itemMaster)
    {
        var currentGamePlusLevel = GameManager.Instance.GetCurrentGamePlusLevel();
        return (from q in QuestTracker.GetQuests().Select(quest => quest)
            let one = ItemDatabase.GetItemByName(q.quest.target, currentGamePlusLevel)
            let two = ItemDatabase.GetItemByName(q.quest.killQuestTarget, currentGamePlusLevel)
            where one == itemMaster || two == itemMaster
            select one).Any();
    }

    private static GameObject FindQuestIcon(Component parent)
    {
        var transforms = parent.GetComponentsInChildren<Transform>();
        return transforms.FirstOrDefault(t => t.name is Plugin.QuestIcon)?.gameObject;
    }

    public static void UpdateQuestIcons()
    {
        var inventorySlots = Object.FindObjectsOfType<InventorySlotGUI>();
        foreach (var slot in inventorySlots)
        {
            UpdateQuestIconPositionInInventories(slot);
            UpdateQuestIconVisibilityInInventories(slot);
        }

        UpdateQuestMarkerColours();
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal static GameObject CreateAndAttachQuestSprite(InventorySlotGUI __instance)
    {
        var parent = __instance.transform;
        var originalRect = __instance.imageWishlisted.rectTransform;

        var questIcon = FindQuestIcon(parent);
        RectTransform questRect;
        Image questImage;

        if (questIcon is null)
        {
            questIcon = new GameObject(Plugin.QuestIcon);
            questIcon.transform.SetParent(parent, false);
            questImage = questIcon.AddComponent<Image>();
            questRect = questIcon.GetComponent<RectTransform>();
        }
        else
        {
            questImage = questIcon.GetComponent<Image>();
            questRect = questIcon.GetComponent<RectTransform>();
        }

        questRect.anchorMin = originalRect.anchorMin;
        questRect.anchorMax = originalRect.anchorMax;
        questRect.anchoredPosition = originalRect.anchoredPosition;
        questRect.sizeDelta = originalRect.sizeDelta;
        questRect.pivot = originalRect.pivot;

        questImage.sprite = GetQuestIconSpriteOfChosenColour();


        questIcon.transform.localScale = new Vector3(0.75f, 0.75f, 1f);
        questIcon.transform.localPosition = new Vector3(-12, -12, 1f);

        var newTransform = __instance.imageWishlisted.transform;
        newTransform.localPosition = new Vector3(-12, 12, 1f);
        newTransform.localScale = new Vector3(0.75f, 0.75f, 1f);
        ItemStackInventoryMarkers.Add(questImage);
        return questIcon;
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    private static void UpdateQuestIconPositionInInventories(InventorySlotGUI __instance)
    {
        var questIcon = FindQuestIcon(__instance.transform) ?? CreateAndAttachQuestSprite(__instance);

        questIcon.transform.localPosition = new Vector3(-12, -12, 1f);
    }

    internal static void UpdateQuestIconVisibilityInInventories(InventorySlotGUI slot)
    {
        var questIcon = FindQuestIcon(slot.transform) ?? CreateAndAttachQuestSprite(slot);

        questIcon.SetActive(false);

        if (slot.item is null)
        {
            questIcon.SetActive(false);
            return;
        }

        if (!IsQuestItem(slot.item)) return;

        Plugin.LOG.LogInfo($"Quest found for {slot.item.name}");
        questIcon.SetActive(true);

        UpdateQuestMarkerColours();
    }

    internal static Sprite GetQuestIconSpriteOfChosenColour()
    {
        var questIconColor = Plugin.ChosenQuestIconColor switch
        {
            Plugin.QuestIconColor.Red => ResourceLoader.LoadSpriteFromEmbeddedResourceByName("QuestIcon_Red.png"),
            Plugin.QuestIconColor.Blue => ResourceLoader.LoadSpriteFromEmbeddedResourceByName("QuestIcon_Blue.png"),
            Plugin.QuestIconColor.Magenta => ResourceLoader.LoadSpriteFromEmbeddedResourceByName("QuestIcon_Magenta.png"),
            Plugin.QuestIconColor.Orange => ResourceLoader.LoadSpriteFromEmbeddedResourceByName("QuestIcon_Orange.png"),
            Plugin.QuestIconColor.White => ResourceLoader.LoadSpriteFromEmbeddedResourceByName("QuestIcon_White.png"),
            _ => ResourceLoader.LoadSpriteFromEmbeddedResourceByName("QuestIcon_Red.png")
        };
        return questIconColor;
    }
}