using Steamworks;

namespace MoreTheMerrier;

[Harmony]
public static class Patches
{
    private const string MultiplayerCount = "MultiplayerCount";

    private static void SaveCount(int count)
    {
        PlayerPrefs.SetInt(MultiplayerCount, count);
    }
    
    private static int GetCount()
    {
        return PlayerPrefs.GetInt(MultiplayerCount, 8);
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(MainMenuController), nameof(MainMenuController.EnableMenu))]
    private static void MainMenuController_LoadLobbyMenu(MainMenuController __instance, GameObject menu)
    {
        if (menu != __instance.lobbySettingsMenu) return;

        var steamLobby = __instance.steamMenu;
        if (steamLobby)
        {
            var slider = steamLobby.maxNumPlayersSlider;
            slider.maxValue = 100;
            slider.minValue = 1;

            var lastCount = GetCount();
            slider.Set(lastCount, false);
            steamLobby.maximumNumberOfPlayers = lastCount;
            steamLobby.numPlayersInputField.SetText(lastCount.ToString(), false);

            slider.wholeNumbers = true;
            slider.onValueChanged = new Slider.SliderEvent();
            slider.onValueChanged.RemoveAllListeners();
            slider.onValueChanged.AddListener(value =>
            {
                var count = Mathf.RoundToInt(value);
                SaveCount(count);
                steamLobby.maximumNumberOfPlayers = count;
                steamLobby.numPlayersInputField.SetText(count.ToString(), false);
            });

            var text = steamLobby.numPlayersInputField;
            text.onValueChanged = new TMP_InputField.OnChangeEvent();
            text.onValueChanged.RemoveAllListeners();
            text.onValueChanged.AddListener(value =>
            {
                if (int.TryParse(value, out var result))
                {
                    SaveCount(result);
                    steamLobby.maxNumPlayersSlider.Set(result, false);
                    steamLobby.maximumNumberOfPlayers = result;
                }
            });
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(LocalizeText), nameof(LocalizeText.TranslateText))]
    public static void LocalizeText_TranslateText(string key, ref string __result)
    {
        if (key == "InviteFriendsInfo")
        {
            __result = __result.Replace("8", "100");
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(SteamMatchmaking), nameof(SteamMatchmaking.CreateLobby))]
    private static void SteamMatchmaking_CreateLobby(ref int cMaxMembers)
    {
        Plugin.Log.LogWarning($"Creating lobby with {cMaxMembers} members");
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(SteamLobby), nameof(SteamLobby.Start))]
    private static void SteamLobby_Start(SteamLobby __instance)
    {
        __instance.maxNumPlayersSlider.maxValue = 100;
        __instance.maxNumPlayersSlider.minValue = 1;
        __instance.maxNumPlayersSlider.wholeNumbers = true;
    }


    [HarmonyTranspiler]
    [HarmonyPatch(typeof(SteamLobby), nameof(SteamLobby.SetMaxNumPlayersFromSlider), typeof(float))]
    [HarmonyPatch(typeof(SteamLobby), nameof(SteamLobby.SetMaxNumPlayers), typeof(int))]
    private static IEnumerable<CodeInstruction> SteamLobby_SetMaxNumPlayersFromSlider(IEnumerable<CodeInstruction> instructions)
    {
        var codes = new List<CodeInstruction>(instructions);
        foreach (var t in codes)
        {
            if (t.opcode == OpCodes.Ldc_R4 && t.OperandIs(8))
            {
                t.operand = 100f;
            }

            if (t.opcode != OpCodes.Ldc_I4_8) continue;

            t.opcode = OpCodes.Ldc_I4_S;
            t.operand = 100;
        }

        return codes.AsEnumerable();
    }
}