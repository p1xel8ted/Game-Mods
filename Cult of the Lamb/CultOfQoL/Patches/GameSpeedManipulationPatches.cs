namespace CultOfQoL.Patches;

[HarmonyPatch]
public static class GameSpeedManipulationPatches
{
    private static float _newGameSpeed;
    private static int _newSpeed;
    private static bool _timeMessageShown;
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(TimeManager), nameof(TimeManager.Simulate), typeof(float))]
    public static void TimeManager_Simulate(ref float deltaGameTime)
    {
        if (Plugin.SlowDownTime.Value)
            //4 is SUPER slow. 
            deltaGameTime /= Mathf.Abs(Plugin.SlowDownTimeMultiplier.Value);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameManager), nameof(GameManager.Update))]
    public static void GameManager_Update(GameManager? __instance)
    {
        if (__instance is null) return;
        if (Plugin.SlowDownTime.Value && !_timeMessageShown)
        {
            _timeMessageShown = true;
            NotificationCentre.Instance.PlayGenericNotification($"Slow down time enabled at {Mathf.Abs(Plugin.SlowDownTimeMultiplier.Value)}x.");
        }


        if (!Plugin.EnableGameSpeedManipulation.Value) return;
        var gameSpeedShort = new List<float>
        {
            0,
            1,
            2,
            3,
            4,
            5
        };
        var gameSpeed = new List<float>
        {
            0,
            1,
            1.25f,
            1.5f,
            1.75f,
            2,
            2.25f,
            2.5f,
            2.75f,
            3,
            3.25f,
            3.5f,
            3.75f,
            4,
            4.25f,
            4.5f,
            4.75f,
            5
        };


        int num;
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            __instance.CurrentGameSpeed = 1;
            _newGameSpeed = 1;
            GameManager.SetTimeScale(1);
            NotificationCentre.Instance.PlayGenericNotification("Returned game speed to 1 (default)"
            );
            return;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            num = __instance.CurrentGameSpeed + 1;
            if (Plugin.ShortenGameSpeedIncrements.Value)
            {
                __instance.CurrentGameSpeed = num % gameSpeedShort.Count;
                _newSpeed = num % gameSpeedShort.Count;
                _newGameSpeed = gameSpeedShort[__instance.CurrentGameSpeed];
            }
            else
            {
                __instance.CurrentGameSpeed = num % gameSpeed.Count;
                _newSpeed = num % gameSpeed.Count;
                _newGameSpeed = gameSpeed[__instance.CurrentGameSpeed];
            }

            GameManager.SetTimeScale(_newGameSpeed);
            NotificationCentre.Instance.PlayGenericNotification(Math.Abs(_newGameSpeed - 1) < 0.001f
                ? $"Returned game speed to {_newGameSpeed} (default)"
                : $"Increased game speed to {_newGameSpeed}");
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            num = __instance.CurrentGameSpeed - 1;
            if (Plugin.ShortenGameSpeedIncrements.Value)
            {
                __instance.CurrentGameSpeed = num % gameSpeedShort.Count;
                _newSpeed = num % gameSpeedShort.Count;
                _newGameSpeed = gameSpeedShort[__instance.CurrentGameSpeed];
            }
            else
            {
                __instance.CurrentGameSpeed = num % gameSpeed.Count;
                _newSpeed = num % gameSpeed.Count;
                _newGameSpeed = gameSpeed[__instance.CurrentGameSpeed];
            }

            GameManager.SetTimeScale(_newGameSpeed);
            NotificationCentre.Instance.PlayGenericNotification(Math.Abs(_newGameSpeed - 1) < 0.001f
                ? $"Returned game speed to {_newGameSpeed} (default)"
                : $"Decreased game speed to {_newGameSpeed}");
        }

        if (_newGameSpeed <= 0 || _newSpeed <= 0) return;

        GameManager.SetTimeScale(_newGameSpeed);
        __instance.CurrentGameSpeed = _newSpeed;
    }
}