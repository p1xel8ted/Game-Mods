namespace BringOutYerDead;

[HarmonyPatch]
public static class Patches
{
    private static WorldGameObject _donkey;
    private static WorldGameObject _carrotBox;
    private static int _carrotCount;
    private static int _deliveryCount;
    private static bool _strikeDone;
    internal static LogicData Ld;
    
    public static void EnvironmentEngine_OnEndOfDay()
    {
        if (EnvironmentEngine.me == null) return;
        Plugin.InternalMorningDelivery.Value = false;
        Plugin.InternalDayDelivery.Value = false;
        Plugin.InternalEveningDelivery.Value = false;
        Plugin.InternalNightDelivery.Value = false;
        Plugin.PrideDayLogged = false;
        Helpers.Log("Resetting donkey day delivery flags!");   
    }

    internal static bool ForceDonkey(WorldGameObject donkey)
    {
        if (donkey == null)
        {
            donkey = WorldMap.GetWorldGameObjectByCustomTag("donkey", true);
            if (donkey == null)
            {
                Helpers.Log($"Donkey appears to be on a holiday and cannot be found!");
                return false;
            }
        }

        if (!Tools.TutorialDone())
        {
            Helpers.Log("Need to complete all 'tutorial' quests first, upto and including the repair the sword quest.");
            return false;
        }

        _strikeDone = donkey.GetParam("strike_completed") > 0f;
        if (!_strikeDone)
        {
            Helpers.Log($"Must complete the donkey strike first! Pay him 10 carrots, grease his wheels etc.");
            return false;
        }

        _carrotBox = WorldMap.GetWorldGameObjectByCustomTag("carrot_box", true);

        if (_carrotBox == null)
        {
            Helpers.Log($"No carrot box! How are you going to pay the donkey?!");
            return false;
        }

        if (_carrotBox.data.inventory.Count > 0)
        {
            _carrotCount = _carrotBox.data.inventory[0].value;
        }


        Helpers.Log($"Current carrots: {_carrotCount}");
        if (_carrotCount <= 0)
        {
            Helpers.Log($"No carrots! How are you going to pay the donkey?!");
            return false;
        }

        Helpers.Log("Forcing donkey to do his thing! Unless it's Pride/Sunday...");

        _donkey = WorldMap.GetWorldGameObjectByCustomTag("donkey", true);

        Ld = new LogicData("donkey");
        Ld.ForceExecute(false);

        if (_donkey != null)
        {
            Plugin.InternalDonkeySpawned.Value = true;
            Helpers.Log($"FD: Found donkey spawn!: Speed: {_donkey.data.GetParam("speed")}");
            _donkey.components.character.SetSpeed(Plugin.DonkeySpeed.Value);
            Helpers.Log($"FD: Found donkey spawn!: New Speed: {_donkey.data.GetParam("speed")}");

            return true;
        }

        return false;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(NewBodyArrivedGUI), nameof(NewBodyArrivedGUI.Display))]
    public static void NewBodyArrivedGUI_Display(ref NewBodyArrivedGUI __instance)
    {
        if (!MainGame.game_started) return;
        if (!Tools.TutorialDone()) return;
        if (!_strikeDone) return;
        if (__instance == null) return;
        _deliveryCount++;
        _donkey = WorldMap.GetWorldGameObjectByCustomTag("donkey", true);
        if (_donkey != null)
        {
            Helpers.Log($"Donkey on way home, setting speed! Current speed: {_donkey.data.GetParam("speed")}");
            _donkey.components.character.SetSpeed(Plugin.DonkeySpeed.Value);
        }

        _carrotBox = WorldMap.GetWorldGameObjectByCustomTag("carrot_box", true);
        if (_carrotBox != null)
        {
            if (_carrotBox.data.inventory.Count > 0)
            {
                _carrotCount = _carrotBox.data.inventory[0].value;
            }
        }

        if (_carrotCount <= 0)
        {
            Tools.ShowMessage(Helpers.GetLocalizedString(strings.CarrotMessage), MainGame.me.player_pos, sayAsPlayer: true);
        }

        Helpers.Log($"Current session delivery count: {_deliveryCount}!");
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(LogicData), nameof(LogicData.Execute))]
    public static bool LogicData_Execute(ref LogicData __instance)
    {
        if (!MainGame.game_started) return true;
        if (!Tools.TutorialDone()) return true;
        if (!_strikeDone) return true;
        if (__instance == null) return true;
        if (__instance.id == "donkey")
        {
            if (__instance == Ld)
            {
                Helpers.Log($"Helpers.LogicData_Execute: My donkey spawning!");
                return true;
            }

            Helpers.Log($"Helpers.LogicData_Execute: Game trying to spawn regular donkey, skipping!");
            return false;
        }

        return true;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.TeleportToGDPoint))]
    public static void WorldGameObject_TeleportToGDPoint(ref WorldGameObject __instance)
    {
        if (!MainGame.game_started) return;
        if (!Tools.TutorialDone()) return;
        if (!_strikeDone) return;
        if (__instance == null) return;
        if (string.IsNullOrEmpty(__instance.custom_tag) || string.IsNullOrWhiteSpace(__instance.custom_tag)) return;

        if (__instance.custom_tag.Equals("donkey"))
        {
            Helpers.Log("Donkey is home! Setting DonkeySpawned to false!");
            Plugin.InternalDonkeySpawned.Value = false;
        }
    }
}