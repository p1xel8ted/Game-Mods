using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using DG.Tweening;
using HarmonyLib;
using I2.Loc;
using TrackIt.Utilities;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace TrackIt.Patches
{
    [HarmonyPatch]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class QuestTracker
    {
        private const string QuestHeader = "QuestHeader";
        private const string QuestTrackerName = "QuestTracker";
        private const string Quest = "Quest";
        private const string SalesBox = "SalesBox";
        private const string MaxActiveQuests = "maxActiveQuests";
        private static Localize QuestTargetText { get; set; }
        private static Text HeaderText { get; set; }
        internal static GameObject QuestTrackerObject { get; private set; }
        private static List<ItemStack> AllShopItems { get; } = [];
        private static List<ItemStack> AllPlayerItems { get; } = [];
        private static Dictionary<Quest, string> KillQuestTargetItems { get; } = [];
        private static Dictionary<Quest, string> QuestTargetItems { get; } = [];
        private static Dictionary<ActiveQuest, Text> QuestTexts { get; } = [];
        private static Dictionary<Text, Image> QuestTextImages { get; } = [];
        private static List<Tweener> QuestTargetAnimations { get; } = [];
        private static WriteOnce<Font> OriginalFont { get; } = new();


        [HarmonyPostfix]
        [HarmonyPatch(typeof(HeroMerchant), nameof(HeroMerchant.CompleteQuest))]
        [HarmonyPatch(typeof(QuestPanel), nameof(QuestPanel.OnQuestAccept))]
        [HarmonyPatch(typeof(QuestPanel), nameof(QuestPanel.OnQuestCancel))]
        public static void Quest_Updates()
        {
            Plugin.QuestTrackerNeedsRebuilding = true;
            Plugin.QuestTrackerNeedsUpdating = true;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(QuestVisitorMind), nameof(QuestVisitorMind.AddSelectedQuest))]
        public static void QuestVisitorMind_AddSelectedQuest(ref QuestVisitorMind __instance)
        {
            __instance.selectedQuest.daysToComplete = Plugin.DaysToComplete.Value;
            __instance._activeQuest.quest.daysToComplete = Plugin.DaysToComplete.Value;
            Plugin.QuestTrackerNeedsRebuilding = true;
            Plugin.QuestTrackerNeedsUpdating = true;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Chest), nameof(Chest.SetChestFromSave))]
        public static void Chest_SetChestFromSave(ref Chest __instance)
        {
            if (!__instance.name.Equals(SalesBox))
            {
                foreach (var chest in ShopManager.Instance.references.shopChests)
                {
                    chest.OpenChest();
                    chest.CloseChest();
                }
            }

            PerformInventoryUpdate();
        }


        [HarmonyPostfix]
        [HarmonyPatch(typeof(ItemStack), nameof(ItemStack.SendAddedToHeroInventoryEvent))]
        public static void ItemStack_SendAddedToHeroInventoryEvent()
        {
            PerformInDungeonOrTownUpdate();
        }

        private static void PerformInventoryUpdate(bool heroInit = false)
        {
            if (ShopManager.Instance is not null)
            {
                if (heroInit)
                {
                    foreach (var chest in ShopManager.Instance.references.shopChests)
                    {
                        chest.OpenChest();
                        chest.CloseChest();
                    }
                }

                AllShopItems.Clear();
                AllShopItems.AddRange(ShopManager.Instance.references.salesBox.GetAllItems().ToArray());
                if (ShopManager.Instance.bedChest is not null) AllShopItems.AddRange(ShopManager.Instance.bedChest.GetAllItems().ToArray());

                var shopItems = new List<ItemStack>();
                foreach (var chest in ShopManager.Instance.references.shopChests)
                {
                    shopItems.AddRange(chest.GetAllItems().ToArray());
                }

                AllShopItems.AddRange(shopItems);
            }

            AllPlayerItems.Clear();
            AllPlayerItems.AddRange(HeroMerchant.Instance.heroMerchantInventory.GetAllItems().ToArray());
        }

        private static void PerformInDungeonOrTownUpdate()
        {
            if (ShopManager.Instance) return;
            AllPlayerItems.Clear();
            AllPlayerItems.AddRange(HeroMerchant.Instance.heroMerchantInventory.GetAllItems().ToArray());
        }


        [HarmonyPostfix]
        [HarmonyPatch(typeof(ShopManager), nameof(ShopManager.GameSlot_OnSaveToDungeon))]
        [HarmonyPatch(typeof(HeroMerchantInventory), nameof(HeroMerchantInventory.Init))]
        [HarmonyPatch(typeof(HeroMerchantInventory), nameof(HeroMerchantInventory.SetItem))]
        [HarmonyPatch(typeof(ItemStack), nameof(ItemStack.SendRemovedFromHeroInventoryEvent))]
        [HarmonyPatch(typeof(Chest), nameof(Chest.CloseChest))]
        public static void Perform_InventoryUpdate()
        {
            PerformInventoryUpdate();
        }


        [HarmonyPostfix]
        [HarmonyPatch(typeof(Constants), nameof(Constants.GetInt))]
        public static void Constants_GetInt(Constants __instance, ref string key, ref int __result)
        {
            if (!key.Equals(MaxActiveQuests)) return;

            __result = Plugin.MaxQuestCount.Value;
            //Plugin.LOG.LogInfo($"{MaxActiveQuests}: {__result}");
        }

        private static int MyGetItemCount(ItemMaster searchItem)
        {
            var one = 0;
            var two = 0;

            foreach (var item in AllPlayerItems)
            {
                if (item.master is null)
                {
                    Plugin.LOG.LogWarning($"Null master found for {item.name} AllPlayerItems");
                    continue;
                }

                if (item.master.nameKey == searchItem.nameKey)
                {
                    one += item._quantity;
                }
            }

            foreach (var item in AllShopItems)
            {
                if (item.master is null)
                {
                    Plugin.LOG.LogWarning($"Null master found for {item.name} in AllShopItems");
                    continue;
                }

                if (item.master.nameKey == searchItem.nameKey)
                {
                    two += item._quantity;
                }
            }

            return one + two;
        }

        internal static List<ActiveQuest> GetQuests()
        {
            if (GameManager.Instance is null && HeroMerchant.Instance is null)
            {
                Plugin.LOG.LogError("Both GameManager.Instance and HeroMerchant.Instance are null.");
                return [];
            }

            var quests = new List<ActiveQuest>();

            // if (GameManager.Instance is not null)
            // {
            //     quests.AddRange(GameManager.Instance.currentGameSlot.willActiveQuests.ToArray());
            // }

            if (HeroMerchant.Instance is not null)
            {
                quests.AddRange(HeroMerchant.Instance.activeQuests.ToArray());
            }

            var questList = quests.Distinct().ToList();
            var removed = questList.RemoveAll(q => q.completed);

            // Plugin.LOG.LogInfo($"Removed {removed} completed quests. Total remaining: {questList.Count}");
            //
            // foreach (var quest in questList)
            // {
            //     Plugin.LOG.LogInfo($"Quest: Visitor: {quest.giverVisitorPrefabName}, Failed: {quest.failed}, Completed: {quest.completed}, Target: {quest.quest.target}, KillTarget: {quest.quest.killQuestTarget}, Qty: {quest.quest.quantity}, DaysToComplete: {quest.quest.daysToComplete}");
            // }
            return questList;
        }


        internal static IEnumerator RunUpdateQuestTrackerIE(bool rebuild = false)
        {
            if (QuestTrackerObject is null || rebuild)
            {
                if (rebuild && QuestTrackerObject is not null) Object.Destroy(QuestTrackerObject);
                yield return CreateQuestTrackerIE();
            }

            yield return UpdateQuestTrackerIE(rebuild);
            yield return UpdateQuestColoursIE();
        }

        private static IEnumerator UpdateQuestTrackerIE(bool rebuild, Action onComplete = null)
        {
            if (rebuild)
            {
                QuestTexts.Clear();
                QuestTextImages.Clear();
                QuestTargetAnimations.Clear();

                foreach (var c in QuestTrackerObject.transform)
                {
                    var child = (Transform) c;

                    if (child is not null && child.name.StartsWith(Quest) && child.name != QuestHeader)
                    {
                        Object.Destroy(child.gameObject);
                    }
                }

                Plugin.QuestTrackerNeedsRebuilding = false;
            }

            // QuestTrackerObject = GameObject.Find(QuestTrackerName);
            var quests = GetQuests();

            var questTextKeys = new List<ActiveQuest>(QuestTexts.Keys);

            foreach (var key in questTextKeys)
            {
                if (quests.Contains(key)) continue;

                var textComponent = QuestTexts[key];
                var imageComponent = QuestTextImages[textComponent];

                // Destroy associated GameObjects
                Object.Destroy(textComponent.gameObject);
                Object.Destroy(imageComponent.gameObject);

                // Remove entries from the dictionaries
                QuestTextImages.Remove(textComponent);
                QuestTexts.Remove(key);
            }

            if (OriginalFont.Value is not null)
            {
                var customFont = Font.CreateDynamicFontFromOSFont(Plugin.CustomFont.Value, Plugin.FontSize.Value) ?? OriginalFont.Value;

                HeaderText.font = Plugin.UseGameFont.Value ? OriginalFont.Value : customFont;
                HeaderText.fontSize = Plugin.FontSize.Value + 1;
                HeaderText.color = Plugin.ColouriseQuestTracker.Value ? Plugin.QuestTrackerHeaderColour.Value : Plugin.Grey;
                var headerText = $"Requests: ({GetQuests().Count}/{Constants.GetInt(MaxActiveQuests)})";
                HeaderText.text = headerText;
            }

            foreach (var quest in GetQuests())
            {
                //Plugin.LOG.LogInfo($"Quest: {quest.quest.target}");
                var currentGamePlusLevel = GameManager.Instance.GetCurrentGamePlusLevel();
                QuestTargetText.SetTerm("QUESTS/" + quest.quest.killQuestTargetKey);
                var targetEnemy = LocalizationManager.GetTranslation(QuestTargetText.Term, false);
                var qty = $"{quest.quest.quantity:#.}x";

                string questText;
                if (!string.IsNullOrWhiteSpace(quest.quest.killQuestTarget))
                {
                    string itemName;
                    int itemCount;
                    var itemByName = ItemDatabase.GetItemByName(quest.quest.killQuestTarget, currentGamePlusLevel);
                    if (itemByName is null)
                    {
                        KillQuestTargetItems.TryGetValue(quest.quest, out itemName);
                        itemCount = 0;
                    }
                    else
                    {
                        KillQuestTargetItems.TryAdd(quest.quest, itemByName.displayName);
                        itemName = itemByName.name;


                        itemCount = MyGetItemCount(itemByName);
                    }

                    questText = $"Collect {qty} {itemName} from {targetEnemy}'s ({itemCount}/{quest.quest.quantity})";
                }
                else
                {
                    string itemName;
                    int itemCount;
                    var itemByName = ItemDatabase.GetItemByName(quest.quest.target, currentGamePlusLevel);
                    if (itemByName is null)
                    {
                        QuestTargetItems.TryGetValue(quest.quest, out itemName);
                        itemCount = 0;
                    }
                    else
                    {
                        QuestTargetItems.TryAdd(quest.quest, itemByName.displayName);
                        itemName = itemByName.name;

                        itemCount = MyGetItemCount(itemByName);
                    }

                    questText = $"Collect {qty} {itemName} ({itemCount}/{quest.quest.quantity})";
                }

                var existingQuestText = QuestTexts.FirstOrDefault(q => q.Key.quest == quest.quest).Value;
                if (existingQuestText is not null)
                {
                    existingQuestText.text = questText;
                    // Plugin.LOG.LogInfo($"Existing quest text: {existingQuestText.text}");
                }
                else
                {
                    var newQuestText = CreateTextComponent(questText, new Vector3(30, Plugin.QuestLineGap.Value * (QuestTexts.Count + 1), 0), $"Quest{QuestTexts.Count + 1}", QuestTrackerObject.transform);
                    var newQuestImage = CreateImageComponent(Helpers.GetQuestIconSpriteOfChosenColour(), new Vector3(-30, Plugin.QuestLineGap.Value * (QuestTexts.Count + 1), 0), $"QuestImage{QuestTexts.Count + 1}", QuestTrackerObject.transform);
                    SetQuestSprite(quest.quest, newQuestImage, newQuestText);
                    QuestTexts.Add(quest, newQuestText);
                    QuestTextImages.Add(newQuestText, newQuestImage);
                    // Plugin.LOG.LogInfo($"New quest text: {newQuestText.text}");
                }

                UpdateQuestTargetImageVisibility();
            }

            onComplete?.Invoke();


            yield return true;
        }


        private static IEnumerator UpdateQuestColoursIE()
        {
            var currentGamePlusLevel = GameManager.Instance.GetCurrentGamePlusLevel();
            HeaderText.color = Plugin.ColouriseQuestTracker.Value ? Plugin.QuestTrackerHeaderColour.Value : Plugin.Grey;
            foreach (var quest in QuestTexts)
            {
                // Plugin.LOG.LogInfo($"Quest: Target: {quest.Key.quest.target}, KillTarget: {quest.Key.quest.killQuestTarget}, Completed: {quest.Key.completed}, Failed: {quest.Key.failed}");

                var itemName = string.IsNullOrWhiteSpace(quest.Key.quest.killQuestTarget) ? quest.Key.quest.target : quest.Key.quest.killQuestTarget;
                var itemByName = ItemDatabase.GetItemByName(itemName, currentGamePlusLevel);
                var have = MyGetItemCount(itemByName);

                var needed = quest.Key.quest.quantity;

                //npc has come back and 'completed' the quest and failed it
                if (quest.Key.completed && quest.Key.failed)
                {
                    quest.Value.color = Plugin.Grey;
                    continue;
                }

                //npc has not come back and 'completed' the quest and failed it
                if (!quest.Key.completed && quest.Key.failed)
                {
                    quest.Value.color = Plugin.Grey;
                    continue;
                }

                //quest still in progress
                if (!quest.Key.completed && !quest.Key.failed)
                {
                    var average = needed / 2.0;

                    if (have >= needed)
                        quest.Value.color = Plugin.ColouriseQuestTracker.Value ? Plugin.QuestTrackerCompletedColour.Value : Plugin.Grey;
                    else if (have >= average)
                        quest.Value.color = Plugin.ColouriseQuestTracker.Value ? Plugin.QuestTrackerHalfwayColour.Value : Plugin.Grey;
                    else
                        quest.Value.color = Plugin.ColouriseQuestTracker.Value ? Plugin.QuestTrackerFailedColour.Value : Plugin.Grey;
                }
            }

            yield break;
        }

        private static void SetQuestSprite(Quest quest, Image image, Text questText)
        {
            var currentGamePlusLevel = GameManager.Instance.GetCurrentGamePlusLevel();
            var itemByName = ItemDatabase.GetItemByName(quest.target, currentGamePlusLevel);
            if (itemByName is not null)
            {
                var sprite = ItemDatabase.GetSprite(itemByName);
                image.sprite = sprite;
                image.DOFade(1f, 0f);
                image.rectTransform.sizeDelta = new Vector2(14, 14);
            }
            else
            {
                var culture = quest.culture + 1;
                var target = quest.target;
                var enemyAnimation = EnemiesPrefabRegister.GetEnemyAnimation(culture, target);
                if (enemyAnimation is not null)
                {
                    image.rectTransform.sizeDelta = new Vector2(28, 28);
                    image.sprite = enemyAnimation.Sprites.ToArray().First();
                    var newTransform = image.transform;
                    var newTransformLocalPosition = newTransform.localPosition;
                    newTransformLocalPosition.y += -2;
                    newTransform.localPosition = newTransformLocalPosition;

                    var newTweener = enemyAnimation.PlayOnImage(image).SetLoops(-1, LoopType.Restart);
                    QuestTargetAnimations.Add(newTweener);
                    if (Plugin.AnimateQuestImages.Value)
                    {
                        newTweener.Restart();
                    }
                    else
                    {
                        newTweener.Pause();
                    }
                }
            }

            var newTransform1 = image.transform;
            var imagePosition = newTransform1.localPosition;
            var textWidth = questText.preferredWidth;
            imagePosition.x += textWidth + 17.5f; // 5 is a padding value, adjust as needed
            newTransform1.localPosition = imagePosition;
        }

        private static IEnumerator CreateQuestTrackerIE()
        {
            var parent = GameObject.Find("GUI_New/Content");
            QuestTrackerObject = new GameObject(QuestTrackerName);
            QuestTrackerObject.transform.SetParent(parent.transform, false);
            QuestTrackerObject.transform.position = new Vector3(115f, Screen.height - 300f, 0);
            var headerText = $"Requests: ({GetQuests().Count}/{Constants.GetInt(MaxActiveQuests)})";
            HeaderText = CreateTextComponent(headerText, new Vector3(30, 0, 0), QuestHeader, QuestTrackerObject.transform);
            HeaderText.alignment = TextAnchor.MiddleLeft;
            HeaderText.fontSize = Plugin.FontSize.Value + 2;
            HeaderText.color = Plugin.ColouriseQuestTracker.Value ? Plugin.QuestTrackerHeaderColour.Value : Plugin.Grey;
            QuestTargetText = QuestTrackerObject.gameObject.AddComponent<Localize>();
            QuestTrackerObject.transform.SetSiblingIndex(2);
            yield break;
        }

        private static Text CreateTextComponent(string text, Vector3 localPosition, string name, Transform parent)
        {
            var originalText = GameObject.Find("GUI_New/Content/HUD/Hero_Info/Gold/Gold_Label").GetComponent<Text>();

            var customFont = Font.CreateDynamicFontFromOSFont(Plugin.CustomFont.Value, Plugin.FontSize.Value);
            if (customFont is null)
            {
                customFont = originalText.font;
            }

            var textComponent = new GameObject(name, typeof(Text)).GetComponent<Text>();
            OriginalFont.Value = originalText.font;
            textComponent.font = Plugin.UseGameFont.Value ? OriginalFont.Value : customFont;
            textComponent.fontSize = Plugin.FontSize.Value;
            textComponent.color = new Color(1, 1, 1, 0.75f);
            textComponent.alignment = TextAnchor.MiddleLeft;
            textComponent.horizontalOverflow = HorizontalWrapMode.Overflow;
            textComponent.verticalOverflow = VerticalWrapMode.Truncate;
            textComponent.text = text;

            Transform newTransform;
            (newTransform = textComponent.transform).SetParent(parent, false);
            newTransform.localPosition = localPosition;

            return textComponent;
        }

        private static Image CreateImageComponent(Sprite sprite, Vector3 localPosition, string name, Transform parent)
        {
            var imageComponent = new GameObject(name, typeof(Image)).GetComponent<Image>();
            imageComponent.sprite = sprite;
            // Transform newTransform;
            imageComponent.transform.SetParent(parent, false);
            imageComponent.transform.localPosition = localPosition;

            return imageComponent;
        }


        public static void UpdateQuestTargetImageVisibility()
        {
            foreach (var image in QuestTextImages)
            {
                image.Value.enabled = Plugin.ShowQuestTargetImages.Value;
            }
        }

        public static void UpdateImageAnimations()
        {
            foreach (var t in QuestTargetAnimations)
            {
                if (Plugin.AnimateQuestImages.Value)
                {
                    t.Restart();
                }
                else
                {
                    t.Pause();
                }
            }
        }
    }
}