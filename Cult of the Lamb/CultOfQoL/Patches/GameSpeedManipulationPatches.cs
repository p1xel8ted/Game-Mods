using System.Globalization;

namespace CultOfQoL.Patches;

[HarmonyPatch]
public static class GameSpeedManipulationPatches
{
    private static float _newGameSpeed;
    private static int _newSpeed;
    private static bool _timeMessageShown;

    private readonly static List<float> gameSpeedShort = [0.25f, 1, 2, 3, 4, 5];

    private readonly static List<float> gameSpeed =
    [
        0.25f, 0.5f, 0.75f, 1, 1.25f, 1.5f, 1.75f, 2,
        2.25f, 2.5f, 2.75f, 3, 3.25f, 3.5f, 3.75f, 4,
        4.25f, 4.5f, 4.75f, 5
    ];

    [HarmonyPrefix]
    [HarmonyPatch(typeof(TimeManager), nameof(TimeManager.Simulate), typeof(float), typeof(bool))]
    public static void TimeManager_Simulate(ref float deltaGameTime, ref bool skippingTime)
    {
        if (!Plugin.SlowDownTime.Value || skippingTime) return;

        if (float.TryParse(Plugin.SlowDownTimeMultiplier.Value.ToString(CultureInfo.InvariantCulture), out var value) && value > 0)
        {
            deltaGameTime /= value;
        }
        else
        {
            Plugin.Log.LogError(value <= 0 ? "SlowDownTimeMultiplier must be greater than 0." : "SlowDownTimeMultiplier is not a valid float.");
        }
    }

    internal static void ResetTime()
    {
        var baseSpeedIndex = Plugin.ShortenGameSpeedIncrements.Value ? 1 : 3;
        _newGameSpeed = baseSpeedIndex;
        _newSpeed = baseSpeedIndex;

        if (!Plugin.EnableGameSpeedManipulation.Value)
        {
            _newGameSpeed = 0;
            _newSpeed = 0;
        }

        if (GameManager.instance)
        {
            GameManager.instance.CurrentGameSpeed = _newSpeed;

            var speedList = Plugin.ShortenGameSpeedIncrements.Value ? gameSpeedShort : gameSpeed;
            _newSpeed = GameManager.instance.CurrentGameSpeed % speedList.Count;
            _newGameSpeed = speedList[_newSpeed];
        }

        GameManager.SetTimeScale(1);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameManager), nameof(GameManager.OnEnable))]
    [HarmonyPatch(typeof(GameManager), nameof(GameManager.Awake))]
    [HarmonyPatch(typeof(GameManager), nameof(GameManager.Start))]
    public static void GameManager_Start(GameManager __instance)
    {
        if (__instance)
        {
            ResetTime();
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameManager), nameof(GameManager.Update))]
    public static void GameManager_Update(GameManager __instance)
    {
        if (!__instance) return;

        if (Plugin.SlowDownTime.Value && !_timeMessageShown)
        {
            _timeMessageShown = true;
            NotificationCentre.Instance.PlayGenericNotification($"Slow down time enabled at {Mathf.Abs(Plugin.SlowDownTimeMultiplier.Value)}x.");
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ResetTime();
            NotificationCentre.Instance.PlayGenericNotification("Returned game speed to 1 (default)");
            return;
        }

        if (!Plugin.EnableGameSpeedManipulation.Value) return;

        var speedList = Plugin.ShortenGameSpeedIncrements.Value ? gameSpeedShort : gameSpeed;
        var speedCount = speedList.Count;

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            UpdateGameSpeed(__instance.CurrentGameSpeed + 1);
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            UpdateGameSpeed(__instance.CurrentGameSpeed - 1);
            return;
        }

        if (_newGameSpeed <= 0 || _newSpeed <= 0 || __instance.CurrentGameSpeed <= 0)
        {
            ResetTime();
        }

        __instance.CurrentGameSpeed = _newSpeed;
        return;

        void UpdateGameSpeed(int newSpeedIndex)
        {
            _newSpeed = newSpeedIndex % speedCount;
            _newGameSpeed = speedList[_newSpeed];
            GameManager.SetTimeScale(_newGameSpeed);
            NotificationCentre.Instance.PlayGenericNotification(
                Math.Abs(_newGameSpeed - 1) < 0.001f
                    ? $"Returned game speed to {_newGameSpeed} (default)"
                    : $"Adjusted game speed to {_newGameSpeed}");
        }
    }
}