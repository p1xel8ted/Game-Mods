namespace CuisineerTweaks;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class Fixes
{
    public static int ResolutionWidth { get; set; }
    public static int ResolutionHeight { get; set; }
    public static FullScreenMode FullScreenMode { get; set; }
    public static float MaxRefreshRate { get; set; }
    private static float TimeScale => Utils.FindLowestFrameRateMultipleAboveFifty(MaxRefreshRate);
    private static Dictionary<string, int> OriginalItemStackSizes { get; } = new();
    private static Dictionary<string, float> OriginalWeaponTimes { get; } = new();
    private static int _lastSetResWidth;
    private static int _lastSetResHeight;
    private static FullScreenMode _lastSetFsMode;
    private static int _lastSetRefreshRate;


    internal static void UpdateWeaponCooldowns()
    {
        if (!Plugin.AttacksDamageEnabled.Value || Player.m_Instance == null) return;
        foreach (var slot in Player.RuntimeData.m_Weapons)
        {
            var weapon = slot.m_Weapon;
            if (weapon == null) continue;

            if (!OriginalWeaponTimes.TryGetValue(slot.m_Weapon.m_WeaponName, out var cooldown))
            {
                OriginalWeaponTimes[slot.m_Weapon.m_WeaponName] = weapon.m_SpecialAttackCooldown;
            }

            if (!Plugin.ModifyWeaponSpecialCooldown.Value)
            {
                weapon.m_SpecialAttackCooldown = OriginalWeaponTimes[slot.m_Weapon.m_WeaponName];
                Utils.WriteLog($"Reset weapon cooldown for {slot.m_Weapon.m_WeaponName} to {weapon.m_SpecialAttackCooldown}");

                continue;
            }

            var weaponCooldownPercent = Plugin.WeaponSpecialCooldownValue.Value;
            var newCooldown = cooldown * (1f - weaponCooldownPercent / 100f);

            weapon.m_SpecialAttackCooldown = newCooldown;
            Utils.WriteLog($"Updated weapon cooldown for {slot.m_Weapon.m_WeaponName} from {cooldown} to {newCooldown}");
        }
    }


    internal static void UpdateCameraZoom(bool showMessage = true)
    {
        if (!Plugin.ZoomEnabled.Value || !Plugin.AdjustableZoomLevel.Value) return;
        if (GameInstances.PlayerCameraInstance == null)
        {
            GameInstances.PlayerCameraInstance = Utils.FindIl2CppType<CinemachineVirtualCamera>()
                .FirstOrDefault(cam => cam != null && cam.name.Equals(Const.VcamPlayerCharacter));
        }

        if (GameInstances.PlayerCameraInstance == null || GameInstances.MapTransitionManagerInstance == null || GameInstances.MapTransitionManagerInstance.m_CurrMapData == null) return;

        var newZoom = GetNewZoomValue(Plugin.RelativeZoomAdjustment.Value);
        var originalZoom = GameInstances.MapTransitionManagerInstance.m_CurrMapData.m_OrthoSize;
        var difference = newZoom - originalZoom;

        Utils.Breadcrumb($"CameraZoom: m_Lens.OrthographicSize = {newZoom} (original={originalZoom})");
        GameInstances.PlayerCameraInstance.m_Lens = GameInstances.PlayerCameraInstance.m_Lens with { OrthographicSize = newZoom };
        Utils.Breadcrumb("CameraZoom: m_Lens OK");

        if (!showMessage) return;
        var message = Plugin.UseStaticZoomLevel.Value ? $"{Lang.GetZoomSetToMessage()} {Plugin.StaticZoomAdjustment.Value:F1}" : $"{Lang.GetZoomAdjustedByMessage()} {difference:F1}";
        Utils.ShowScreenMessage(message, 1);
    }

    internal static float GetNewZoomValue(float original)
    {
        if (GameInstances.MapTransitionManagerInstance == null || GameInstances.MapTransitionManagerInstance.m_CurrMapData == null)
        {
            return original;
        }

        var currentOrthographicSize = GameInstances.MapTransitionManagerInstance.m_CurrMapData.m_OrthoSize;

        if (!Plugin.ZoomEnabled.Value || !Plugin.AdjustableZoomLevel.Value)
        {
            return currentOrthographicSize;
        }

        if (Plugin.UseStaticZoomLevel.Value)
        {
            return Plugin.StaticZoomAdjustment.Value;
        }

        var adjustment = Mathf.Round(original * 10f) / 10f; // Directly round the original value
        var adjustedSize = currentOrthographicSize + adjustment; // Apply the adjustment directly

        // Ensure the adjusted size does not go below 0.5
        adjustedSize = Mathf.Max(adjustedSize, 0.5f);


        return adjustedSize;
    }


    internal static void UpdateItemStackSize(ItemInstance __instance)
    {
        if (!Plugin.InventoryEnabled.Value) return;
        if (__instance == null || __instance.ItemSO == null) return;
        if (__instance.ItemSO.Type is not (ItemType.Ingredient or ItemType.Potion or ItemType.Material)) return;

        if (!OriginalItemStackSizes.TryGetValue(__instance.ItemID, out var maxStack))
        {
            maxStack = __instance.ItemSO.m_MaxStack;
            OriginalItemStackSizes[__instance.ItemID] = maxStack;
        }

        if (!Plugin.IncreaseStackSize.Value)
        {
            __instance.ItemSO.m_MaxStack = OriginalItemStackSizes[__instance.ItemID];
            Utils.WriteLog($"Reset Stack Size: {__instance.ItemID} stack size: {maxStack} -> {OriginalItemStackSizes[__instance.ItemID]}");
            return;
        }

        if (Plugin.IncreaseStackSizeValue.Value > maxStack)
        {
            __instance.ItemSO.m_MaxStack = Plugin.IncreaseStackSizeValue.Value;
            Utils.WriteLog($"ItemInstance: {__instance.ItemID} stack size: {maxStack} -> {Plugin.IncreaseStackSizeValue.Value}");
        }
        else
        {
            Utils.WriteLog($"ItemInstance: {__instance.ItemID} stack size: {maxStack} (unchanged)");
        }
    }

    internal static void RunFixes(string scene)
    {
        Utils.Breadcrumb($"RunFixes START scene={scene}");
        Utils.Breadcrumb("RunFixes > UpdateResolutionFrameRate");
        UpdateResolutionFrameRate(force: true);
        Utils.Breadcrumb("RunFixes > UpdateFixedDeltaTime (skipped - deferred to LoadingScreen)");
        Utils.Breadcrumb("RunFixes > UpdateAutoSave");
        UpdateAutoSave();
        Utils.Breadcrumb("RunFixes > UpdateMainMenu");
        UpdateMainMenu(scene);
        Utils.Breadcrumb("RunFixes > UpdateCheats");
        UpdateCheats();
        Utils.Breadcrumb("RunFixes > UpdateCameraZoom");
        UpdateCameraZoom(false);
        Utils.Breadcrumb("RunFixes > UpdateScalers");
        UpdateScalers();
        Utils.Breadcrumb("RunFixes DONE");
    }

    private static void UpdateScalers()
    {
        if (!Plugin.PerformanceEnabled.Value) return;
        const float ultrawideAspect = 2.3f;
        var currentAspect = (float)Screen.currentResolution.width / Screen.currentResolution.height;
        var scalers = Utils.FindIl2CppType<CanvasScaler>();
        Utils.Breadcrumb($"UpdateScalers: aspect={currentAspect:F3}, count={scalers.Count}");
        foreach (var scaler in scalers)
        {
            Utils.Breadcrumb($"UpdateScalers: {scaler.name}");
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.screenMatchMode = currentAspect >= ultrawideAspect ? CanvasScaler.ScreenMatchMode.Expand : CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        }
        Utils.Breadcrumb("UpdateScalers DONE");
    }

    internal static void UpdateResolutionFrameRate(bool force = false)
    {
        if (ResolutionWidth == 0 || ResolutionHeight == 0 || FullScreenMode == 0 || MaxRefreshRate == 0) return;

        if (!Application.isFocused)
        {
            Utils.Breadcrumb("UpdateResolutionFrameRate SKIPPED (not focused)");
            return;
        }

        var rr = Mathf.RoundToInt(MaxRefreshRate);

        if (!force && _lastSetResWidth == ResolutionWidth && _lastSetResHeight == ResolutionHeight &&
            _lastSetFsMode == FullScreenMode && _lastSetRefreshRate == rr)
        {
            Utils.Breadcrumb($"UpdateResolutionFrameRate SKIPPED (unchanged {ResolutionWidth}x{ResolutionHeight}@{rr})");
            return;
        }

        Utils.Breadcrumb($"Screen.SetResolution({ResolutionWidth}, {ResolutionHeight}, {FullScreenMode}, {rr})");
        Screen.SetResolution(ResolutionWidth, ResolutionHeight, FullScreenMode, rr);
        _lastSetResWidth = ResolutionWidth;
        _lastSetResHeight = ResolutionHeight;
        _lastSetFsMode = FullScreenMode;
        _lastSetRefreshRate = rr;
        Utils.Breadcrumb("Screen.SetResolution OK");

        if (Application.targetFrameRate != rr)
        {
            Utils.Breadcrumb($"Application.targetFrameRate = {rr}");
            Application.targetFrameRate = rr;
            Utils.Breadcrumb("Application.targetFrameRate OK");
        }
    }

    public static void UpdateFixedDeltaTime()
    {
        if (!Plugin.PerformanceEnabled.Value || !Plugin.CorrectFixedUpdateRate.Value) return;

        if (MaxRefreshRate <= 0)
        {
            MaxRefreshRate = Screen.resolutions.Max(a => a.m_RefreshRate);
        }

        var targetFPS = Plugin.UseRefreshRateForFixedUpdateRate.Value ? MaxRefreshRate : TimeScale;
        var newValue = 1f / targetFPS;

        if (Mathf.Approximately(newValue, Time.fixedDeltaTime)) return;

        var oldFps = Mathf.Round(1f / Time.fixedDeltaTime);
        var newFps = Mathf.Round(1f / newValue);

        if (newFps < oldFps) return;

        Utils.Breadcrumb($"Time.fixedDeltaTime = {newValue} (was {Time.fixedDeltaTime}, target {targetFPS}fps)");
        Time.fixedDeltaTime = newValue;
        Utils.Breadcrumb("Time.fixedDeltaTime OK");
    }


    private static void UpdateCheats()
    {
        if (!Plugin.CheatsEnabled.Value) return;
        Utils.Breadcrumb($"UpdateCheats: DineAndDash={Plugin.DineAndDash.Value}, AutoPay={Plugin.AutomaticPayment.Value}, AutoCook={Plugin.AutoCook.Value}, FreeCook={Plugin.FreeCook.Value}, InstantKill={Plugin.InstantKill.Value}");
        Cheats.Customer.DisableDineAndDash = !Plugin.DineAndDash.Value;
        Utils.Breadcrumb("UpdateCheats: DisableDineAndDash OK");
        Cheats.Customer.AutoCollectPayment = Plugin.AutomaticPayment.Value;
        Utils.Breadcrumb("UpdateCheats: AutoCollectPayment OK");
        Cheats.CookingTools.AutoCook = Plugin.AutoCook.Value;
        Utils.Breadcrumb("UpdateCheats: AutoCook OK");
        Cheats.CookingTools.FreeCook = Plugin.FreeCook.Value;
        Utils.Breadcrumb("UpdateCheats: FreeCook OK");
        Player.OnePunchMode = Plugin.InstantKill.Value;
        Utils.Breadcrumb("UpdateCheats DONE");
    }

    private static void UpdateMainMenu(string scene)
    {
        if (!scene.Equals(Const.MainMenuScene)) return;
        if (!Plugin.PerformanceEnabled.Value || !Plugin.CorrectMainMenuAspect.Value) return;

        var currentAspect = ResolutionHeight > 0 && ResolutionWidth > 0
            ? (float)ResolutionWidth / ResolutionHeight
            : (float)Display.main.systemWidth / Display.main.systemHeight;

        if (currentAspect != 0 && currentAspect <= Const.BaseAspect) return;

        var positiveScaleFactor = currentAspect / Const.BaseAspect;
        var negativeScaleFactor = 1f / positiveScaleFactor;
        Utils.Breadcrumb($"UpdateMainMenu: aspect={currentAspect:F3}, scale+={positiveScaleFactor:F3}, scale-={negativeScaleFactor:F3}");

        Utils.Breadcrumb("UpdateMainMenu > Mask (disable mask)");
        Utils.ScaleElement("UI_MainMenuCanvas/Mask", true);
        Utils.Breadcrumb("UpdateMainMenu > Container (scale+)");
        Utils.ScaleElement("UI_MainMenuCanvas/Mask/UI_MainMenu/Container", false, positiveScaleFactor);
        Utils.Breadcrumb("UpdateMainMenu > MC (scale-)");
        Utils.ScaleElement("UI_MainMenuCanvas/Mask/UI_MainMenu/Container/Centre/MC", false, negativeScaleFactor);
        Utils.Breadcrumb("UpdateMainMenu > Press any key (scale-)");
        Utils.ScaleElement("UI_MainMenuCanvas/Mask/UI_MainMenu/Container/Press any key text", false, negativeScaleFactor);
        Utils.Breadcrumb("UpdateMainMenu > Logo (scale-)");
        Utils.ScaleElement("UI_MainMenuCanvas/Mask/UI_MainMenu/Container/CuisineerLogo", false, negativeScaleFactor);
        Utils.Breadcrumb("UpdateMainMenu > ButtonsBacking (scale-)");
        Utils.ScaleElement("UI_MainMenuCanvas/Mask/UI_MainMenu/Container/ButtonsBacking", false, negativeScaleFactor);
        Utils.Breadcrumb("UpdateMainMenu > SaveSlotDetail (scale-)");
        Utils.ScaleElement("UI_MainMenuCanvas/Mask/UI_MainMenu/Container/UI_SaveSlotDetail", false, negativeScaleFactor);
        Utils.Breadcrumb("UpdateMainMenu DONE");
    }

    internal static void UpdateInventoryStackSize()
    {
        if (!Plugin.InventoryEnabled.Value || InventoryManager.m_Instance == null) return;

        Utils.WriteLog("Updating Inventory Stack Sizes");

        foreach (var instanceMInventory in InventoryManager.Instance.m_Inventories)
        {
            if (instanceMInventory.Value == null) continue;

            foreach (var valueMSlot in instanceMInventory.Value.m_Slots)
            {
                if (valueMSlot?.ItemSO == null) continue;

                UpdateItemStackSize(valueMSlot);
            }
        }
    }


    private static void UpdateAutoSave()
    {
        if (!Plugin.SaveSystemEnabled.Value || CuisineerSaveManager.m_Instance == null) return;
        Utils.Breadcrumb($"UpdateAutoSave: enabled={Plugin.EnableAutoSave.Value}, freq={Plugin.AutoSaveFrequency.Value}");
        CuisineerSaveManager.Instance.m_AutoSave = Plugin.EnableAutoSave.Value;
        CuisineerSaveManager.Instance.m_AutoSaveFrequency = Plugin.AutoSaveFrequency.Value;
        Utils.Breadcrumb("UpdateAutoSave DONE");
    }
}