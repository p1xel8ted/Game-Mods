namespace CultOfQoL.Patches.Systems;

[Harmony]
public static class GameSpeedManipulationPatches
{
    private static float _newGameSpeed;
    private static int _newSpeed;
    private static bool _timeMessageShown;
    private static bool _hasInitialized;

    private static readonly List<float> GameSpeedShort = [0.25f, 1, 2, 3, 4, 5];

    private static readonly List<float> GameSpeed =
    [
        0.25f, 0.5f, 0.75f, 1, 1.25f, 1.5f, 1.75f, 2,
        2.25f, 2.5f, 2.75f, 3, 3.25f, 3.5f, 3.75f, 4,
        4.25f, 4.5f, 4.75f, 5
    ];

    [HarmonyPrefix]
    [HarmonyPatch(typeof(TimeManager), nameof(TimeManager.Simulate), typeof(float), typeof(bool))]
    public static void TimeManager_Simulate(ref float deltaGameTime, ref bool skippingTime)
    {
        if (Mathf.Approximately(Plugin.SlowDownTimeMultiplier.Value, 1.0f) || skippingTime) return;

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

            var speedList = Plugin.ShortenGameSpeedIncrements.Value ? GameSpeedShort : GameSpeed;
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
        if (!__instance) return;
        if (!Plugin.EnableGameSpeedManipulation.Value) return;

        if (!_hasInitialized)
        {
            // First time - reset to default speed
            ResetTime();
            _hasInitialized = true;
        }
        else
        {
            // Scene change - restore current speed setting
            GameManager.SetTimeScale(_newGameSpeed);
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameManager), nameof(GameManager.Update))]
    public static void GameManager_Update(GameManager __instance)
    {
        if (!__instance) return;

#if DEBUG
        HandleDebugKeys();
#endif

        if (!Mathf.Approximately(Plugin.SlowDownTimeMultiplier.Value, 1.0f) && !_timeMessageShown)
        {
            _timeMessageShown = true;
            NotificationCentre.Instance.PlayGenericNotification($"Slow down time enabled at {Mathf.Abs(Plugin.SlowDownTimeMultiplier.Value)}x.");
        }

        if (!Plugin.EnableGameSpeedManipulation.Value) return;

        if (Plugin.ResetTimeScaleKey.Value.IsUp())
        {
            ResetTime();
            NotificationCentre.Instance.PlayGenericNotification("Returned game speed to 1 (default)");
            return;  
        }
        
        var speedList = Plugin.ShortenGameSpeedIncrements.Value ? GameSpeedShort : GameSpeed;
        var speedCount = speedList.Count;

        if (Plugin.IncreaseGameSpeedKey.Value.IsUp())
        {
            UpdateGameSpeed(__instance.CurrentGameSpeed + 1);
            return;
        }

        if (Plugin.DecreaseGameSpeedKey.Value.IsUp())
        {
            UpdateGameSpeed(__instance.CurrentGameSpeed - 1);
            return;
        }

        // Only reset if we haven't initialized yet (prevents unwanted resets during gameplay)
        if (!_hasInitialized && (_newGameSpeed <= 0 || _newSpeed < 0))
        {
            ResetTime();
            _hasInitialized = true;
        }

        __instance.CurrentGameSpeed = _newSpeed;
        return;

        void UpdateGameSpeed(int newSpeedIndex)
        {
            // Clamp instead of wrap - don't loop around at min/max
            _newSpeed = Mathf.Clamp(newSpeedIndex, 0, speedCount - 1);
            _newGameSpeed = speedList[_newSpeed];
            GameManager.SetTimeScale(_newGameSpeed);
            NotificationCentre.Instance.PlayGenericNotification(
                Math.Abs(_newGameSpeed - 1) < 0.001f
                    ? $"Returned game speed to {_newGameSpeed} (default)"
                    : $"Adjusted game speed to {_newGameSpeed}");
        }
    }

#if DEBUG
    private static void HandleDebugKeys()
    {
        // F8: Unlock all structures, rituals, and upgrades
        if (Input.GetKeyDown(KeyCode.F8))
        {
            // Unlock all structures
            foreach (StructureBrain.TYPES type in Enum.GetValues(typeof(StructureBrain.TYPES)))
            {
                if (!StructuresData.GetUnlocked(type))
                {
                    DataManager.Instance.UnlockedStructures.Add(type);
                }
            }

            // Unlock all rituals
            CheatConsole.UnlockAllRituals = true;

            // Unlock all upgrade abilities
            foreach (UpgradeSystem.Type type in Enum.GetValues(typeof(UpgradeSystem.Type)))
            {
                UpgradeSystem.UnlockAbility(type);
            }

            NotificationCentre.Instance.PlayGenericNotification("[DEBUG] Unlocked all structures, rituals, and upgrades.");
            Plugin.Log.LogInfo("[DEBUG] Unlocked all structures, rituals, and upgrades.");
        }

        // F9: Set all followers to max adoration (ready for level up)
        if (Input.GetKeyDown(KeyCode.F9))
        {
            var count = 0;
            foreach (var follower in Helpers.AllFollowers)
            {
                if (follower.Brain.Stats.Adoration < follower.Brain.Stats.MAX_ADORATION)
                {
                    follower.Brain.Stats.Adoration = follower.Brain.Stats.MAX_ADORATION;
                    follower.AdorationUI.BarController.SetBarSize(1f, false);
                    count++;
                }
            }

            NotificationCentre.Instance.PlayGenericNotification($"[DEBUG] Set {count} followers to max adoration.");
            Plugin.Log.LogInfo($"[DEBUG] Set {count} followers to max adoration.");
        }

        // F10: Fill all outhouses with poop
        if (Input.GetKeyDown(KeyCode.F10))
        {
            var count = 0;
            foreach (var interaction in Interaction.interactions.ToList())
            {
                if (interaction is Interaction_Outhouse outhouse && outhouse && outhouse.StructureBrain is { } brain)
                {
                    var capacity = Structures_Outhouse.Capacity(brain.Data.Type);
                    var currentPoop = brain.GetPoopCount();
                    var toAdd = capacity - currentPoop;

                    if (toAdd > 0)
                    {
                        // Add poop to inventory (item type 39 = POOP)
                        brain.Data.Inventory.Add(new InventoryItem { type = 39, quantity = toAdd });
                        count++;
                    }
                }
            }

            NotificationCentre.Instance.PlayGenericNotification($"[DEBUG] Filled {count} outhouses with poop.");
            Plugin.Log.LogInfo($"[DEBUG] Filled {count} outhouses with poop.");
        }

        // F11: Fill all scarecrows with birds
        if (Input.GetKeyDown(KeyCode.F11))
        {
            var count = 0;
            foreach (var scarecrow in Scarecrow.Scarecrows.ToList())
            {
                if (scarecrow == null || scarecrow.Brain == null || scarecrow.Brain.HasBird)
                {
                    continue;
                }

                if (scarecrow.TrapOpen == null || scarecrow.TrapShut == null)
                {
                    continue;
                }

                scarecrow.Brain.HasBird = true;
                scarecrow.ShutTrap();
                count++;
            }

            NotificationCentre.Instance.PlayGenericNotification($"[DEBUG] Filled {count} scarecrows with birds.");
            Plugin.Log.LogInfo($"[DEBUG] Filled {count} scarecrows with birds.");
        }

        // F12: Trigger all wolf traps (catch wolves instantly)
        if (Input.GetKeyDown(KeyCode.F12))
        {
            var count = 0;
            foreach (var trap in Interaction_WolfTrap.Traps.ToList())
            {
                if (trap == null || trap.structure?.Brain?.Data == null)
                {
                    continue;
                }

                // Skip if already caught something
                if (trap.structure.Brain.Data.HasBird)
                {
                    continue;
                }

                // Trigger the catch (HasBird is reused for wolf caught)
                trap.structure.Brain.Data.HasBird = true;
                trap.structure.Brain.Data.Inventory.Clear();
                trap.UpdateVisuals();
                count++;
            }

            NotificationCentre.Instance.PlayGenericNotification($"[DEBUG] Triggered {count} wolf traps.");
            Plugin.Log.LogInfo($"[DEBUG] Triggered {count} wolf traps.");
        }
    }
#endif
}