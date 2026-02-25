using System.Diagnostics.CodeAnalysis;
using UnityEngine.Serialization;

namespace TraitControl.Patches;

[Harmony]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class NoNegativeTraits
{
    private static HashSet<FollowerTrait.TraitType> _allTraits;
    private static List<FollowerTraitBackup> _followerTraitBackups = [];

    /// <summary>
    /// True while loading followers from a save file. Set in BiomeBaseManager_Start,
    /// cleared after one frame. Used to gate the FollowerBrain constructor postfix
    /// so existing followers respect the ApplyToExistingFollowers setting.
    /// </summary>
    private static bool _isLoadingSave;

    private static string DataPath => Path.Combine(Application.persistentDataPath, "saves", $"slot_{SaveAndLoad.SAVE_SLOT}_follower_trait_backups.json");

    private static void LoadBackupFromFile()
    {
        if (Plugin.IsNothingNegativePresent()) return;
        if (!File.Exists(DataPath)) return;
        var json = File.ReadAllText(DataPath);
        _followerTraitBackups = JsonConvert.DeserializeObject<List<FollowerTraitBackup>>(json);
        Plugin.L($"Loaded {_followerTraitBackups.Count} follower trait backups.");
    }

    private static HashSet<FollowerTrait.TraitType> InitializeAllTraits()
    {
        var allTraits = new HashSet<FollowerTrait.TraitType>();
        allTraits.UnionWith(FollowerTrait.SingleTraits);
        allTraits.UnionWith(FollowerTrait.GoodTraits);
        allTraits.UnionWith(FollowerTrait.SinTraits);
        allTraits.UnionWith(FollowerTrait.RareStartingTraits);
        allTraits.UnionWith(FollowerTrait.StartingTraits);
        allTraits.UnionWith(FollowerTrait.ExcludedFromMating);

        allTraits.RemoveWhere(a => !FollowerTrait.IsPositiveTrait(a));

        // Filter by game unlock requirements if enabled
        if (Plugin.UseUnlockedTraitsOnly.Value)
        {
            Plugin.L("Using unlocked traits only - filtering by game unlock requirements.");
            allTraits.RemoveWhere(FollowerTrait.IsTraitUnavailable);
        }

        allTraits.RemoveWhere(a => a == FollowerTrait.TraitType.BishopOfCult);

        if (!Plugin.IncludeImmortal.Value)
        {
            Plugin.L("Removing Immortal trait from available traits.");
            allTraits.Remove(FollowerTrait.TraitType.Immortal);
        }

        if (!Plugin.IncludeDisciple.Value)
        {
            Plugin.L("Removing Disciple trait from available traits.");
            allTraits.Remove(FollowerTrait.TraitType.Disciple);
        }

        if (!Plugin.IncludeDontStarve.Value)
        {
            Plugin.L("Removing DontStarve trait from available traits.");
            allTraits.Remove(FollowerTrait.TraitType.DontStarve);
        }

        if (!Plugin.IncludeBlind.Value)
        {
            Plugin.L("Removing Blind trait from available traits.");
            allTraits.Remove(FollowerTrait.TraitType.Blind);
        }

        if (!Plugin.IncludeBornToTheRot.Value)
        {
            Plugin.L("Removing BornToTheRot trait from available traits.");
            allTraits.Remove(FollowerTrait.TraitType.BornToTheRot);
        }

        // Remove event traits from replacement pool unless config allows them
        if (!Plugin.IncludeStoryEventTraits.Value)
        {
            allTraits.RemoveWhere(a => Plugin.StoryEventTraits.Contains(a));
        }
        else
        {
            // Even with event traits enabled, only include positive ones for replacement
            allTraits.RemoveWhere(a => Plugin.StoryEventTraits.Contains(a) && !FollowerTrait.IsPositiveTrait(a));
        }

        Plugin.L("All 'positive' traits currently available based on your configuration:");
        foreach (var trait in allTraits)
        {
            Plugin.L($"\t{trait.ToString()}");
        }
        return allTraits;
    }

    internal static void UpdateAllFollowerTraits()
    {
        if (Plugin.IsNothingNegativePresent()) return;
        foreach (var follower in FollowerManager.Followers.Values.SelectMany(list => list))
        {
            ProcessTraitReplacement(follower.Brain);
        }
    }

    internal static void RestoreOriginalTraits()
    {
        if (Plugin.IsNothingNegativePresent() || _followerTraitBackups == null) return;

        foreach (var backup in _followerTraitBackups)
        {
            var follower = FollowerManager.FindFollowerByID(backup.id);
            if (follower == null || follower.Brain == null)
            {
                Plugin.L($"Could not find follower with ID {backup.id} to restore traits. Is the follower dead?");
                continue;
            }

            if (backup.traits == null)
            {
                Plugin.L($"No traits to restore for {follower.Brain._directInfoAccess.Name}");
                continue;
            }

            RestoreFollowerTraits(follower, backup.traits);
        }
    }

    private static void ClearAllTraits(Follower follower)
    {
        if (Plugin.IsNothingNegativePresent()) return;
        foreach (var trait in follower.Brain._directInfoAccess.Traits.ToList())
        {
            follower.Brain.RemoveTrait(trait, ShouldShowRemoveNotification(trait));
        }
    }

    private static void RestoreFollowerTraits(Follower follower, List<string> backupTraits)
    {
        Plugin.L($"Restoring traits for {follower.Brain._directInfoAccess.Name}");

        ClearAllTraits(follower);

        // Add backed up traits
        foreach (var trait in backupTraits)
        {
            if (Enum.TryParse(trait, out FollowerTrait.TraitType traitType))
            {
                follower.Brain.AddTrait(traitType, ShouldShowAddNotification(traitType));
            }
            else
            {
                Plugin.L($"Could not parse trait {trait} for follower {follower.Brain._directInfoAccess.Name}");
            }
        }
    }

    private static void SaveBackupToFile(bool log = true)
    {
        if (Plugin.IsNothingNegativePresent()) return;
        try
        {
            if (_followerTraitBackups == null || !_followerTraitBackups.Any())
            {
                if (log)
                {
                    Plugin.L("No follower trait backups to save.");
                }
                return;
            }

            var json = JsonConvert.SerializeObject(_followerTraitBackups, Formatting.Indented);
            File.WriteAllText(DataPath, json);
            if (log)
            {
                Plugin.L($"Saved {_followerTraitBackups.Count} follower trait backups to {DataPath}");
            }
        }
        catch (Exception ex)
        {
            if (log)
            {
                Plugin.L($"Error saving follower trait backups: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Processes trait replacement for a follower brain.
    /// </summary>
    /// <param name="brain">The follower brain to process.</param>
    /// <param name="directManipulation">
    /// When true, directly manipulates the traits list without calling RemoveTrait/AddTrait.
    /// This avoids NullReferenceExceptions during early game loading when singletons like
    /// TwitchFollowers or NotificationCentre may not be initialized.
    /// </param>
    internal static void ProcessTraitReplacement(FollowerBrain brain, bool directManipulation = false)
    {
        CreateTraitBackup(brain);

        var traits = brain._directInfoAccess.Traits;

        Plugin.L($"Processing traits for {brain._directInfoAccess.Name}");

        foreach (var trait in traits.ToList())
        {
            if (FollowerTrait.IsPositiveTrait(trait))
            {
                Plugin.L($"\tSkipping positive trait {trait}");
                continue;
            }

            // Skip Mutated (Rot) and Zombie (Cursed) traits if preservation is enabled
            // Mutated: mechanically distinct rot state, useful for rituals
            // Zombie: granted by resurrection ritual, changes animation and behavior
            if (Plugin.PreserveMutatedTrait.Value && (trait == FollowerTrait.TraitType.Mutated || trait == FollowerTrait.TraitType.Zombie))
            {
                Plugin.L($"\tSkipping {trait} trait - special state trait preservation enabled");
                continue;
            }

            // Only use exclusive counterpart if it's actually positive (e.g., Lazy→Industrious)
            // Skip if counterpart is also negative (e.g., Hibernation→Aestivation are both negative)
            // Skip if counterpart is an event trait (e.g., OverworkedParent→ProudParent requires having kids)
            if (Plugin.PreferExclusiveCounterparts.Value && IsExclusiveTrait(trait) && TryReplaceExclusiveTrait(trait, out var replacement) && FollowerTrait.IsPositiveTrait(replacement) && !Plugin.StoryEventTraits.Contains(replacement))
            {
                if (directManipulation)
                {
                    traits.Remove(trait);
                    if (!traits.Contains(replacement))
                    {
                        traits.Add(replacement);
                    }
                }
                else
                {
                    brain.RemoveTrait(trait, ShouldShowRemoveNotification(trait));
                    brain.AddTrait(replacement, ShouldShowAddNotification(replacement));
                }
                Plugin.L($"\tReplacing negative exclusive trait {trait} with exclusive {replacement}");
                continue;
            }

            Plugin.L($"\tRemoving negative trait {trait}");
            if (directManipulation)
            {
                traits.Remove(trait);
            }
            else
            {
                brain.RemoveTrait(trait, ShouldShowRemoveNotification(trait));
            }

            var newTrait = FindPositiveReplacement(brain);
            if (newTrait != FollowerTrait.TraitType.None)
            {
                if (directManipulation)
                {
                    if (!traits.Contains(newTrait))
                    {
                        traits.Add(newTrait);
                    }
                }
                else
                {
                    brain.AddTrait(newTrait, ShouldShowAddNotification(newTrait));
                }
                Plugin.L($"\tAdded replacement positive trait {newTrait}");
            }
            else
            {
                Plugin.L($"\tNo replacement trait available for {trait}");
            }
        }
    }

    private static void CreateTraitBackup(FollowerBrain brain)
    {
        var stringTraits = brain._directInfoAccess.Traits.Select(trait => trait.ToString()).ToList();
        var traitBackup = new FollowerTraitBackup
        {
            traits = stringTraits,
            id = brain._directInfoAccess.ID,
            name = brain._directInfoAccess.Name
        };

        var alreadyBackedUp = _followerTraitBackups?.Any(b => b.id == traitBackup.id && b.name == traitBackup.name) ?? false;
        if (alreadyBackedUp) return;
        Plugin.L($"Backing up original traits for {brain._directInfoAccess.Name}");
        _followerTraitBackups?.Add(traitBackup);
        SaveBackupToFile();
    }

    private static bool TryReplaceExclusiveTrait(FollowerTrait.TraitType trait, out FollowerTrait.TraitType replacement)
    {
        if (FollowerTrait.ExclusiveTraits.TryGetValue(trait, out replacement)) return true;

        var pair = FollowerTrait.ExclusiveTraits.FirstOrDefault(x => x.Value == trait);
        if (pair.Key == default) return false;
        replacement = pair.Key;
        return true;
    }

    private static FollowerTrait.TraitType FindPositiveReplacement(FollowerBrain brain)
    {
        if (_allTraits == null || _allTraits.Count == 0)
        {
            GenerateAvailableTraits();
        }

        if (_allTraits == null || _allTraits.Count == 0)
        {
            Plugin.L("Warning: No positive traits available for replacement.");
            return FollowerTrait.TraitType.None;
        }

        var availableTraits = _allTraits.Where(t => !brain.HasTrait(t)).ToList();
        if (availableTraits.Count == 0)
        {
            Plugin.L("Warning: Follower already has all available positive traits.");
            return FollowerTrait.TraitType.None;
        }

        return availableTraits[Random.Range(0, availableTraits.Count)];
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(FollowerBrain), MethodType.Constructor, typeof(FollowerInfo))]
    private static void FollowerBrain_Constructor(ref FollowerBrain __instance)
    {
        if (Plugin.IsNothingNegativePresent()) return;
        if (!Plugin.NoNegativeTraits.Value) return;

        // During save load, respect ApplyToExistingFollowers setting.
        // New followers created during gameplay are always processed.
        if (_isLoadingSave && !Plugin.ApplyToExistingFollowers.Value)
        {
            Plugin.L($"Skipping trait replacement for {__instance._directInfoAccess.Name} - loading save with 'Apply To Existing' disabled");
            return;
        }

        // Use directManipulation to avoid NullReferenceException during early game loading
        // when singletons like TwitchFollowers or NotificationCentre may not be initialized.
        ProcessTraitReplacement(__instance, directManipulation: true);
    }

    private static bool IsExclusiveTrait(FollowerTrait.TraitType trait)
    {
        return FollowerTrait.ExclusiveTraits.ContainsKey(trait) || FollowerTrait.ExclusiveTraits.ContainsValue(trait);
    }

    /// <summary>
    /// Checks if a trait has a valid localization entry.
    /// Returns false if the localized title is empty, null, or the raw key format.
    /// </summary>
    private static bool HasValidLocalization(FollowerTrait.TraitType trait)
    {
        var title = FollowerTrait.GetLocalizedTitle(trait);
        // I2 Localization returns the key itself if no translation exists
        return !string.IsNullOrWhiteSpace(title) && !title.StartsWith("Traits/");
    }

    /// <summary>
    /// Determines if a notification should be shown for adding a trait.
    /// </summary>
    private static bool ShouldShowAddNotification(FollowerTrait.TraitType trait)
    {
        return Plugin.ShowNotificationsWhenAddingTraits.Value && HasValidLocalization(trait);
    }

    /// <summary>
    /// Determines if a notification should be shown for removing a trait.
    /// </summary>
    private static bool ShouldShowRemoveNotification(FollowerTrait.TraitType trait)
    {
        return Plugin.ShowNotificationsWhenRemovingTraits.Value && HasValidLocalization(trait);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(BiomeBaseManager), nameof(BiomeBaseManager.Start))]
    private static void BiomeBaseManager_Start()
    {
        if (Plugin.IsNothingNegativePresent()) return;

        // Mark that we're loading from save — FollowerBrain constructors during this frame
        // should respect ApplyToExistingFollowers instead of always processing
        _isLoadingSave = true;
        Plugin._instance?.StartCoroutine(ClearLoadFlag());

        _followerTraitBackups?.Clear();
        LoadBackupFromFile();
        GenerateAvailableTraits();
    }

    /// <summary>
    /// Clears the save-load flag after one frame, once all existing followers have been constructed.
    /// After this, any new FollowerBrain constructions are for genuinely new followers.
    /// </summary>
    private static IEnumerator ClearLoadFlag()
    {
        yield return null;
        _isLoadingSave = false;
        Plugin.L("Save load complete - new followers will have trait replacement applied normally");
    }

    internal static void GenerateAvailableTraits()
    {
        _allTraits = InitializeAllTraits();
    }

    [Serializable]
    public class FollowerTraitBackup
    {
        [FormerlySerializedAs("ID")] public int id;
        [FormerlySerializedAs("Name")] public string name;
        [FormerlySerializedAs("Traits")] public List<string> traits;
    }
}
