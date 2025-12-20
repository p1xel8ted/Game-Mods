using System.Collections;

using Il2Cpp;
using Il2CppCinemachine;

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


    internal static void UpdateWeaponCooldowns()
    {
        if (Player.m_Instance == null || Player.RuntimeData == null) return;
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
        if (!Plugin.AdjustableZoomLevel.Value) return;
        if (GameInstances.PlayerCameraInstance == null)
        {
            var cameras = Utils.FindIl2CppType<CinemachineVirtualCamera>();
            foreach (var cam in cameras.Where(cam => cam != null && cam.name.Equals(Const.VcamPlayerCharacter)))
            {
                GameInstances.PlayerCameraInstance = cam;
                break;
            }
        }

        if (GameInstances.PlayerCameraInstance == null || GameInstances.MapTransitionManagerInstance == null || GameInstances.MapTransitionManagerInstance.m_CurrMapData == null) return;

        var newZoom = GetNewZoomValue(Plugin.RelativeZoomAdjustment.Value);
        var originalZoom = GameInstances.MapTransitionManagerInstance.m_CurrMapData.m_OrthoSize;
        var difference = newZoom - originalZoom;

        GameInstances.PlayerCameraInstance.m_Lens = GameInstances.PlayerCameraInstance.m_Lens with { OrthographicSize = newZoom };

        if (!showMessage) return;
        var message = Plugin.UseStaticZoomLevel.Value ? $"{Lang.GetZoomSetToMessage()} {Plugin.StaticZoomAdjustment.Value:F1}" : $"{Lang.GetZoomAdjustedByMessage()} {difference:F1}";
        Utils.ShowScreenMessage(message, 1);
    }

    internal static float GetNewZoomValue(float original)
    {
        var currentOrthographicSize = GameInstances.MapTransitionManagerInstance.m_CurrMapData.m_OrthoSize;

        if (!Plugin.AdjustableZoomLevel.Value)
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

    internal static void RunFixes(string scene, bool refresh = false)
    {
        // Utils.WriteLog(!refresh ? $"New Scene {scene} Loaded: Running Fixes" : "Refresh Requested: Running Fixes", true);

        UpdateResolutionFrameRate();
        UpdateFixedDeltaTime();
        UpdateAutoSave();
        // UpdateInventoryStackSize();
        // UpdateWeaponCooldowns();
        UpdateMainMenu(scene);
     UpdateCheats();
        UpdateCameraZoom(false);
        UpdateScalers();
    }

    private static void UpdateScalers()
    {
        const float ultrawideAspect = 2.3f;
        var currentAspect = (float)Screen.currentResolution.width / Screen.currentResolution.height;
        var scalers = Utils.FindIl2CppType<CanvasScaler>();
        foreach (var scaler in scalers)
        {
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.screenMatchMode = currentAspect >= ultrawideAspect ? CanvasScaler.ScreenMatchMode.Expand : CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        }
    }

    internal static void UpdateResolutionFrameRate()
    {
        if (ResolutionWidth == 0 || ResolutionHeight == 0 || FullScreenMode == 0 || MaxRefreshRate == 0) return;

        var rr = Mathf.RoundToInt(MaxRefreshRate);

        Screen.SetResolution(ResolutionWidth, ResolutionHeight, FullScreenMode, rr);

        Utils.WriteLog($"Set resolution to {Screen.currentResolution}");

        if (Application.targetFrameRate != rr)
        {
            Application.targetFrameRate = rr;
            Utils.WriteLog($"Set targetFrameRate to {Application.targetFrameRate}.");
        }
        else
        {
            Utils.WriteLog($"targetFrameRate is already {Application.targetFrameRate}. No update necessary.");
        }
    }

    public static void UpdateFixedDeltaTime()
    {
        if (!Plugin.CorrectFixedUpdateRate.Value) return;

        if (MaxRefreshRate <= 0)
        {
            //hasnt initialized yet
            MaxRefreshRate = Screen.resolutions.Max(a => a.m_RefreshRate);
        }

        var targetFPS = Plugin.UseRefreshRateForFixedUpdateRate.Value ? MaxRefreshRate : TimeScale;

        MelonLogger.Msg($"Using targetFPS: {targetFPS} for fixedDeltaTime calculation.");
        var newValue = 1f / targetFPS;

        if (Mathf.Approximately(newValue, Time.fixedDeltaTime))
        {
            Utils.WriteLog($"fixedDeltaTime is already {newValue} ({targetFPS}fps). No update necessary.");
            return;
        }

        var oldFps = Mathf.Round(1f / Time.fixedDeltaTime);
        var newFps = Mathf.Round(1f / newValue);

        if (newFps < oldFps)
        {
            Utils.WriteLog($"Cannot set fixedDeltaTime to {newValue} ({newFps}fps), it is less than the original {Time.fixedDeltaTime} ({oldFps}fps).");
            return;
        }

        Time.fixedDeltaTime = newValue;
        //  Utils.WriteLog($"Time.fixedDeltaTime: {Time.fixedDeltaTime}");
        Utils.WriteLog($"Set fixedDeltaTime to {newValue} ({targetFPS}fps).");
    }


    private static void UpdateCheats()
    {
        Cheats.Customer.DisableDineAndDash = !Plugin.DineAndDash.Value;
        Cheats.Customer.AutoCollectPayment = Plugin.AutomaticPayment.Value;
        Cheats.CookingTools.AutoCook = Plugin.AutoCook.Value;
        Cheats.CookingTools.FreeCook = Plugin.FreeCook.Value;
        Player.OnePunchMode = Plugin.InstantKill.Value;
    }

    private static void UpdateMainMenu(string scene)
    {
        if (!scene.Equals(Const.MainMenuScene)) return;
        if (!Plugin.CorrectMainMenuAspect.Value) return;

        float currentAspect;
        if (ResolutionHeight == 0 || ResolutionWidth == 0)
        {
            currentAspect = (float)Display.main.systemWidth / Display.main.systemHeight;
        }
        else
        {
            currentAspect = (float)ResolutionWidth / ResolutionHeight;
        }

        if (currentAspect != 0 && currentAspect <= Const.BaseAspect) return;

        var positiveScaleFactor = currentAspect / Const.BaseAspect;
        var negativeScaleFactor = 1f / positiveScaleFactor;
        Utils.WriteLog($"Current aspect ratio ({currentAspect}) is greater than base aspect ratio ({Const.BaseAspect}). Resizing UI elements.");

        Utils.ScaleElement("UI_MainMenuCanvas/Mask", true);
        Utils.ScaleElement("UI_MainMenuCanvas/Mask/UI_MainMenu/Container", false, positiveScaleFactor);
        Utils.ScaleElement("UI_MainMenuCanvas/Mask/UI_MainMenu/Container/Centre/MC", false, negativeScaleFactor);
        Utils.ScaleElement("UI_MainMenuCanvas/Mask/UI_MainMenu/Container/Press any key text", false, negativeScaleFactor);
        Utils.ScaleElement("UI_MainMenuCanvas/Mask/UI_MainMenu/Container/CuisineerLogo", false, negativeScaleFactor);
        Utils.ScaleElement("UI_MainMenuCanvas/Mask/UI_MainMenu/Container/ButtonsBacking", false, negativeScaleFactor);
        Utils.ScaleElement("UI_MainMenuCanvas/Mask/UI_MainMenu/Container/UI_SaveSlotDetail", false, negativeScaleFactor);
    }

    internal static void UpdateInventoryStackSize()
    {
        if (InventoryManager.m_Instance == null) return;

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
        if (CuisineerSaveManager.m_Instance == null) return;
        Utils.WriteLog("Initiating AutoSave");
        CuisineerSaveManager.Instance.m_AutoSave = Plugin.EnableAutoSave.Value;
        CuisineerSaveManager.Instance.m_AutoSaveFrequency = Plugin.AutoSaveFrequency.Value;
        Utils.WriteLog($"AutoSave: {CuisineerSaveManager.Instance.m_AutoSave} ({CuisineerSaveManager.Instance.m_AutoSaveFrequency / 60f} minutes)");
    }
}