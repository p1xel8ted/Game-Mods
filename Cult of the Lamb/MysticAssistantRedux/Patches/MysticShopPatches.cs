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
        __instance.SecondaryLabel = DataManager.Instance.MysticKeeperName + "'s assistant";
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

        switch (boughtItemType)
        {
            case InventoryItem.ITEM_TYPE.BLACK_GOLD:
                Inventory.ChangeItemQuantity((int)boughtItemType, 100, 0);
                break;

            case InventoryItem.ITEM_TYPE.Necklace_Dark:
            case InventoryItem.ITEM_TYPE.Necklace_Light:
                Inventory.ChangeItemQuantity((int)boughtItemType, 1, 0);
                break;

            case InventoryItem.ITEM_TYPE.CRYSTAL_DOCTRINE_STONE:
                Inventory.ChangeItemQuantity((int)boughtItemType, 1, 0);
                Plugin.CurrentInventoryManager.ChangeShopStockByQuantity(boughtItemType, -1);

                if (DataManager.Instance.CrystalDoctrinesReceivedFromMysticShop < Interaction_MysticShop.maxAmountOfCrystalDoctrines)
                {
                    DataManager.Instance.CrystalDoctrinesReceivedFromMysticShop++;
                }

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
                Plugin.CurrentInventoryManager.RemoveItemFromListByTypeAndIndex(boughtItemType, skinIndex);
                Plugin.CurrentInventoryManager.ChangeShopStockByQuantity(boughtItemType, -1);
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
                if (!Plugin.CurrentInventoryManager.BoughtRelic)
                {
                    Plugin.PostShopActions.Add(ShowUnlockedRelics);
                    Plugin.CurrentInventoryManager.SetBoughtRelicFlag(true);
                }
                break;

            default:
                Inventory.ChangeItemQuantity((int)boughtItemType, 1, 0);
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
    public static bool RefreshContextText_Prefix(
        UIItemSelectorOverlayController __instance,
        ItemSelector.Params ____params,
        ItemSelector.Category ____category,
        TextMeshProUGUI ____buttonPromptText,
        string ____addtionalText,
        string ____contextString)
    {
        if (____params.Key != Plugin.ShopContextKey)
        {
            return true; // Run original method
        }

        if (____params.Context != ItemSelector.Context.Buy)
        {
            return false;
        }

        var traderTrackerItems = __instance.CostProvider?.Invoke(____category.MostRecentItem);
        if (traderTrackerItems == null)
        {
            return false;
        }

        if (Plugin.ShowOverbuyWarning)
        {
            ____buttonPromptText.text = " <color=red>You are buying more of this than the game normally allows. Click it again to confirm.</color>";
        }
        else
        {
            if (____category == null)
            {
                ____buttonPromptText.text = string.Format(____contextString, "something broke, please leave the shop and try again",
                    CostFormatter.FormatCost(InventoryItem.ITEM_TYPE.GOD_TEAR, traderTrackerItems.SellPriceActual, true, false)) + ____addtionalText;
            }
            else
            {
                ____buttonPromptText.text = string.Format(____contextString,
                    InventoryInfo.GetShopLabelByItemType(____category.MostRecentItem),
                    CostFormatter.FormatCost(InventoryItem.ITEM_TYPE.GOD_TEAR, traderTrackerItems.SellPriceActual, true, false)) + ____addtionalText;
            }
        }

        return false;
    }

    /// <summary>
    /// Resets overbuy warning when selecting a different item.
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(UIItemSelectorOverlayController), nameof(UIItemSelectorOverlayController.OnItemSelected))]
    public static bool OnItemSelected_Prefix()
    {
        Plugin.ShowOverbuyWarning = false;
        return true; // Continue to original method
    }

    /// <summary>
    /// Handles item click with overbuy warning system.
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(UIItemSelectorOverlayController), nameof(UIItemSelectorOverlayController.OnItemClicked))]
    public static bool OnItemClicked_Prefix(
        UIItemSelectorOverlayController __instance,
        ItemSelector.Params ____params,
        GenericInventoryItem item)
    {
        if (____params.Key != Plugin.ShopContextKey)
        {
            return true; // Run original method
        }

        // Direct method call - assemblies are publicized
        var quantity = __instance.GetItemQuantity(item.Type);

        if (quantity > 0 && ____params.Context == ItemSelector.Context.Buy)
        {
            var traderTrackerItems = __instance.CostProvider?.Invoke(item.Type);
            if (traderTrackerItems != null && Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.GOD_TEAR) >= traderTrackerItems.SellPriceActual)
            {
                // Check for overbuy warning
                if (!Plugin.ShowOverbuyWarning && InventoryInfo.CheckForBoughtQuantityWarning(traderTrackerItems))
                {
                    Plugin.ShowOverbuyWarning = true;
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
                    if (____params.HideOnSelection)
                    {
                        __instance.Hide();
                    }
                    else
                    {
                        __instance.UpdateQuantities();
                    }
                    Plugin.ShowOverbuyWarning = false;
                }

                // Direct method call - assemblies are publicized
                __instance.RefreshContextText();
                return false;
            }
        }

        // Can't afford or no stock
        item.Shake();
        UIManager.PlayAudio("event:/ui/negative_feedback");
        return false;
    }

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

    private static void SetMysticShopInteractable(Interaction_MysticShop instance, bool active)
    {
        instance.Interactable = active;
    }

    #endregion
}
