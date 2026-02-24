namespace MysticAssistantRedux.Patches;

/// <summary>
/// Harmony patches for the Mystic Shop to add the assistant shop functionality.
/// </summary>
[HarmonyPatch]
public static class MysticShopPatches
{
    /// <summary>
    /// Adds a secondary interaction to the Mystic Shop for accessing the assistant.
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Interaction_MysticShop), nameof(Interaction_MysticShop.Start))]
    public static void Start_Postfix(Interaction_MysticShop __instance)
    {
        __instance.HasSecondaryInteraction = true;
        __instance.SecondaryLabel = Localization.MysticAssistantLabel;
    }

    /// <summary>
    /// Handles the secondary interaction to open the assistant shop.
    /// Must patch base Interaction class since Interaction_MysticShop doesn't override this method.
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Interaction), nameof(Interaction.OnSecondaryInteract))]
    public static void OnSecondaryInteract_Prefix(Interaction __instance, StateMachine state)
    {
        // Only handle Mystic Shop interactions
        if (__instance is not Interaction_MysticShop mysticShop)
        {
            return;
        }

        // Handle SimpleBark interference - close any active bark first
        var boughtBark = mysticShop.boughtBark;
        if (boughtBark != null && boughtBark.IsSpeaking)
        {
            mysticShop.StartCoroutine(ClearShopKeeperBarkAndReopen(mysticShop, boughtBark, state));
            return;
        }

        OpenAssistantShop(mysticShop, state);
    }

    private static void OpenAssistantShop(Interaction_MysticShop instance, StateMachine state)
    {
        Plugin.Log.LogInfo("[MysticShop] Opening assistant shop");

        // Debug: give player God Tears for testing
        DebugAddGodTears();

        // Reset shop state for new session
        Plugin.CurrentInventoryManager = new InventoryManager(instance);
        Plugin.ResetShopState();

        // Hide HUD for cleaner shop experience
        HUD_Manager.Instance.Hide(false, 0, false);

        // Set player to inactive state
        var playerFarming = state.GetComponent<PlayerFarming>();
        PlayerFarming.SetStateForAllPlayers(StateMachine.State.InActive, false, null);
        playerFarming.GoToAndStop(playerFarming.transform.position, playerFarming.LookToObject, false, false, null, 20f, true, null, true, true, true, true, null);

        // Create the item selector (shop UI)
        var shopItemSelector = MonoSingleton<UIManager>.Instance.ShowItemSelector(
            playerFarming,
            Plugin.CurrentInventoryManager.GetShopInventory(),
            new ItemSelector.Params
            {
                Key = Plugin.ShopContextKey,
                Context = ItemSelector.Context.Buy,
                Offset = new Vector2(0f, 150f),
                ShowEmpty = true,
                RequiresDiscovery = false,
                HideQuantity = false,
                ShowCoins = false,
                AllowInputOnlyFromPlayer = playerFarming
            }
        );

        if (instance.InputOnlyFromInteractingPlayer)
        {
            MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer = playerFarming;
        }

        // Set up cost provider
        shopItemSelector.CostProvider = Plugin.GetTraderTrackerItemFromItemType;

        // Set up item purchase handler
        shopItemSelector.OnItemChosen = (Action<InventoryItem.ITEM_TYPE>)Delegate.Combine(
            shopItemSelector.OnItemChosen,
            new Action<InventoryItem.ITEM_TYPE>(chosenItemType =>
            {
                var tradeItem = Plugin.GetTraderTrackerItemFromItemType(chosenItemType);
                GivePlayerBoughtItem(instance, shopItemSelector, playerFarming, tradeItem, chosenItemType);
            }));

        // Show HUD again when shop is cancelled
        shopItemSelector.OnCancel = (Action)Delegate.Combine(
            shopItemSelector.OnCancel,
            new Action(() => HUD_Manager.Instance.Show(0, false)));

        // Handle shop closing - run post-shop screens
        shopItemSelector.OnHidden += () =>
        {
            Plugin.Log.LogInfo($"[MysticShop] Shop closed, {Plugin.PostShopActions.Count} post-shop actions queued");
            PlayerFarming.SetStateForAllPlayers(StateMachine.State.InActive, false, null);
            SetMysticShopInteractable(instance, false);
            instance.StartCoroutine(RunPostShopActionsCoroutine(instance, state));
        };
    }

    private static void GivePlayerBoughtItem(
        Interaction_MysticShop instance,
        UIItemSelectorOverlayController shopItemSelector,
        PlayerFarming playerFarming,
        TraderTrackerItems boughtItem,
        InventoryItem.ITEM_TYPE boughtItemType)
    {
        // Deduct cost (God Tear)
        Inventory.ChangeItemQuantity((int)InventoryItem.ITEM_TYPE.GOD_TEAR, -boughtItem.SellPriceActual, 0);
        DataManager.Instance.MysticRewardCount++;

        var shopStock = Plugin.CurrentInventoryManager.GetItemListCountByItemType(boughtItemType);

        Plugin.Log.LogInfo($"[MysticShop] Player purchased: {boughtItemType} (cost: {boughtItem.SellPriceActual} God Tear)");

        switch (boughtItemType)
        {
            case InventoryItem.ITEM_TYPE.BLACK_GOLD:
                Inventory.ChangeItemQuantity((int)boughtItemType, 100, 0);
                Plugin.Log.LogInfo("[MysticShop] Gave player 100 Gold");
                break;

            case InventoryItem.ITEM_TYPE.Necklace_Dark:
            case InventoryItem.ITEM_TYPE.Necklace_Light:
                Inventory.ChangeItemQuantity((int)boughtItemType, 1, 0);
                Plugin.Log.LogInfo($"[MysticShop] Gave player necklace: {boughtItemType}");
                break;

            case InventoryItem.ITEM_TYPE.CRYSTAL_DOCTRINE_STONE:
                Inventory.ChangeItemQuantity((int)boughtItemType, 1, 0);
                Plugin.CurrentInventoryManager.ChangeShopStockByQuantity(boughtItemType, -1);

                if (DataManager.Instance.CrystalDoctrinesReceivedFromMysticShop < Interaction_MysticShop.maxAmountOfCrystalDoctrines + (DataManager.Instance.MAJOR_DLC ? 4 : 0))
                {
                    DataManager.Instance.CrystalDoctrinesReceivedFromMysticShop++;
                }

                Plugin.Log.LogInfo($"[MysticShop] Gave player Crystal Doctrine Stone ({DataManager.Instance.CrystalDoctrinesReceivedFromMysticShop}/{Interaction_MysticShop.maxAmountOfCrystalDoctrines})");

                if (!Plugin.CurrentInventoryManager.BoughtCrystalDoctrineStone)
                {
                    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_CrystalDoctrine, false);
                    if (DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.CrystalDoctrine))
                    {
                        Plugin.PostShopActions.Add(ShowCrystalDoctrineTutorial);
                        Plugin.PostShopActions.Add(ShowCrystalDoctrineInMenu);
                    }
                    Plugin.CurrentInventoryManager.SetBoughtCrystalDoctrineStoneFlag(true);
                }
                break;

            case InventoryItem.ITEM_TYPE.TALISMAN:
                Inventory.KeyPieces++;
                if (DataManager.Instance.TalismanPiecesReceivedFromMysticShop < Interaction_MysticShop.maxAmountOfTalismanPieces)
                {
                    DataManager.Instance.TalismanPiecesReceivedFromMysticShop++;
                }
                Plugin.Log.LogInfo($"[MysticShop] Gave player Talisman Piece (total: {Inventory.KeyPieces})");
                if (!Plugin.CurrentInventoryManager.BoughtKeyPiece)
                {
                    Plugin.PostShopActions.Add(ShowNewTalismanPieceAnimation);
                    Plugin.CurrentInventoryManager.SetBoughtKeyPieceFlag(true);
                }
                break;

            case InventoryItem.ITEM_TYPE.FOUND_ITEM_FOLLOWERSKIN:
                var skinIndex = UnityEngine.Random.Range(0, shopStock);
                var skinToUnlock = Plugin.CurrentInventoryManager.GetFollowerSkinNameByIndex(skinIndex);
                DataManager.SetFollowerSkinUnlocked(skinToUnlock);
                RegisterSkinAlert(skinToUnlock);
                Plugin.CurrentInventoryManager.RemoveItemFromListByTypeAndIndex(boughtItemType, skinIndex);
                Plugin.CurrentInventoryManager.ChangeShopStockByQuantity(boughtItemType, -1);
                Plugin.Log.LogInfo($"[MysticShop] Unlocked follower skin: {skinToUnlock}");
                if (!Plugin.CurrentInventoryManager.BoughtFollowerSkin)
                {
                    Plugin.PostShopActions.Add(ShowUnlockedFollowerSkins);
                    Plugin.CurrentInventoryManager.SetBoughtFollowerSkinFlag(true);
                }
                break;

            case InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION_ALT:
                var decoIndex = UnityEngine.Random.Range(0, shopStock);
                var deco = Plugin.CurrentInventoryManager.GetDecorationByIndex(decoIndex);
                StructuresData.CompleteResearch(deco);
                StructuresData.SetRevealed(deco);
                Plugin.UnlockedDecorations.Add(deco);
                Plugin.CurrentInventoryManager.RemoveItemFromListByTypeAndIndex(boughtItemType, decoIndex);
                Plugin.CurrentInventoryManager.ChangeShopStockByQuantity(boughtItemType, -1);
                Plugin.Log.LogInfo($"[MysticShop] Unlocked decoration: {deco}");
                if (!Plugin.CurrentInventoryManager.BoughtDecoration)
                {
                    Plugin.PostShopActions.Add(ShowUnlockedDecorations);
                    Plugin.CurrentInventoryManager.SetBoughtDecorationFlag(true);
                }
                break;

            case InventoryItem.ITEM_TYPE.TRINKET_CARD:
                var cardIndex = UnityEngine.Random.Range(0, shopStock);
                var card = Plugin.CurrentInventoryManager.GetTarotCardByIndex(cardIndex);
                // Tarot cards are unlocked during the Show() method of UITarotCardsMenuController
                Plugin.UnlockedTarotCards.Add(card);
                Plugin.CurrentInventoryManager.RemoveItemFromListByTypeAndIndex(boughtItemType, cardIndex);
                Plugin.CurrentInventoryManager.ChangeShopStockByQuantity(boughtItemType, -1);
                Plugin.Log.LogInfo($"[MysticShop] Unlocked tarot card: {card}");
                if (!Plugin.CurrentInventoryManager.BoughtTarotCard)
                {
                    Plugin.PostShopActions.Add(ShowUnlockedTarotCards);
                    Plugin.CurrentInventoryManager.SetBoughtTarotCardFlag(true);
                }
                break;

            case InventoryItem.ITEM_TYPE.SOUL_FRAGMENT: // Actually for relics
                var relicIndex = UnityEngine.Random.Range(0, shopStock);
                var relic = Plugin.CurrentInventoryManager.GetRelicByIndex(relicIndex);
                DataManager.UnlockRelic(relic);
                Plugin.UnlockedRelics.Add(relic);
                Plugin.CurrentInventoryManager.RemoveItemFromListByTypeAndIndex(boughtItemType, relicIndex);
                Plugin.CurrentInventoryManager.ChangeShopStockByQuantity(boughtItemType, -1);
                Plugin.Log.LogInfo($"[MysticShop] Unlocked relic: {relic}");
                if (!Plugin.CurrentInventoryManager.BoughtRelic)
                {
                    Plugin.PostShopActions.Add(ShowUnlockedRelics);
                    Plugin.CurrentInventoryManager.SetBoughtRelicFlag(true);
                }
                break;

            // Apple Arcade content
            case InventoryManager.AppleSkinType:
                var appleSkinIndex = UnityEngine.Random.Range(0, shopStock);
                var appleSkinToUnlock = Plugin.CurrentInventoryManager.GetAppleSkinNameByIndex(appleSkinIndex);
                UnlockAppleSkin(appleSkinToUnlock);
                RegisterSkinAlert(appleSkinToUnlock);
                Plugin.CurrentInventoryManager.RemoveItemFromListByTypeAndIndex(boughtItemType, appleSkinIndex);
                Plugin.CurrentInventoryManager.ChangeShopStockByQuantity(boughtItemType, -1);
                Plugin.Log.LogInfo($"[MysticShop] Unlocked Apple skin: {appleSkinToUnlock}");
                if (!Plugin.CurrentInventoryManager.BoughtAppleSkin)
                {
                    Plugin.PostShopActions.Add(ShowUnlockedFollowerSkins);
                    Plugin.CurrentInventoryManager.SetBoughtAppleSkinFlag(true);
                }
                break;

            case InventoryManager.AppleDecorationType:
                var appleDecoIndex = UnityEngine.Random.Range(0, shopStock);
                var appleDeco = Plugin.CurrentInventoryManager.GetAppleDecorationByIndex(appleDecoIndex);
                StructuresData.CompleteResearch(appleDeco);
                StructuresData.SetRevealed(appleDeco);
                Plugin.UnlockedDecorations.Add(appleDeco);
                Plugin.CurrentInventoryManager.RemoveItemFromListByTypeAndIndex(boughtItemType, appleDecoIndex);
                Plugin.CurrentInventoryManager.ChangeShopStockByQuantity(boughtItemType, -1);
                Plugin.Log.LogInfo($"[MysticShop] Unlocked Apple decoration: {appleDeco}");
                if (!Plugin.CurrentInventoryManager.BoughtAppleDecoration)
                {
                    Plugin.PostShopActions.Add(ShowUnlockedDecorations);
                    Plugin.CurrentInventoryManager.SetBoughtAppleDecorationFlag(true);
                }
                break;

            case InventoryManager.AppleClothingType:
                var appleClothingIndex = UnityEngine.Random.Range(0, shopStock);
                var appleClothing = Plugin.CurrentInventoryManager.GetAppleClothingByIndex(appleClothingIndex);
                DataManager.Instance.AddNewClothes(appleClothing);
                // Add to tailor so it's available to equip (not greyed out)
                TailorManager.AddClothingToTailor(appleClothing, $"Clothes/{appleClothing}");
                Plugin.UnlockedClothing.Add(appleClothing);
                Plugin.CurrentInventoryManager.RemoveItemFromListByTypeAndIndex(boughtItemType, appleClothingIndex);
                Plugin.CurrentInventoryManager.ChangeShopStockByQuantity(boughtItemType, -1);
                Plugin.Log.LogInfo($"[MysticShop] Unlocked Apple clothing: {appleClothing}");
                if (!Plugin.CurrentInventoryManager.BoughtAppleClothing)
                {
                    Plugin.PostShopActions.Add(ShowUnlockedClothing);
                    Plugin.CurrentInventoryManager.SetBoughtAppleClothingFlag(true);
                }
                break;

            // case InventoryManager.PalworldSkinType:
            //     var palworldSkinIndex = UnityEngine.Random.Range(0, shopStock);
            //     var palworldSkinToUnlock = Plugin.CurrentInventoryManager.GetPalworldSkinNameByIndex(palworldSkinIndex);
            //     DataManager.SetFollowerSkinUnlocked(palworldSkinToUnlock);
            //     RegisterSkinAlert(palworldSkinToUnlock);
            //     Plugin.CurrentInventoryManager.RemoveItemFromListByTypeAndIndex(boughtItemType, palworldSkinIndex);
            //     Plugin.CurrentInventoryManager.ChangeShopStockByQuantity(boughtItemType, -1);
            //     Plugin.Log.LogInfo($"[MysticShop] Unlocked Palworld skin: {palworldSkinToUnlock}");
            //     if (!Plugin.CurrentInventoryManager.BoughtPalworldSkin)
            //     {
            //         Plugin.PostShopActions.Add(ShowUnlockedFollowerSkins);
            //         Plugin.CurrentInventoryManager.SetBoughtPalworldSkinFlag(true);
            //     }
            //     break;

            case InventoryManager.BossSkinType:
                var bossSkinIndex = UnityEngine.Random.Range(0, shopStock);
                var bossSkinToUnlock = Plugin.CurrentInventoryManager.GetBossSkinNameByIndex(bossSkinIndex);
                DataManager.SetFollowerSkinUnlocked(bossSkinToUnlock);
                RegisterSkinAlert(bossSkinToUnlock);
                Plugin.CurrentInventoryManager.RemoveItemFromListByTypeAndIndex(boughtItemType, bossSkinIndex);
                Plugin.CurrentInventoryManager.ChangeShopStockByQuantity(boughtItemType, -1);
                Plugin.Log.LogInfo($"[MysticShop] Unlocked Boss skin: {bossSkinToUnlock}");
                if (!Plugin.CurrentInventoryManager.BoughtBossSkin)
                {
                    Plugin.PostShopActions.Add(ShowUnlockedFollowerSkins);
                    Plugin.CurrentInventoryManager.SetBoughtBossSkinFlag(true);
                }
                break;

            case InventoryManager.QuestSkinType:
                var questSkinIndex = UnityEngine.Random.Range(0, shopStock);
                var questSkinToUnlock = Plugin.CurrentInventoryManager.GetQuestSkinNameByIndex(questSkinIndex);
                DataManager.SetFollowerSkinUnlocked(questSkinToUnlock);
                RegisterSkinAlert(questSkinToUnlock);
                Plugin.CurrentInventoryManager.RemoveItemFromListByTypeAndIndex(boughtItemType, questSkinIndex);
                Plugin.CurrentInventoryManager.ChangeShopStockByQuantity(boughtItemType, -1);
                Plugin.Log.LogInfo($"[MysticShop] Unlocked Quest skin: {questSkinToUnlock}");
                if (!Plugin.CurrentInventoryManager.BoughtQuestSkin)
                {
                    Plugin.PostShopActions.Add(ShowUnlockedFollowerSkins);
                    Plugin.CurrentInventoryManager.SetBoughtQuestSkinFlag(true);
                }
                break;

            case InventoryManager.AppleFleeceType:
                var appleFleeceIndex = UnityEngine.Random.Range(0, shopStock);
                var appleFleece = Plugin.CurrentInventoryManager.GetAppleFleeceByIndex(appleFleeceIndex);
                if (!DataManager.Instance.UnlockedFleeces.Contains(appleFleece))
                {
                    DataManager.Instance.UnlockedFleeces.Add(appleFleece);
                }
                Plugin.UnlockedFleeces.Add(appleFleece);
                Plugin.CurrentInventoryManager.RemoveItemFromListByTypeAndIndex(boughtItemType, appleFleeceIndex);
                Plugin.CurrentInventoryManager.ChangeShopStockByQuantity(boughtItemType, -1);
                Plugin.Log.LogInfo($"[MysticShop] Unlocked Apple fleece: {appleFleece}");
                if (!Plugin.CurrentInventoryManager.BoughtAppleFleece)
                {
                    Plugin.PostShopActions.Add(ShowUnlockedFleeces);
                    Plugin.CurrentInventoryManager.SetBoughtAppleFleeceFlag(true);
                }
                break;

            default:
                Inventory.ChangeItemQuantity((int)boughtItemType, 1, 0);
                Plugin.Log.LogInfo($"[MysticShop] Gave player item: {boughtItemType}");
                break;
        }

        // Play purchase feedback
        UIManager.PlayAudio("event:/followers/pop_in");
        ResourceCustomTarget.Create(instance.gameObject, playerFarming.transform.position, InventoryItem.ITEM_TYPE.GOD_TEAR, delegate { }, true);
    }

    /// <summary>
    /// Custom context text for the assistant shop.
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(UIItemSelectorOverlayController), nameof(UIItemSelectorOverlayController.RefreshContextText))]
    public static bool RefreshContextText_Prefix(UIItemSelectorOverlayController __instance)
    {
        if (__instance._params.Key != Plugin.ShopContextKey)
        {
            return true; // Run original method
        }

        if (__instance._params.Context != ItemSelector.Context.Buy)
        {
            return false;
        }

        var traderTrackerItems = __instance.CostProvider?.Invoke(__instance._category.MostRecentItem);
        if (traderTrackerItems == null)
        {
            return false;
        }

        if (Plugin.WarningMessage != null)
        {
            __instance._buttonPromptText.text = $" <color=red>{Plugin.WarningMessage}</color>";
        }
        else
        {
            if (__instance._category == null)
            {
                __instance._buttonPromptText.text = string.Format(__instance._contextString, "something broke, please leave the shop and try again",
                    CostFormatter.FormatCost(InventoryItem.ITEM_TYPE.GOD_TEAR, traderTrackerItems.SellPriceActual, true, false)) + __instance._addtionalText;
            }
            else
            {
                __instance._buttonPromptText.text = string.Format(__instance._contextString,
                    InventoryInfo.GetShopLabelByItemType(__instance._category.MostRecentItem),
                    CostFormatter.FormatCost(InventoryItem.ITEM_TYPE.GOD_TEAR, traderTrackerItems.SellPriceActual, true, false)) + __instance._addtionalText;
            }
        }

        return false;
    }

    /// <summary>
    /// Initializes button prompt text when the item selector opens.
    /// Without this, _buttonPromptText retains stale text from the previous selector usage
    /// because OnShowStarted calls OverrideDefault() which does NOT trigger OnItemSelected/RefreshContextText.
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(UIItemSelectorOverlayController), nameof(UIItemSelectorOverlayController.OnShowStarted))]
    public static void ItemSelector_OnShowStarted_Postfix(UIItemSelectorOverlayController __instance)
    {
        if (__instance._inventoryItems.Count > 0)
        {
            __instance.RefreshContextText();
        }
    }

    /// <summary>
    /// Redirects icon lookups for custom shop types to appropriate existing types.
    /// This fixes missing/wrong icons for Apple Arcade content categories.
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(InventoryIconMapping), nameof(InventoryIconMapping.GetImage),
        [typeof(InventoryItem.ITEM_TYPE), typeof(Image)])]
    public static void GetImage_Prefix(ref InventoryItem.ITEM_TYPE type)
    {
        // Redirect custom types to types with appropriate icons
        type = type switch
        {
            // Apple content - use icons from matching Mystic types
            InventoryManager.AppleSkinType => InventoryItem.ITEM_TYPE.FOUND_ITEM_FOLLOWERSKIN,
            InventoryManager.AppleDecorationType => InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION_ALT,
            InventoryManager.AppleClothingType => InventoryItem.ITEM_TYPE.FOUND_ITEM_FOLLOWERSKIN, // Use skin icon as fallback

            // Palworld/Boss - use follower skin icon
            // InventoryManager.PalworldSkinType => InventoryItem.ITEM_TYPE.FOUND_ITEM_FOLLOWERSKIN,
            InventoryManager.BossSkinType => InventoryItem.ITEM_TYPE.FOUND_ITEM_FOLLOWERSKIN,
            _ => type
        };
    }

    /// <summary>
    /// Sets custom icon for Apple fleece in the shop.
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(InventoryIconMapping), nameof(InventoryIconMapping.GetImage),
        [typeof(InventoryItem.ITEM_TYPE), typeof(Image)])]
    public static void GetImage_Postfix(InventoryItem.ITEM_TYPE type, Image image)
    {
        if (type != InventoryManager.AppleFleeceType)
        {
            return;
        }

        var customSprite = LoadFleece680Sprite();
        if (customSprite != null)
        {
            image.sprite = customSprite;
        }
    }

    /// <summary>
    /// Resets overbuy warning when selecting a different item.
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(UIItemSelectorOverlayController), nameof(UIItemSelectorOverlayController.OnItemSelected))]
    public static bool OnItemSelected_Prefix()
    {
        Plugin.WarningMessage = null;
        return true; // Continue to original method
    }

    /// <summary>
    /// Handles item click with overbuy warning system.
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(UIItemSelectorOverlayController), nameof(UIItemSelectorOverlayController.OnItemClicked))]
    public static bool OnItemClicked_Prefix(UIItemSelectorOverlayController __instance, GenericInventoryItem item)
    {
        if (__instance._params.Key != Plugin.ShopContextKey)
        {
            return true; // Run original method
        }

        var quantity = __instance.GetItemQuantity(item.Type);

        if (quantity > 0 && __instance._params.Context == ItemSelector.Context.Buy)
        {
            var traderTrackerItems = __instance.CostProvider?.Invoke(item.Type);
            if (traderTrackerItems != null && Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.GOD_TEAR) >= traderTrackerItems.SellPriceActual)
            {
                // Check for purchase warning
                var warningMsg = Plugin.WarningMessage == null
                    ? InventoryInfo.CheckForPurchaseWarning(traderTrackerItems)
                    : null;

                if (warningMsg != null)
                {
                    Plugin.WarningMessage = warningMsg;
                    Plugin.Log.LogInfo($"[MysticShop] Warning triggered for {item.Type}");
                    item.Shake();
                    // 10% chance for funny bleat
                    if (UnityEngine.Random.Range(0, 10) == 5)
                    {
                        UIManager.PlayAudio("event:/player/yeehaa");
                    }
                    else
                    {
                        UIManager.PlayAudio("event:/player/speak_to_follower_noBookPage");
                    }
                }
                else
                {
                    // Proceed with purchase
                    __instance.OnItemChosen?.Invoke(item.Type);
                    if (__instance._params.HideOnSelection)
                    {
                        __instance.Hide();
                    }
                    else
                    {
                        __instance.UpdateQuantities();
                    }
                    Plugin.WarningMessage = null;
                }

                __instance.RefreshContextText();
                return false;
            }
        }

        // Can't afford or no stock
        item.Shake();
        UIManager.PlayAudio("event:/ui/negative_feedback");
        return false;
    }

    #region Save Slot Cleanup

    /// <summary>
    /// Clears the locked God Tear cost when a save slot is deleted.
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(SaveAndLoad), nameof(SaveAndLoad.DeleteSaveSlot))]
    public static void DeleteSaveSlot_Postfix(int saveSlot)
    {
        Plugin.ClearCostForSlot(saveSlot);
    }

    /// <summary>
    /// Clears the locked God Tear cost when a save slot is reset (new game).
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(SaveAndLoad), nameof(SaveAndLoad.ResetSave))]
    public static void ResetSave_Postfix(int saveSlot)
    {
        Plugin.ClearCostForSlot(saveSlot);
    }

    #endregion

    #region Coroutines

    private static IEnumerator ClearShopKeeperBarkAndReopen(Interaction_MysticShop instance, SimpleBark boughtBark, StateMachine state)
    {
        boughtBark.gameObject.SetActive(false);
        boughtBark.Close();
        yield return new WaitForSecondsRealtime(0.5f);
        instance.OnSecondaryInteract(state);
    }

    private static IEnumerator RunPostShopActionsCoroutine(Interaction_MysticShop instance, StateMachine state)
    {
        SetMysticShopInteractable(instance, false);

        // Wait for any menus to close
        while (UIMenuBase.ActiveMenus.Count > 0)
        {
            yield return null;
        }

        // Run each post-shop action
        foreach (var action in Plugin.PostShopActions)
        {
            action.Invoke();
            while (UIMenuBase.ActiveMenus.Count > 0)
            {
                yield return null;
            }
            yield return new WaitForSecondsRealtime(0.25f);
        }

        yield return new WaitForSecondsRealtime(0.5f);
        SetMysticShopInteractable(instance, true);
        Plugin.Log.LogInfo("[MysticShop] Post-shop actions complete, returning control to player");

        // Return control to player
        foreach (var playerFarming in PlayerFarming.players)
        {
            if (playerFarming.GoToAndStopping)
            {
                playerFarming.AbortGoTo(true);
            }
        }

        PlayerFarming.SetStateForAllPlayers(
            (LetterBox.IsPlaying || MMConversation.isPlaying) ? StateMachine.State.InActive : StateMachine.State.Idle,
            false, null);
        state.CURRENT_STATE = StateMachine.State.Idle;
    }

    #endregion

    #region Post-Shop Screen Actions

    private static void ShowCrystalDoctrineTutorial()
    {
        var menu = MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.CrystalDoctrine, 0f);
        menu.Show();
    }

    private static void ShowCrystalDoctrineInMenu()
    {
        var controller = MonoSingleton<UIManager>.Instance.PlayerUpgradesMenuTemplate.Instantiate();
        controller.ShowCrystalUnlock();
    }

    private static void ShowNewTalismanPieceAnimation()
    {
        var controller = MonoSingleton<UIManager>.Instance.KeyScreenTemplate.Instantiate();
        controller.Show();
    }

    private static void ShowUnlockedFollowerSkins()
    {
        var controller = MonoSingleton<UIManager>.Instance.FollowerFormsMenuTemplate.Instantiate();
        controller.Show();
    }

    private static void ShowUnlockedDecorations()
    {
        var controller = MonoSingleton<UIManager>.Instance.BuildMenuTemplate.Instantiate();
        controller.Show(Plugin.UnlockedDecorations);
    }

    private static void ShowUnlockedTarotCards()
    {
        var controller = MonoSingleton<UIManager>.Instance.TarotCardsMenuTemplate.Instantiate();
        controller.Show(Plugin.UnlockedTarotCards.ToArray());
    }

    private static void ShowUnlockedRelics()
    {
        var controller = MonoSingleton<UIManager>.Instance.RelicMenuTemplate.Instantiate();
        controller.Show(Plugin.UnlockedRelics);
    }

    private static void ShowUnlockedClothing()
    {
        // Show unlock notification for each clothing item
        foreach (var clothing in Plugin.UnlockedClothing)
        {
            NotificationCentre.Instance.PlayGenericNotification(
                "Notifications/UnlockedNewOutfit/Notification/On",
                NotificationBase.Flair.Positive);
        }
    }

    private static void ShowUnlockedFleeces()
    {
        foreach (var fleeceId in Plugin.UnlockedFleeces)
        {
            var fleeceName = LocalizationManager.GetTranslation($"TarotCards/Fleece{fleeceId}/Name");
            NotificationCentre.Instance.PlayGenericNotification(
                $"{fleeceName} unlocked!",
                NotificationBase.Flair.Positive);
        }
    }

    private static void SetMysticShopInteractable(Interaction_MysticShop instance, bool active)
    {
        instance.Interactable = active;
    }

    /// <summary>
    /// Registers a skin in the alert system so the "new" badge appears in the follower forms menu.
    /// The UI badge (IndoctrinationFormItem) checks alerts using Skin[0].Skin (the Spine skeleton key),
    /// not the Title field. We must look up the WorshipperData entry and use the correct key.
    /// For boss/invariant skins, the vanilla OnSkinUnlocked handler silently skips alert registration,
    /// so we call AddOnce directly to bypass that check. We also clear _singleAlerts first because
    /// AddOnce/Add both reject alerts already in _singleAlerts (the "shown once" persistence list),
    /// which would block the badge if the skin was ever registered in a previous session.
    /// </summary>
    private static void RegisterSkinAlert(string skinName)
    {
        var skinData = WorshipperData.Instance.GetCharacters(skinName);
        var alertKey = skinData?.Skin[0].Skin ?? skinName;
        var alerts = DataManager.Instance.Alerts.CharacterSkinAlerts;

        // Clear _singleAlerts so AddOnce can succeed even if this skin was registered before
        alerts._singleAlerts.Remove(alertKey);
        alerts.AddOnce(alertKey);
    }

    [System.Diagnostics.Conditional("DEBUG")]
    private static void DebugAddGodTears()
    {
        const int amount = 999;
        Inventory.ChangeItemQuantity((int)InventoryItem.ITEM_TYPE.GOD_TEAR, amount, 0);
        Plugin.Log.LogInfo($"[Debug] Added {amount} God Tears (total: {Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.GOD_TEAR)})");
    }

    #endregion

    #region Fleece Menu Patches

    /// <summary>
    /// Tracks whether we've already added the Apple fleece button to avoid duplicates.
    /// </summary>
    private static bool _appleFleeceButtonAdded;

    /// <summary>
    /// Adds Apple fleece 680 to the Player Upgrades Menu (Altar) if the player has unlocked it.
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(UIPlayerUpgradesMenuController), nameof(UIPlayerUpgradesMenuController.Start))]
    public static void UIPlayerUpgradesMenuController_Start_Postfix(UIPlayerUpgradesMenuController __instance)
    {


        if (!DataManager.Instance.UnlockedFleeces.Contains(ExclusiveContent.AppleFleece))
        {
            return;
        }

        if (_appleFleeceButtonAdded)
        {
            return;
        }

        // Check if any fleece items exist to clone
        if (__instance._fleeceItems == null || __instance._fleeceItems.Length == 0)
        {
            return;
        }

        // Find a template to clone (use first DLC fleece or any fleece)
        FleeceItemBuyable template = null;
        foreach (var item in __instance._fleeceItems)
        {
            if (item != null && item.gameObject != null)
            {
                template = item;
                break;
            }
        }

        if (template == null)
        {
            return;
        }

        // Clone the template - place in regular fleece container (not DLC, since it's Apple Arcade content)
        var newFleeceObj = UnityEngine.Object.Instantiate(template.gameObject, __instance._fleeceContainer.transform);
        newFleeceObj.name = "FleeceItemBuyable_Apple680";

        var newFleece = newFleeceObj.GetComponent<FleeceItemBuyable>();
        if (newFleece == null)
        {
            return;
        }

        // Configure for fleece 680
        newFleece._forcedFleeceIndex = ExclusiveContent.AppleFleece;
        newFleece.Configure(ExclusiveContent.AppleFleece);
        newFleece.OnFleeceChosen = __instance.FleeceItemSelected;

        // Expand the array to include the new fleece
        var newArray = new FleeceItemBuyable[__instance._fleeceItems.Length + 1];
        __instance._fleeceItems.CopyTo(newArray, 0);
        newArray[newArray.Length - 1] = newFleece;
        __instance._fleeceItems = newArray;

        _appleFleeceButtonAdded = true;

        // Recalculate fleece counter — Start() computed it before we added fleece 680
        int unlocked = 0;
        int total = 0;
        foreach (var fi in __instance._fleeceItems)
        {
            if (fi.gameObject.activeSelf)
            {
                ++total;
                if (fi.Unlocked)
                {
                    ++unlocked;
                }
            }
        }
        __instance._fleeceCount.text = LocalizeIntegration.FormatCurrentMax(unlocked.ToString(), total.ToString()) ?? "";

        Plugin.Log.LogInfo($"[FleeceMenu] Added Apple fleece 680 to Player Upgrades Menu (counter: {unlocked}/{total})");
    }

    /// <summary>
    /// Resets the Apple fleece button tracking when menu is hidden.
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(UIPlayerUpgradesMenuController), nameof(UIPlayerUpgradesMenuController.OnHideCompleted))]
    public static void UIPlayerUpgradesMenuController_OnHideCompleted_Postfix()
    {
        _appleFleeceButtonAdded = false;
    }

    /// <summary>
    /// Adds Apple fleece 680 button to the standalone fleece selection menu if the player has unlocked it.
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(FleeceSelectionMenu), nameof(FleeceSelectionMenu.OnShowStarted))]
    public static void FleeceSelectionMenu_OnShowStarted_Postfix(FleeceSelectionMenu __instance)
    {


        if (!DataManager.Instance.UnlockedFleeces.Contains(ExclusiveContent.AppleFleece))
        {
            return;
        }

        // Check if already has a button for 680
        foreach (var btn in __instance._fleeceButtons)
        {
            var icon = btn.GetComponent<FleeceMenuIcon>();
            if (icon != null && icon.FleeceNumber == ExclusiveContent.AppleFleece)
            {
                return; // Already added
            }
        }

        // Clone an existing button to use as template
        if (__instance._fleeceButtons.Count == 0)
        {
            return;
        }

        var templateButton = __instance._fleeceButtons[0];
        var newButtonObj = UnityEngine.Object.Instantiate(templateButton.gameObject, templateButton.transform.parent);
        newButtonObj.name = "FleeceButton_Apple680";

        var newButton = newButtonObj.GetComponent<Button>();
        var fleeceIcon = newButtonObj.GetComponent<FleeceMenuIcon>();

        // Set to Apple fleece number
        fleeceIcon.FleeceNumber = ExclusiveContent.AppleFleece;
        fleeceIcon.Init();

        // Add click handler
        newButton.onClick.RemoveAllListeners();
        newButton.onClick.AddListener(() => __instance.SetFleece(newButton, fleeceIcon));

        // Add to the button list
        __instance._fleeceButtons.Add(newButton);

        // If currently equipped, make it the default selection
        if (DataManager.Instance.PlayerFleece == ExclusiveContent.AppleFleece)
        {
            __instance.OverrideDefault(newButton);
        }

        Plugin.Log.LogInfo("[FleeceMenu] Added Apple fleece 680 button to standalone menu");
    }

    #endregion

    #region Apple Follower Skin Support

    private static bool _appleSkinsValidated;
    private static readonly HashSet<string> ValidatedAppleSkins = [];

    /// <summary>
    /// Validates which Apple follower skins exist in WorshipperData at runtime.
    /// The Apple skins use real animal names (Orangutan, Beaver, Lobster) — not Apple_1/2/etc.
    /// </summary>
    internal static void ValidateAppleSkins()
    {
        if (_appleSkinsValidated)
        {
            return;
        }

        _appleSkinsValidated = true;

        // Migrate legacy "Apple_" from old code that used StripNumbers("Apple_1")
        if (DataManager.Instance.FollowerSkinsUnlocked.Contains("Apple_"))
        {
            DataManager.Instance.FollowerSkinsUnlocked.Remove("Apple_");
            foreach (var (skinName, hasPcAssets) in ExclusiveContent.AppleSkins)
            {
                if (hasPcAssets)
                {
                    DataManager.SetFollowerSkinUnlocked(skinName);
                }
            }
            Plugin.Log.LogInfo("[AppleSkins] Migrated legacy 'Apple_' unlock to real skin names");
        }

        try
        {
            var characters = WorshipperData.Instance.Characters;
            foreach (var (skinName, hasPcAssets) in ExclusiveContent.AppleSkins)
            {
                if (!hasPcAssets)
                {
                    Plugin.Log.LogInfo($"[AppleSkins] '{skinName}' has no PC assets, skipping validation");
                    continue;
                }

                var found = false;
                for (var i = 0; i < characters.Count; i++)
                {
                    if (characters[i].Title != skinName)
                    {
                        continue;
                    }

                    ValidatedAppleSkins.Add(skinName);
                    found = true;
                    var c = characters[i];
                    Plugin.Log.LogInfo($"[AppleSkins] Validated '{skinName}' at index {i}: Skin[0]='{c.Skin[0].Skin}', Variants={c.Skin.Count}, DropLocation={c.DropLocation}, Hidden={c.Hidden}");
                    break;
                }

                if (!found)
                {
                    Plugin.Log.LogWarning($"[AppleSkins] '{skinName}' not found in WorshipperData despite having PC assets");
                }
            }

            Plugin.Log.LogInfo($"[AppleSkins] Validation complete: {ValidatedAppleSkins.Count} skins available");
        }
        catch (Exception ex)
        {
            Plugin.Log.LogWarning($"[AppleSkins] Failed to validate Apple skin data: {ex.Message}");
        }
    }

    /// <summary>
    /// Whether a specific Apple skin has been validated as existing in WorshipperData.
    /// </summary>
    internal static bool IsAppleSkinAvailable(string skinName) => ValidatedAppleSkins.Contains(skinName);

    /// <summary>
    /// Unlocks an Apple follower skin using its real animal name.
    /// Real names (Orangutan, Beaver, Lobster) have no digits, so SetFollowerSkinUnlocked works correctly.
    /// </summary>
    private static void UnlockAppleSkin(string skinName)
    {
        ValidateAppleSkins();
        DataManager.SetFollowerSkinUnlocked(skinName);
        Plugin.Log.LogInfo($"[AppleSkins] Unlocked '{skinName}'");
    }

    #endregion

    #region Fleece 680 Icon

    /// <summary>
    /// Cached sprite for fleece 680.
    /// </summary>
    private static Sprite _fleece680Sprite;

    /// <summary>
    /// Loads the fleece 680 icon, preferring the game's native Outfit_Apple sprite
    /// and falling back to an embedded resource if not found.
    /// </summary>
    private static Sprite LoadFleece680Sprite()
    {
        if (_fleece680Sprite != null)
        {
            return _fleece680Sprite;
        }

        // Try loading from game assets first (Outfit_Apple sprite exists in the game's asset bundles)
        try
        {
            var outfitApple = Resources.FindObjectsOfTypeAll<Sprite>().FirstOrDefault(s => s.name == "Outfit_Apple");
            if (outfitApple != null)
            {
                _fleece680Sprite = outfitApple;
                Plugin.Log.LogInfo("[Fleece680Icon] Using game's Outfit_Apple sprite");
                return _fleece680Sprite;
            }

            Plugin.Log.LogWarning("[Fleece680Icon] Outfit_Apple sprite not found in game assets, falling back to embedded resource");
        }
        catch (Exception ex)
        {
            Plugin.Log.LogWarning($"[Fleece680Icon] Failed to search game assets: {ex.Message}");
        }

        // Fallback: load from embedded resource
        try
        {
            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream("MysticAssistantRedux.assets.Fleece_680.png");
            if (stream == null)
            {
                Plugin.Log.LogWarning("[Fleece680Icon] Could not find embedded resource");
                return null;
            }

            using var ms = new MemoryStream();
            stream.CopyTo(ms);
            var data = ms.ToArray();

            var tex = new Texture2D(2, 2, TextureFormat.RGBA32, false) { filterMode = FilterMode.Point };
            tex.LoadImage(data);

            _fleece680Sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            Plugin.Log.LogInfo($"[Fleece680Icon] Loaded from embedded resource ({tex.width}x{tex.height})");
            return _fleece680Sprite;
        }
        catch (Exception ex)
        {
            Plugin.Log.LogError($"[Fleece680Icon] Failed to load embedded resource: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Provides the custom icon for Apple fleece 680.
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(FleeceIconMapping), nameof(FleeceIconMapping.GetImage), [typeof(int)])]
    public static void FleeceIconMapping_GetImage_Postfix(int index, ref Sprite __result)
    {
        if (__result != null || index != ExclusiveContent.AppleFleece)
        {
            return;
        }

        __result = LoadFleece680Sprite();
    }

    #endregion

    #region Apple Clothing Injection

    /// <summary>
    /// Reference to the last ClothingData array we injected Apple clothing into.
    /// Using reference equality instead of a boolean flag so we detect when
    /// TailorManager.clothingData is replaced with a fresh array (e.g., after
    /// scene reload) and need to re-inject.
    /// </summary>
    private static ClothingData[] _lastInjectedClothingArray;

    /// <summary>
    /// Injects ClothingData for Apple Arcade clothing types into TailorManager.
    /// This makes them appear in the clothing selection menu.
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(TailorManager), nameof(TailorManager.ClothingData), MethodType.Getter)]
    public static void TailorManager_ClothingData_Postfix(ref ClothingData[] __result)
    {
        if (__result == null || __result == _lastInjectedClothingArray)
        {
            return;
        }

        // Check if Apple clothing already exists (e.g., from a previous injection still in cache)
        if (__result.Any(cd => cd.ClothingType == FollowerClothingType.Apple_1))
        {
            _lastInjectedClothingArray = __result;
            return;
        }

        // Create ClothingData for each Apple clothing type
        var appleClothingList = new List<ClothingData>(__result);

        foreach (var clothingType in ExclusiveContent.AppleClothing)
        {
            var clothingData = ScriptableObject.CreateInstance<ClothingData>();
            clothingData.ClothingType = clothingType;
            clothingData.ProtectionType = FollowerProtectionType.None;
            clothingData.ForSale = false;
            clothingData.SpecialClothing = false;
            clothingData.IsSecret = false;
            clothingData.IsDLC = false;
            clothingData.IsMajorDLC = false;
            clothingData.HideOnTailorMenu = false;
            clothingData.CanBeCrafted = true;
            clothingData.Variants = [$"Clothes/{clothingType}"];
            // Must have at least one entry or ConfigureFollowerOutfit crashes with index out of range
            clothingData.SlotAndColours = [new WorshipperData.SlotsAndColours { SlotAndColours = [] }];

            appleClothingList.Add(clothingData);

            // Log the localized name
            var localizedName = LocalizationManager.GetTranslation($"Clothing/{clothingType}/Name");
            Plugin.Log.LogInfo($"[AppleClothing] Injected ClothingData for {clothingType}, Name: '{localizedName}'");
        }

        __result = appleClothingList.ToArray();
        TailorManager.clothingData = __result;
        _lastInjectedClothingArray = __result;
    }

    /// <summary>
    /// Provides crafting costs for Apple clothing, which are missing from the vanilla
    /// ClothingData.Cost switch statement (Apple_1/Apple_2 fall through to default → empty array).
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(ClothingData), nameof(ClothingData.Cost), MethodType.Getter)]
    public static void ClothingData_Cost_Postfix(ClothingData __instance, ref ClothingData.CostItem[] __result)
    {
        switch (__instance.ClothingType)
        {
            case FollowerClothingType.Apple_1:
                __result =
                [
                    new ClothingData.CostItem(InventoryItem.ITEM_TYPE.SILK_THREAD, 4),
                    new ClothingData.CostItem(InventoryItem.ITEM_TYPE.COTTON, 4)
                ];
                break;
            case FollowerClothingType.Apple_2:
                __result =
                [
                    new ClothingData.CostItem(InventoryItem.ITEM_TYPE.SILK_THREAD, 4),
                    new ClothingData.CostItem(InventoryItem.ITEM_TYPE.COTTON, 6)
                ];
                break;
        }
    }

    #endregion

    #region Apple Decoration Injection

    /// <summary>
    /// Injects Apple decorations into the DLC decoration list so they appear in the Build Menu.
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(DataManager), nameof(DataManager.DecorationsForType))]
    public static void DataManager_DecorationsForType_Postfix(DataManager.DecorationType decorationType, ref List<StructureBrain.TYPES> __result)
    {
        // Add Apple decorations to the DLC category
        if (decorationType == DataManager.DecorationType.DLC)
        {
            var added = 0;
            foreach (var deco in ExclusiveContent.AppleDecorations)
            {
                if (!__result.Contains(deco))
                {
                    __result.Add(deco);
                    added++;
                }
            }
            if (added > 0)
            {
                Plugin.Log.LogInfo($"[AppleDecorations] Injected {added} Apple decorations into DLC list (total: {__result.Count})");
            }
        }
    }

    #endregion

    #region Apple Decoration Placement Fix

    private static readonly HashSet<StructureBrain.TYPES> _appleDecoTypes = new(ExclusiveContent.AppleDecorations);
    private static readonly Dictionary<StructureBrain.TYPES, GameObject> _ghostCache = new();

    /// <summary>
    /// Apple decoration entries exist in the TypeAndPlacementObjects scene data, but their
    /// Addr_PlacementObject GUIDs are only valid on Apple Arcade builds. On PC, accessing
    /// the PlacementObject property throws InvalidKeyException because the GUID doesn't
    /// resolve in the addressable catalog. This causes a crash at:
    ///   Interaction_PlacementRegion.PlaceBuilding() line 224:
    ///     placementRegion.PlacementGameObject = TypeAndPlacementObjects.GetByType(type).PlacementObject;
    ///
    /// This prefix creates a runtime PlacementObject ghost that loads the real decoration
    /// prefab via ToBuildAsset, giving a proper visual preview during placement.
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Interaction_PlacementRegion), nameof(Interaction_PlacementRegion.PlaceBuilding))]
    public static void PlaceBuilding_Prefix(StructureBrain.TYPES structureType) => EnsureAppleGhost(structureType);

    /// <summary>
    /// Ensures the ghost PlacementObject exists when MOVING an already-placed Apple decoration.
    /// PlayMove() accesses TypeAndPlacementObjects.GetByType(type).PlacementObject, which
    /// throws InvalidKeyException for Apple GUIDs on PC.
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(PlacementRegion), nameof(PlacementRegion.PlayMove))]
    public static void PlayMove_Prefix(Structure structure) => EnsureAppleGhost(structure.Brain.Data.Type);

    /// <summary>
    /// Ensures a ghost PlacementObject is injected into the TypeAndPlacementObjects entry
    /// for an Apple decoration type. Without this, accessing the PlacementObject property
    /// triggers an Addressable load for a GUID that only exists on Apple Arcade builds,
    /// crashing the game on PC with InvalidKeyException.
    /// </summary>
    private static void EnsureAppleGhost(StructureBrain.TYPES structureType)
    {
        if (!_appleDecoTypes.Contains(structureType))
        {
            return;
        }

        var entry = TypeAndPlacementObjects.GetByType(structureType);
        if (entry == null || entry._placementObject != null)
        {
            return;
        }

        if (_ghostCache.TryGetValue(structureType, out var cached))
        {
            entry._placementObject = cached;
            Plugin.Log.LogInfo($"[AppleDecoFix] Using cached ghost for {structureType}");
            return;
        }

        var ghost = CreateAppleDecorationGhost(structureType);
        if (ghost != null)
        {
            _ghostCache[structureType] = ghost;
            entry._placementObject = ghost;
            Plugin.Log.LogInfo($"[AppleDecoFix] Created ghost for {structureType}");
        }
        else
        {
            Plugin.Log.LogWarning($"[AppleDecoFix] Failed to create ghost for {structureType}");
        }
    }

    /// <summary>
    /// Creates a runtime PlacementObject ghost for an Apple decoration.
    /// The template must be active so that clones created by Object.Instantiate are also
    /// active (inactive clones freeze the game since Time.timeScale=0 during placement).
    /// ToBuildAsset is left empty because PlacementObject.Start() modifies it in-place
    /// (prepends "Assets/", appends ".prefab") — if set on the template, clones inherit
    /// the already-modified path and Start() double-prefixes it.
    /// </summary>
    private static GameObject CreateAppleDecorationGhost(StructureBrain.TYPES type)
    {
        if (!ExclusiveContent.AppleDecorationPrefabPaths.ContainsKey(type))
        {
            return null;
        }

        var ghostObj = new GameObject($"AppleGhost_{type}");

        var placement = ghostObj.AddComponent<PlacementObject>();
        placement.Bounds = new Vector2Int(1, 1);

        var rotatedChild = new GameObject("RotatedObject");
        rotatedChild.transform.SetParent(ghostObj.transform, false);
        placement.RotatedObject = rotatedChild.transform;

        // Prevent garbage collection — ghost template lives for the session
        UnityEngine.Object.DontDestroyOnLoad(ghostObj);

        return ghostObj;
    }

    /// <summary>
    /// Injects ToBuildAsset on Apple ghost CLONES so PlacementObject.Start() loads the
    /// real decoration prefab. Only targets clones (name contains "(Clone)") — the template's
    /// Start() runs with empty ToBuildAsset and returns early.
    /// PlaceObject() line 782 waits for GetComponentInChildren&lt;Structure&gt;() on the ghost.
    /// Without loading the real prefab, Structure never appears and the loop hangs forever
    /// (timeout uses Time.deltaTime which is 0 at timeScale=0).
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(PlacementObject), nameof(PlacementObject.Start))]
    public static void PlacementObject_Start_Prefix(PlacementObject __instance)
    {
        var name = __instance.gameObject.name;
        if (!name.Contains("AppleGhost_") || !name.Contains("(Clone)"))
        {
            return;
        }

        var typeName = name.Replace("AppleGhost_", "").Replace("(Clone)", "").Trim();
        if (Enum.TryParse<StructureBrain.TYPES>(typeName, out var type)
            && ExclusiveContent.AppleDecorationPrefabPaths.TryGetValue(type, out var path))
        {
            __instance.ToBuildAsset = path;
            Plugin.Log.LogInfo($"[AppleDecoFix] Injected ToBuildAsset for clone: {type} → {path}");
        }
    }

    #endregion

    #region Boss Skin Invariant Patches

    /// <summary>
    /// The game's GetSkinsFromLocation unconditionally filters out Invariant skins.
    /// Three boss skins (Death Cat, Aym, Baal) are Invariant, meaning they never appear
    /// in the follower forms menu even when unlocked. This postfix adds back unlocked
    /// Invariant boss skins so they appear in forms menus with "new" badges.
    /// Affects UIFollowerFormsMenuController, UIAppearanceMenuController_Form,
    /// UICustomizeClothesController_Form, and SetUnlockedText counters.
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(WorshipperData), nameof(WorshipperData.GetSkinsFromLocation))]
    public static void GetSkinsFromLocation_Postfix(
        WorshipperData __instance,
        WorshipperData.DropLocation dropLocation,
        ref List<WorshipperData.SkinAndData> __result)
    {
        if (!Plugin.EnableBossSkins.Value)
        {
            return;
        }

        foreach (var character in __instance.Characters)
        {
            if (!character.Invariant) continue;
            if (!character.Skin[0].Skin.Contains("Boss")) continue;
            if (!DataManager.GetFollowerSkinUnlocked(character.Skin[0].Skin)) continue;
            if (character.DropLocation != dropLocation) continue;
            if (__result.Exists(s => s.Title.Equals(character.Title))) continue;

            __result.Add(character);
        }
    }

    /// <summary>
    /// Snapshots active boss skin alerts before ReplaceDeprication clears them.
    /// ReplaceDeprication removes ALL Invariant skin alerts from _alerts on every save load.
    /// We capture which boss skin alerts are active so the postfix can restore them.
    /// </summary>
    private static readonly HashSet<string> _bossSkinAlertsBeforeReplace = [];

    [HarmonyPrefix]
    [HarmonyPatch(typeof(DataManager), nameof(DataManager.ReplaceDeprication))]
    public static void ReplaceDeprication_Prefix()
    {
        _bossSkinAlertsBeforeReplace.Clear();

        if (!Plugin.EnableBossSkins.Value)
        {
            return;
        }

        var alerts = DataManager.Instance?.Alerts?.CharacterSkinAlerts;
        if (alerts == null)
        {
            return;
        }

        foreach (var alert in alerts._alerts)
        {
            if (alert.Contains("Boss"))
            {
                _bossSkinAlertsBeforeReplace.Add(alert);
            }
        }
    }

    /// <summary>
    /// Restores boss skin alerts that were stripped by ReplaceDeprication's Invariant cleanup.
    /// Only restores alerts that were active before the cleanup (i.e., the user hasn't viewed them yet).
    /// Adds directly to _alerts to bypass the _singleAlerts check that blocks AddOnce/Add.
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(DataManager), nameof(DataManager.ReplaceDeprication))]
    public static void ReplaceDeprication_Postfix()
    {
        if (!Plugin.EnableBossSkins.Value)
        {
            return;
        }

        var alerts = DataManager.Instance?.Alerts?.CharacterSkinAlerts;
        if (alerts == null)
        {
            return;
        }

        foreach (var alertKey in _bossSkinAlertsBeforeReplace)
        {
            if (!alerts._alerts.Contains(alertKey))
            {
                alerts._alerts.Add(alertKey);
            }
        }
    }

    #endregion

    #region Forms Menu Alert Suppression

    /// <summary>
    /// When the forms menu opens, the first item is auto-selected via OverrideDefaultOnce + ActivateNavigation.
    /// IndoctrinationFormItem.OnSelect calls TryRemoveAlert(), which instantly removes the "new" badge
    /// before the player can see it. Boss skins sort first (not in MiscOrder) so they're always the victim.
    /// We suppress TryRemoveAlert for one frame after the menu opens to prevent this.
    /// </summary>
    private static bool _suppressAlertRemoval;

    [HarmonyPrefix]
    [HarmonyPatch(typeof(UIFollowerFormsMenuController), nameof(UIFollowerFormsMenuController.OnShowStarted))]
    public static void UIFollowerFormsMenuController_OnShowStarted_Prefix()
    {
        if (!Plugin.EnableBossSkins.Value)
        {
            return;
        }

        _suppressAlertRemoval = true;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(UIFollowerFormsMenuController), nameof(UIFollowerFormsMenuController.OnShowStarted))]
    public static void UIFollowerFormsMenuController_OnShowStarted_Postfix(UIFollowerFormsMenuController __instance)
    {
        if (!Plugin.EnableBossSkins.Value)
        {
            return;
        }

        __instance.StartCoroutine(ClearAlertSuppression());
    }

    private static IEnumerator ClearAlertSuppression()
    {
        yield return null;
        _suppressAlertRemoval = false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(IndoctrinationFormItem), nameof(IndoctrinationFormItem.OnSelect))]
    public static bool IndoctrinationFormItem_OnSelect_Prefix(IndoctrinationFormItem __instance)
    {
        if (!Plugin.EnableBossSkins.Value)
        {
            return true;
        }

        if (!_suppressAlertRemoval)
        {
            return true;
        }

        // Only suppress for boss skins — let all other skins clear badges normally
        return __instance.followerSkin == null || !__instance.followerSkin.Contains("Boss");
    }

    #endregion

}
