// using COTL_API.CustomSettings;
//
// namespace CultOfQoL;
//
// public static class SoftDepend
// {
//     private static bool? _enabled;
//     private const string Version = "0.2.3";
//     private const string CotlAPI = "COTL_API";
//
//     public static bool Enabled
//     {
//         get
//         {
//             if (_enabled != null) return (bool) _enabled;
//             var plugin = BepInEx.Bootstrap.Chainloader.PluginInfos.FirstOrDefault(a => a.Value.Metadata.GUID.Contains(CotlAPI)).Value;
//             if (plugin.Metadata.Version < new Version(Version))
//             {
//                 Plugin.Log.LogWarning($"Please update COTL_API to version {Version} or higher.");
//                 _enabled = false;
//                 return false;
//             }
//             _enabled = plugin.Instance != null;
//
//             return (bool) _enabled;
//         }
//     }
//
//     public static void AddSettingsMenus()
//     {
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Player", Plugin.EnableBaseDamageMultiplier.Definition.Key, Plugin.EnableBaseDamageMultiplier);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Player", Plugin.BaseDamageMultiplier.Definition.Key, Plugin.BaseDamageMultiplier, 1, MMSlider.ValueDisplayFormat.RawValue);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Player", Plugin.EnableRunSpeedMulti.Definition.Key, Plugin.EnableRunSpeedMulti);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Player", Plugin.RunSpeedMulti.Definition.Key, Plugin.RunSpeedMulti, 1, MMSlider.ValueDisplayFormat.RawValue);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Player", Plugin.EnableDodgeSpeedMulti.Definition.Key, Plugin.EnableDodgeSpeedMulti);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Player", Plugin.DodgeSpeedMulti.Definition.Key, Plugin.DodgeSpeedMulti, 1, MMSlider.ValueDisplayFormat.RawValue);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Player", Plugin.EnableLungeSpeedMulti.Definition.Key, Plugin.EnableLungeSpeedMulti);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Player", Plugin.LungeSpeedMulti.Definition.Key, Plugin.LungeSpeedMulti, 1, MMSlider.ValueDisplayFormat.RawValue);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - General", "Skip Intros", Plugin.SkipDevIntros);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - General", "Skip Crown Video", Plugin.SkipCrownVideo);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - General", "Remove Extra Menu Buttons", Plugin.RemoveMenuClutter);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - General", "Remove Twitch Buttons", Plugin.RemoveTwitchButton);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - General", "Unlock Twitch Stuff", Plugin.UnlockTwitchItems);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - General", "Remove Ad", Plugin.DisableAds);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - General", "Remove Help Button In Pause Menu", Plugin.RemoveHelpButtonInPauseMenu);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - General", "Remove Twitch Button In Pause Menu", Plugin.RemoveTwitchButtonInPauseMenu);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - General", "Remove Photo Button In Pause Menu", Plugin.RemovePhotoModeButtonInPauseMenu);
//
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Save", "Direct Load Save", Plugin.DirectLoadSave);
//         CustomSettingsManager.AddDropdown("Cult of QoL - Save", "Save To Load", Plugin.SaveSlotToLoad.Value.ToString(), new[] {"1", "2", "3"}, delegate(int i)
//         {
//             Plugin.SaveSlotToLoad.Value = i + 1;
//         });
//
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Save", "Save On Quit To Menu", Plugin.SaveOnQuitToMenu);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Save", "Save On Quit To Desktop", Plugin.SaveOnQuitToDesktop);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Save", "Hide New Game Button(s)", Plugin.HideNewGameButtons);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Save", "Enable Quick Save Shortcut", Plugin.EnableQuickSaveShortcut);
//         CustomSettingsManager.AddDropdown("Cult of QoL - Save", "Quick Save Key", Plugin.SaveKeyboardShortcut.Value.MainKey.ToString(), Enum.GetNames(typeof(KeyCode)),
//             delegate(int i)
//             {
//                 var keyCodes = Enum.GetValues(typeof(KeyCode));
//                 Plugin.SaveKeyboardShortcut.Value = new KeyboardShortcut((KeyCode) keyCodes.GetValue(i));
//             });
//
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Scale", "Enable Custom UI Scale", Plugin.EnableCustomUiScale);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Scale", "Custom UI Scale Value", Plugin.CustomUiScale, 1,
//             MMSlider.ValueDisplayFormat.RawValue);
//
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Weather", "Change Weather During The Day", Plugin.ChangeWeatherOnPhaseChange);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Weather", "Randomize Weather On Exit Area", Plugin.RandomWeatherChangeWhenExitingArea);
//
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Golden Fleece", "Reverse Golden Fleece Change", Plugin.ReverseGoldenFleeceDamageChange);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Golden Fleece", "Increase Golden Fleece Rate", Plugin.IncreaseGoldenFleeceDamageRate);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Golden Fleece", "Use Custom Damage Value", Plugin.UseCustomDamageValue);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Golden Fleece", "Custom Damage Multiplier", Plugin.CustomDamageMulti, 1, MMSlider.ValueDisplayFormat.RawValue);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Game Mechanics", "Mass Collecting", Plugin.MassCollecting);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Game Mechanics", "Adjust Refinery Requirements", Plugin.AdjustRefineryRequirements);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Game Mechanics", "Disable Fishing Mini-Game", Plugin.EasyFishing);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Game Mechanics", "No More Game-Over", Plugin.DisableGameOver);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Game Mechanics", "3x Tarot Luck", Plugin.ThriceMultiplyTarotCardLuck);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Lumber/Mine Mods", "Infinite Lumber & Mining Stations",
//             Plugin.LumberAndMiningStationsDontAge);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Lumber/Mine Mods", "Double Life Span Instead",
//             Plugin.DoubleLifespanInstead);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Lumber/Mine Mods", "Add 50% to Life Span Instead",
//             Plugin.FiftyPercentIncreaseToLifespanInstead);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Propaganda", Plugin.TurnOffSpeakersAtNight.Definition.Key, Plugin.TurnOffSpeakersAtNight);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Propaganda", Plugin.DisablePropagandaSpeakerAudio.Definition.Key, Plugin.DisablePropagandaSpeakerAudio);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Speed", Plugin.EnableGameSpeedManipulation.Definition.Key, Plugin.EnableGameSpeedManipulation);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Speed", Plugin.ShortenGameSpeedIncrements.Definition.Key, Plugin.ShortenGameSpeedIncrements);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Speed", Plugin.FastCollecting.Definition.Key, Plugin.FastCollecting);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Speed", Plugin.SlowDownTime.Definition.Key, Plugin.SlowDownTime);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Speed", Plugin.SlowDownTimeMultiplier.Definition.Key, Plugin.SlowDownTimeMultiplier, 1, MMSlider.ValueDisplayFormat.RawValue);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Chests", Plugin.EnableAutoInteract.Definition.Key, Plugin.EnableAutoInteract);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Chests", Plugin.TriggerAmount.Definition.Key, Plugin.TriggerAmount, 1, MMSlider.ValueDisplayFormat.RawValue);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Chests", Plugin.IncreaseRange.Definition.Key, Plugin.IncreaseRange);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Chests", Plugin.UseCustomRange.Definition.Key, Plugin.UseCustomRange);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Chests", Plugin.CustomRangeMulti.Definition.Key, Plugin.CustomRangeMulti, 1, MMSlider.ValueDisplayFormat.RawValue);
//         // CustomSettingsManager.AddBepInExConfig("Cult of QoL - Capacity", Plugin.JustRightSiloCapacity.Definition.Key, Plugin.JustRightSiloCapacity);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Capacity", Plugin.UseCustomSiloCapacity.Definition.Key, Plugin.UseCustomSiloCapacity);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Capacity", Plugin.CustomSiloCapacityMulti.Definition.Key, Plugin.CustomSiloCapacityMulti, 1, MMSlider.ValueDisplayFormat.RawValue);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Capacity", Plugin.DoubleSoulCapacity.Definition.Key, Plugin.DoubleSoulCapacity);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Capacity", Plugin.UseCustomSoulCapacity.Definition.Key, Plugin.UseCustomSoulCapacity);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Capacity", Plugin.CustomSoulCapacityMulti.Definition.Key, Plugin.CustomSoulCapacityMulti, 1, MMSlider.ValueDisplayFormat.RawValue);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Notifications", Plugin.NotifyOfScarecrowTraps.Definition.Key, Plugin.NotifyOfScarecrowTraps);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Notifications", Plugin.NotifyOfNoFuel.Definition.Key, Plugin.NotifyOfNoFuel);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Notifications", Plugin.NotifyOfBedCollapse.Definition.Key, Plugin.NotifyOfBedCollapse);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Notifications", Plugin.ShowPhaseNotifications.Definition.Key, Plugin.ShowPhaseNotifications);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Notifications", Plugin.ShowWeatherChangeNotifications.Definition.Key, Plugin.ShowWeatherChangeNotifications);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Followers", Plugin.GiveFollowersNewNecklaces.Definition.Key, Plugin.GiveFollowersNewNecklaces);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Followers", Plugin.CleanseIllnessAndExhaustionOnLevelUp.Definition.Key, Plugin.CleanseIllnessAndExhaustionOnLevelUp);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Followers", Plugin.CollectTitheFromOldFollowers.Definition.Key, Plugin.CollectTitheFromOldFollowers);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Followers", Plugin.AddExhaustedToHealingBay.Definition.Key, Plugin.AddExhaustedToHealingBay);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Followers", Plugin.BulkFollowerCommands.Definition.Key, Plugin.BulkFollowerCommands);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Followers", Plugin.OnlyShowDissenters.Definition.Key, Plugin.OnlyShowDissenters);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Followers", Plugin.MassLevelUp.Definition.Key, Plugin.MassLevelUp);
//         CustomSettingsManager.AddBepInExConfig("Cult of QoL - Followers", Plugin.RemoveLevelLimit.Definition.Key, Plugin.RemoveLevelLimit);
//     }
// }