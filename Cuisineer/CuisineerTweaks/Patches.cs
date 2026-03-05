namespace CuisineerTweaks;

[Harmony]
public static class Patches
{
    private static List<Furniture_CookingTool> RestaurantTools { get; } = [];
    private static float NextRegen { get; set; }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(ItemInstance), nameof(ItemInstance.Clone))]
    [HarmonyPatch(typeof(ItemInstance), nameof(ItemInstance.CloneWithCount))]
    [HarmonyPatch(typeof(ItemInstance), nameof(ItemInstance.CloneWithMods), typeof(Il2CppReferenceArray<ItemModSO>))]
    [HarmonyPatch(typeof(ItemInstance), nameof(ItemInstance.Insert))]
    [HarmonyPatch(typeof(ItemInstance), nameof(ItemInstance.Insert_IgnoreMaxStack))]
    [HarmonyPatch(typeof(ItemInstance), nameof(ItemInstance.Merge))]
    [HarmonyPatch(typeof(ItemInstance), nameof(ItemInstance.Remove))]
    [HarmonyPatch(typeof(ItemInstance), nameof(ItemInstance.SameAs))]
    [HarmonyPatch(typeof(ItemInstance), nameof(ItemInstance.SplitOutStack))]
    public static void ItemInstance_Patches(ref ItemInstance __instance)
    {
        try
        {
            if (!Plugin.InventoryEnabled.Value) return;
            Fixes.UpdateItemStackSize(__instance);
        }
        catch (System.Exception ex)
        {
            Utils.LogCrash(nameof(ItemInstance_Patches), ex);
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(UI_EquippedSlot_Belt), nameof(UI_EquippedSlot_Belt.SetupUI))]
    public static void UI_EquippedSlot_Belt_SetupUI(ref UI_EquippedSlot_Belt __instance, ref ItemInstance data)
    {
        try
        {
            if (!Plugin.AttacksDamageEnabled.Value) return;
            Fixes.UpdateWeaponCooldowns();
        }
        catch (System.Exception ex)
        {
            Utils.LogCrash(nameof(UI_EquippedSlot_Belt_SetupUI), ex);
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Inventory), nameof(Inventory.Sort))]
    public static void Inventory_Sort()
    {
        try
        {
            if (!Plugin.InventoryEnabled.Value) return;
            Fixes.UpdateInventoryStackSize();
        }
        catch (System.Exception ex)
        {
            Utils.LogCrash(nameof(Inventory_Sort), ex);
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Inventory), nameof(Inventory.InsertHelper))]
    public static void Inventory_SetSlotData(ref ItemInstance item)
    {
        try
        {
            if (!Plugin.InventoryEnabled.Value || !Plugin.IncreaseStackSize.Value || item == null) return;
            Fixes.UpdateItemStackSize(item);
        }
        catch (System.Exception ex)
        {
            Utils.LogCrash(nameof(Inventory_SetSlotData), ex);
        }
    }

    private static string _lastSceneName;
    private static int _lastFrame;

    [HarmonyPostfix]
    [HarmonyPatch(typeof(SceneManager), nameof(SceneManager.LoadScene), typeof(string))]
    [HarmonyPatch(typeof(SceneManager), nameof(SceneManager.LoadScene), typeof(string), typeof(LoadSceneParameters))]
    [HarmonyPatch(typeof(SceneManager), nameof(SceneManager.LoadScene), typeof(string), typeof(LoadSceneMode))]
    [HarmonyPatch(typeof(SceneManager), nameof(SceneManager.LoadScene), typeof(int), typeof(LoadSceneMode))]
    [HarmonyPatch(typeof(SceneManager), nameof(SceneManager.LoadScene), typeof(int), typeof(LoadSceneParameters))]
    [HarmonyPatch(typeof(SceneManager), nameof(SceneManager.LoadSceneAsync), typeof(string))]
    public static void SceneManager_LoadScene(object[] __args)
    {
        try
        {
            if (__args.Length == 0) return;
            var sceneName = __args[0] as string ?? (__args[0] is int index ? SceneManager.GetSceneByBuildIndex(index).name : null);
            if (sceneName == null) return;

            var currentFrame = Time.frameCount;
            if (_lastSceneName == sceneName && _lastFrame == currentFrame) return;

            _lastSceneName = sceneName;
            _lastFrame = currentFrame;

            Fixes.RunFixes(sceneName);
        }
        catch (System.Exception ex)
        {
            Utils.LogCrash(nameof(SceneManager_LoadScene), ex);
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(UI_CarpenterUpgradeArea), nameof(UI_CarpenterUpgradeArea.HandleUpgradeRestaurantClicked))]
    public static void UI_CarpenterUpgradeArea_HandleUpgradeRestaurantClicked(ref UI_CarpenterUpgradeArea __instance)
    {
        try
        {
            if (!Plugin.GameplayEnabled.Value || !Plugin.InstantRestaurantUpgrades.Value) return;
            var currentDateInt = GameInstances.CalendarManagerInstance.CurrDate;
            GlobalEvents.Narrative.OnFlagTrigger.Invoke(Const.UpgradeDateRestaurant, FlagType.Persisting, currentDateInt - 2);
            GameInstances.RestaurantDataManagerInstance.HandleDayChanged();
            if (GameInstances.RestaurantExtInstance != null)
            {
                GameInstances.RestaurantExtInstance.LoadRestaurantExterior();
            }
        }
        catch (System.Exception ex)
        {
            Utils.LogCrash(nameof(UI_CarpenterUpgradeArea_HandleUpgradeRestaurantClicked), ex);
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(RestaurantExt), nameof(RestaurantExt.Awake))]
    [HarmonyPatch(typeof(RestaurantExt), nameof(RestaurantExt.LoadRestaurantExterior))]
    public static void RestaurantExt_Awake(ref RestaurantExt __instance)
    {
        try
        {
            GameInstances.RestaurantExtInstance ??= __instance;
        }
        catch (System.Exception ex)
        {
            Utils.LogCrash(nameof(RestaurantExt_Awake), ex);
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Player), nameof(Player.LateUpdate))]
    public static void Player_OnEnable(ref Player __instance)
    {
        try
        {
            GameInstances.PlayerRuntimeDataInstance = __instance.m_RuntimeData;
            if (!Plugin.PlayerMovementEnabled.Value || !Plugin.IncreasePlayerMoveSpeed.Value) return;
            __instance.m_RuntimeData.m_MovementModifier = Plugin.PlayerMoveSpeedValue.Value;
            __instance.m_AnimHandler.Anim.speed = Plugin.PlayerMoveSpeedValue.Value;
        }
        catch (System.Exception ex)
        {
            Utils.LogCrash(nameof(Player_OnEnable), ex);
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Customer), nameof(Customer.FixedUpdate))]
    public static void Customer_FixedUpdate(ref Customer __instance)
    {
        try
        {
            if (!Plugin.RestaurantEnabled.Value || !Plugin.IncreaseCustomerMoveSpeed.Value) return;
            if (__instance == null || __instance.Data == null || __instance.m_Agent == null) return;

            var newSpeed = __instance.Data.m_MovementSpeed * Plugin.CustomerMoveSpeedValue.Value;
            __instance.m_Agent.speed = newSpeed;
            __instance.m_Agent.maxSpeed = newSpeed;

            if (Plugin.IncreaseCustomerMoveSpeedAnimation.Value)
            {
                __instance.m_WalkSpeedAnimMultiplier = newSpeed;
            }
        }
        catch (System.Exception ex)
        {
            Utils.LogCrash(nameof(Customer_FixedUpdate), ex);
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(UI_WeaponUpgrade), nameof(UI_WeaponUpgrade.ToggleClaimMode))]
    public static void UI_WeaponUpgrade_ToggleClaimMode(ref UI_WeaponUpgrade __instance)
    {
        try
        {
            if (!Plugin.GameplayEnabled.Value || !Plugin.InstantWeaponUpgrades.Value) return;
            __instance.ClaimEquipment();
        }
        catch (System.Exception ex)
        {
            Utils.LogCrash(nameof(UI_WeaponUpgrade_ToggleClaimMode), ex);
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(ItemDrop), nameof(ItemDrop.Setup), typeof(ItemInstance), typeof(Vector3), typeof(float), typeof(float), typeof(float), typeof(float))]
    public static void ItemDropManager_PickupItem(ref ItemDrop __instance)
    {
        try
        {
            if (!Plugin.GameplayEnabled.Value || !Plugin.ItemDropMultiplier.Value) return;
            if (__instance == null || __instance.m_ItemInstance == null) return;
            if (__instance.m_ItemInstance.m_ItemSO.Type is not (ItemType.Ingredient or ItemType.Potion or ItemType.Material)) return;
            __instance.m_ItemInstance.m_Stack = Mathf.RoundToInt(__instance.m_ItemInstance.m_Stack * Plugin.ItemDropMultiplierValue.Value);
        }
        catch (System.Exception ex)
        {
            Utils.LogCrash(nameof(ItemDropManager_PickupItem), ex);
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(BaseAttack), nameof(BaseAttack.HandleCollision))]
    public static bool BaseAttack_HandleCollision(BaseAttack __instance, Collider collider)
    {
        try
        {
            if (!Plugin.AttacksDamageEnabled.Value || !Plugin.OneHitDestructible.Value || __instance == null || collider == null) return true;

            var prop = collider.GetComponent<Prop>();
            if (prop == null || __instance.m_AttackPropertyType != AttackPropertyType.MELEE) return true;

            if ((prop.Destructible || prop._Destructible_k__BackingField) &&
                prop.Type is PropType.Destructible or PropType.ShatterDestructible or PropType.PassThroughDestructible)
            {
                for (var i = 0; i < 5; i++)
                {
                    if (__instance == null || prop == null) break;
                    __instance.HandleHitDestructible(prop);
                }

                return false;
            }

            return true;
        }
        catch (System.Exception ex)
        {
            Utils.LogCrash(nameof(BaseAttack_HandleCollision), ex);
            return true;
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(BaseAttack), nameof(BaseAttack.Update))]
    public static void BaseAttack_Update(ref BaseAttack __instance)
    {
        try
        {
            if (!Plugin.AttacksDamageEnabled.Value || !Plugin.RemoveChainAttackDelay.Value) return;
            if (!__instance._IsPlayer_k__BackingField) return;
            __instance.m_ChainAttDelay = 0f;
            __instance.m_CurrChainAttackDelay = 0f;
        }
        catch (System.Exception ex)
        {
            Utils.LogCrash(nameof(BaseAttack_Update), ex);
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(UI_BrewArea), nameof(UI_BrewArea.EnterBrew))]
    public static void UI_BrewArea_EnterBrew(ref UI_BrewArea __instance)
    {
        try
        {
            if (!Plugin.GameplayEnabled.Value || !Plugin.InstantBrew.Value) return;
            Utils.FastForwardBrewCraft(__instance.m_BrewConfirmationData);
            __instance.SwitchState(UI_BrewArea.Stage.Claim);
        }
        catch (System.Exception ex)
        {
            Utils.LogCrash(nameof(UI_BrewArea_EnterBrew), ex);
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(UI_BrewArea), nameof(UI_BrewArea.Show))]
    public static void UI_BrewArea_Show(ref UI_BrewArea __instance)
    {
        try
        {
            if (!Plugin.GameplayEnabled.Value || !Plugin.InstantBrew.Value) return;
            Utils.FastForwardBrewCraft(__instance.m_BrewConfirmationData);
        }
        catch (System.Exception ex)
        {
            Utils.LogCrash(nameof(UI_BrewArea_Show), ex);
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(UI_MainMenu), nameof(UI_MainMenu.HandleDependenciesReady))]
    public static void UI_MainMenu_Awake(ref UI_MainMenu __instance)
    {
        try
        {
            if (!Plugin.UserInterfaceEnabled.Value) return;
            Utils.Breadcrumb("UI_MainMenu_Awake START");
            __instance.m_FadeDuration = 0f;
            Utils.Breadcrumb("UI_MainMenu_Awake > m_FadeDuration OK");
            if (__instance.m_CreditBtn != null)
            {
                __instance.m_CreditBtn.gameObject.SetActive(false);
                Utils.Breadcrumb("UI_MainMenu_Awake > m_CreditBtn.SetActive OK");
                if (__instance.m_QuitGameBtn != null)
                {
                    __instance.m_QuitGameBtn.transform.localPosition = __instance.m_CreditBtn.transform.localPosition;
                    Utils.Breadcrumb("UI_MainMenu_Awake > m_QuitGameBtn reposition OK");
                }
            }
            if (__instance.m_ButtonsBackingAnimator != null)
            {
                __instance.m_ButtonsBackingAnimator.Activate();
                Utils.Breadcrumb("UI_MainMenu_Awake > m_ButtonsBackingAnimator OK");
            }
            __instance.m_WaitingForInput = false;
            Utils.Breadcrumb("UI_MainMenu_Awake > m_WaitingForInput OK");
            if (__instance.m_PressAnyButtonText != null)
            {
                __instance.m_PressAnyButtonText.CrossFadeAlpha(0, 0, true);
                Utils.Breadcrumb("UI_MainMenu_Awake > CrossFadeAlpha OK");
            }
            if (Plugin.SaveSystemEnabled.Value && Plugin.LoadToSaveMenu.Value)
            {
                Utils.Breadcrumb("UI_MainMenu_Awake > StartGameBtnClicked");
                __instance.StartGameBtnClicked();
                Utils.Breadcrumb("UI_MainMenu_Awake > StartGameBtnClicked OK");
            }

            if (Plugin.SaveSystemEnabled.Value && Plugin.AutoLoadSpecifiedSave.Value)
            {
                var slot = Plugin.AutoLoadSpecifiedSaveSlot.Value - 1;
                Utils.Breadcrumb($"UI_MainMenu_Awake > AutoLoad slot {slot}");
                if (!__instance.m_SaveSlotMenu.m_SaveSlots[slot].m_NewSave)
                {
                    __instance.m_SaveSlotMenu.m_SaveSlots[slot].HandleClicked();
                    Utils.Breadcrumb("UI_MainMenu_Awake > AutoLoad HandleClicked OK");
                }
                else
                {
                    Plugin.Logger.LogError($"AutoLoadSpecifiedSaveSlot: Chosen save slot {slot + 1} is empty!");
                }
            }
        }
        catch (System.Exception ex)
        {
            Utils.LogCrash(nameof(UI_MainMenu_Awake), ex);
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Player), nameof(Player.GetMaxHP))]
    public static void Player_MaxHP(ref Player __instance, ref int __result)
    {
        try
        {
            if (!Plugin.PlayerHealthEnabled.Value || !Plugin.ModifyPlayerMaxHp.Value) return;
            var originalHp = __result;
            var newHp = originalHp * Plugin.ModifyPlayerMaxHpMultiplier.Value;
            __result = Mathf.RoundToInt(newHp);
            Utils.WriteLog($"Player.GetMaxHP: Player max HP: {originalHp} -> {newHp} ({Plugin.ModifyPlayerMaxHpMultiplier.Value}x)");
        }
        catch (System.Exception ex)
        {
            Utils.LogCrash(nameof(Player_MaxHP), ex);
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Player), nameof(Player.LateUpdate))]
    public static void Player_LateUpdate(ref Player __instance)
    {
        try
        {
            if (Input.GetKeyUp(Plugin.KeybindReload.Value))
            {
                Utils.Breadcrumb("ConfigReload START");
                var oldValues = Utils.SnapshotConfig(Plugin.Instance.Config);
                Utils.Breadcrumb("ConfigReload > Config.Reload()");
                Plugin.Instance.Config.Reload();
                Utils.Breadcrumb("ConfigReload > Reload OK, snapshotting new values");
                var newValues = Utils.SnapshotConfig(Plugin.Instance.Config);
                Utils.WriteConfigDiff(oldValues, newValues);
                Utils.Breadcrumb("ConfigReload DONE");
                Utils.ShowScreenMessage("Config reloaded...", 1);
            }

            if (CuisineerSaveManager.m_Instance != null && Input.GetKeyUp(Plugin.KeybindSaveGame.Value))
            {
                CuisineerSaveManager.SaveCurrent();
                Plugin.Logger.LogInfo("Saved current game.");
            }

            if (Plugin.ZoomEnabled.Value && Plugin.AdjustableZoomLevel.Value && GameInstances.PlayerRuntimeDataInstance != null)
            {
                var change = 0f;
                if (Input.GetKeyUp(Plugin.KeybindZoomIn.Value))
                {
                    change = -0.1f;
                }
                else if (Input.GetKeyUp(Plugin.KeybindZoomOut.Value))
                {
                    change = 0.1f;
                }

                if (change != 0f)
                {
                    var newValue = Mathf.Round((Plugin.UseStaticZoomLevel.Value ? Plugin.StaticZoomAdjustment.Value : Plugin.RelativeZoomAdjustment.Value + change) * 10f) / 10f;
                    var newZoom = Fixes.GetNewZoomValue(newValue);

                    if (newZoom > 0.5f)
                    {
                        if (Plugin.UseStaticZoomLevel.Value)
                        {
                            Plugin.StaticZoomAdjustment.Value = newValue;
                        }
                        else
                        {
                            Plugin.RelativeZoomAdjustment.Value = newValue;
                        }

                        Fixes.UpdateCameraZoom();
                    }
                }
            }

            if (!Plugin.GameplayEnabled.Value || TimeManager.m_Instance == null) return;
            var shouldPause = Plugin.PauseTimeWhenViewingInventories.Value && UI_InventoryViewBase.AnyInventoryActive;
            TimeManager.ToggleTimePause(shouldPause);

            if (!Plugin.PlayerHealthEnabled.Value || !Plugin.RegenPlayerHp.Value) return;

            if (Time.time > NextRegen && __instance.m_RuntimeData.m_PlayerHP < __instance.m_RuntimeData.MaxAvailableHP)
            {
                NextRegen = Time.time + Plugin.RegenPlayerHpTick.Value;
                __instance.Heal(Plugin.RegenPlayerHpAmount.Value, !Plugin.RegenPlayerHpShowFloatingText.Value);
            }
        }
        catch (System.Exception ex)
        {
            Utils.LogCrash(nameof(Player_LateUpdate), ex);
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(GUI), nameof(GUI.Label), typeof(Rect), typeof(string), typeof(GUIStyle))]
    public static void GUI_Label(ref Rect position, ref string text, ref GUIStyle style)
    {
        try
        {
            var version = Application.version;
            var versionText = "v" + version;
            if (text.Contains(versionText, StringComparison.OrdinalIgnoreCase))
            {
                style.alignment = TextAnchor.UpperLeft;
                position.y -= 17;
                style.wordWrap = true;
                text += $"{Environment.NewLine}Cuisineer Tweaks v{Plugin.PluginVersion}";
            }
        }
        catch (System.Exception ex)
        {
            Utils.LogCrash(nameof(GUI_Label), ex);
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(UI_Cutscene), nameof(UI_Cutscene.RefreshText))]
    public static void UI_Cutscene_RefreshText(ref UI_Cutscene __instance)
    {
        try
        {
            if (!Plugin.UserInterfaceEnabled.Value || !Plugin.InstantText.Value || __instance == null) return;
            var speaker = __instance.m_CurrLine.m_MainSpeaker;
            if (speaker is not (SpeakerType.Left or SpeakerType.Right)) return;
            __instance.Skip();
        }
        catch (System.Exception ex)
        {
            Utils.LogCrash(nameof(UI_Cutscene_RefreshText), ex);
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(UI_GearModDisplay), nameof(UI_GearModDisplay.SetLockedState))]
    public static void UI_GearModDisplay_SetLockedState(ref bool state)
    {
        try
        {
            if (!Plugin.GameplayEnabled.Value || !Plugin.UnlockBothModSlots.Value) return;
            state = false;
        }
        catch (System.Exception ex)
        {
            Utils.LogCrash(nameof(UI_GearModDisplay_SetLockedState), ex);
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(CameraController), nameof(CameraController.HandleLightingModeChange))]
    public static void CameraController_HandleLightingModeChange()
    {
        try
        {
            if (!Plugin.ZoomEnabled.Value || !Plugin.AdjustableZoomLevel.Value) return;
            Fixes.UpdateCameraZoom(false);
        }
        catch (System.Exception ex)
        {
            Utils.LogCrash(nameof(CameraController_HandleLightingModeChange), ex);
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(CameraController), nameof(CameraController.SetZoom))]
    public static void CameraController_SetZoom(ref float zoom)
    {
        try
        {
            if (!Plugin.ZoomEnabled.Value || !Plugin.AdjustableZoomLevel.Value) return;
            zoom = Fixes.GetNewZoomValue(Plugin.UseStaticZoomLevel.Value ? Plugin.StaticZoomAdjustment.Value : Plugin.RelativeZoomAdjustment.Value);
        }
        catch (System.Exception ex)
        {
            Utils.LogCrash(nameof(CameraController_SetZoom), ex);
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(CameraController), nameof(CameraController.RestoreZoom))]
    public static bool CameraController_RestoreZoom(ref CameraController __instance)
    {
        try
        {
            if (!Plugin.ZoomEnabled.Value || !Plugin.AdjustableZoomLevel.Value) return true;
            __instance.m_TargetZoom = Fixes.GetNewZoomValue(Plugin.UseStaticZoomLevel.Value ? Plugin.StaticZoomAdjustment.Value : Plugin.RelativeZoomAdjustment.Value);
            __instance.m_RestoringZoom = true;
            return false;
        }
        catch (System.Exception ex)
        {
            Utils.LogCrash(nameof(CameraController_RestoreZoom), ex);
            return true;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(LoadingScreenProgressComponent), nameof(LoadingScreenProgressComponent.OnEnable))]
    [HarmonyPatch(typeof(LoadingScreenProgressComponent), nameof(LoadingScreenProgressComponent.OnDisable))]
    public static void LoadingScreenProgressComponent_OnDisable()
    {
        try
        {
            Utils.Breadcrumb("LoadingScreenProgressComponent fired");
            Fixes.UpdateResolutionFrameRate();
            Fixes.UpdateFixedDeltaTime();

            if (!Plugin.ZoomEnabled.Value || !Plugin.AdjustableZoomLevel.Value) return;
            Fixes.UpdateCameraZoom(false);
        }
        catch (System.Exception ex)
        {
            Utils.LogCrash(nameof(LoadingScreenProgressComponent_OnDisable), ex);
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(UI_Option), nameof(UI_Option.SetDropdownValue))]
    public static void UI_Option_SetDropdownValue(string value)
    {
        try
        {
            Utils.UpdateResolutionData(GameInstances.GameplayOptionsInstance);
        }
        catch (System.Exception ex)
        {
            Utils.LogCrash(nameof(UI_Option_SetDropdownValue), ex);
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(UI_EquippedSlot_Belt), nameof(UI_EquippedSlot_Belt.UpdateCurrentAmmoCount))]
    public static void UI_EquippedSlot_Belt_UpdateCurrentAmmoCount(ref UI_EquippedSlot_Belt __instance)
    {
        try
        {
            if (!Plugin.AttacksDamageEnabled.Value || !Plugin.AutoReloadWeapons.Value) return;

            var currWeapon = __instance.m_CurrWeapon;
            if (currWeapon is not { IsRanged: true }) return;

            var ammoCount = currWeapon.m_RangedWeapon.m_AmmoCount;
            if (currWeapon.m_CurrentAmmo < ammoCount)
            {
                GameInstances.PlayerRuntimeDataInstance?.ManualReload();
            }
        }
        catch (System.Exception ex)
        {
            Utils.LogCrash(nameof(UI_EquippedSlot_Belt_UpdateCurrentAmmoCount), ex);
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(UI_GameplayOptions), nameof(UI_GameplayOptions.HandleSelectedFramerate))]
    [HarmonyPatch(typeof(UI_GameplayOptions), nameof(UI_GameplayOptions.HandleSelectedResolution))]
    [HarmonyPatch(typeof(UI_GameplayOptions), nameof(UI_GameplayOptions.HandleSelectedFullscreenMode))]
    public static void UI_GameplayOptions_HandleSelectedFullscreenMode(ref UI_GameplayOptions __instance)
    {
        try
        {
            Utils.UpdateResolutionData(__instance);
        }
        catch (System.Exception ex)
        {
            Utils.LogCrash(nameof(UI_GameplayOptions_HandleSelectedFullscreenMode), ex);
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(RestaurantToolManager), nameof(RestaurantToolManager.AddToolToDictionary))]
    public static void RestaurantToolManager_AddToolToDictionary(ToolType tooltype, Furniture_CookingTool tool)
    {
        try
        {
            if (!Plugin.RestaurantEnabled.Value) return;
            RestaurantTools.Add(tool);
            Utils.WriteLog($"RestaurantToolManager.AddToolToDictionary: Added {tool.name} ({tooltype.ToString()}) to RestaurantTools");
        }
        catch (System.Exception ex)
        {
            Utils.LogCrash(nameof(RestaurantToolManager_AddToolToDictionary), ex);
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(CookingTracker), nameof(CookingTracker.HandleAddQueue))]
    public static void CookingTracker_HandleAddQueue(ref Furniture_CookingTool tool, ref RecipeSO recipe)
    {
        try
        {
            if (!Plugin.RestaurantEnabled.Value) return;
            var count = RestaurantTools.RemoveAll(a => a == null);
            Utils.WriteLog($"CookingTracker.HandleAddQueue: Removed {count} null tools from RestaurantTools");
            foreach (var t in RestaurantTools.Where(t => t != null))
            {
                if (t.CanCook(recipe) && !t.Full && !t.CookingSomething)
                {
                    tool = t;
                    Utils.WriteLog($"CookingTracker.HandleAddQueue: Changed tool to {t.name}");
                    return;
                }
            }
        }
        catch (System.Exception ex)
        {
            Utils.LogCrash(nameof(CookingTracker_HandleAddQueue), ex);
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Customer), nameof(Customer.SetupData))]
    public static void Customer_SetupData(ref Customer __instance)
    {
        try
        {
            if (!Plugin.RestaurantEnabled.Value) return;
            if (!__instance.Data.m_SelfService && Plugin.AllCustomersSelfServe.Value)
            {
                Utils.WriteLog($"Customer {__instance.Data.name} is now smart enough for self-service!");
                __instance.Data.m_SelfService = true;
            }
        }
        catch (System.Exception ex)
        {
            Utils.LogCrash(nameof(Customer_SetupData), ex);
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(UI_GameplayOptions), nameof(UI_GameplayOptions.SetupOptions))]
    public static void UI_GameplayOptions_Setup_Postfix(ref UI_GameplayOptions __instance)
    {
        try
        {
            Utils.UpdateResolutionData(__instance);
        }
        catch (System.Exception ex)
        {
            Utils.LogCrash(nameof(UI_GameplayOptions_Setup_Postfix), ex);
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(UI_GameplayOptions), nameof(UI_GameplayOptions.Update))]
    public static void UI_GameplayOptions_Update(ref UI_GameplayOptions __instance)
    {
        try
        {
            GameInstances.GameplayOptionsInstance = __instance;
        }
        catch (System.Exception ex)
        {
            Utils.LogCrash(nameof(UI_GameplayOptions_Update), ex);
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(UI_GameplayOptions), nameof(UI_GameplayOptions.SetupOptions))]
    public static void UI_GameplayOptions_Setup_Prefix(ref UI_GameplayOptions __instance)
    {
        try
        {
            var myResData = new ResolutionData
            {
                m_Height = Display._mainDisplay.systemHeight,
                m_Width = Display._mainDisplay.systemWidth
            };
            var resDatas = UI_GameplayOptions.ResolutionDatas.ToList();
            if (!resDatas.Exists(a => a.m_Height == Display._mainDisplay.systemHeight && a.m_Width == Display._mainDisplay.systemWidth))
            {
                resDatas.Add(myResData);
                Utils.WriteLog($"Main display resolution not detected; added {myResData.m_Width}x{myResData.m_Height} to resolution list");
            }

            UI_GameplayOptions.ResolutionDatas = resDatas.ToArray();

            var frameRateDatas = UI_GameplayOptions.FramerateDatas.ToList();

            var refreshRates = Screen.resolutions
                .Select(resolution => resolution.refreshRate)
                .Distinct()
                .ToList();

            foreach (var refreshRate in refreshRates.Where(refreshRate => frameRateDatas.All(a => a.m_FPS != refreshRate)))
            {
                if (refreshRate < 50) continue;
                frameRateDatas.Add(new FramerateData { m_FPS = refreshRate });
                Utils.WriteLog($"{refreshRate}Hz not detected in Target Framerate options; adding now.");
            }

            frameRateDatas.Sort((a, b) => b.m_FPS.CompareTo(a.m_FPS));

            UI_GameplayOptions.FramerateDatas = frameRateDatas.ToArray();
        }
        catch (System.Exception ex)
        {
            Utils.LogCrash(nameof(UI_GameplayOptions_Setup_Prefix), ex);
        }
    }

#if DEBUG
    [HarmonyPostfix]
    [HarmonyPatch(typeof(CurrencyManager), nameof(CurrencyManager.CanAfford), typeof(int))]
    [HarmonyPatch(typeof(CurrencyManager), nameof(CurrencyManager.CanAfford), typeof(ShopInventory), typeof(int))]
    [HarmonyPatch(typeof(CurrencyManager), nameof(CurrencyManager.CanAfford), typeof(Cost), typeof(int), typeof(int))]
    [HarmonyPatch(typeof(CurrencyManager), nameof(CurrencyManager.CanAffordMaterials))]
    public static void CurrencyManager_CanAfford(ref bool __result)
    {
        __result = true;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(CurrencyManager), nameof(CurrencyManager.Spend), typeof(Cost), typeof(int), typeof(int))]
    [HarmonyPatch(typeof(CurrencyManager), nameof(CurrencyManager.Spend), typeof(ShopInventory), typeof(int))]
    [HarmonyPatch(typeof(CurrencyManager), nameof(CurrencyManager.SpendCoins))]
    public static bool CurrencyManager_Spend()
    {
        return false;
    }
#endif
}
