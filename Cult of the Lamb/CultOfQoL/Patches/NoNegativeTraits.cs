namespace CultOfQoL.Patches;

[Harmony]
public static class NoNegativeTraits
{

    private static HashSet<FollowerTrait.TraitType> AllTraits;
    private static bool? isNothingNegativePresentCache;
    private static List<FollowerTraitBackup> FollowerTraitBackups = [];

    private static string DataPath => Path.Combine(Application.persistentDataPath, "saves", $"slot_{SaveAndLoad.SAVE_SLOT}_follower_trait_backups.json");

    private static void LoadBackupFromFile()
    {
        if (IsNothingNegativePresent()) return;
        if (!File.Exists(DataPath)) return;
        var json = File.ReadAllText(DataPath);
        FollowerTraitBackups = JsonConvert.DeserializeObject<List<FollowerTraitBackup>>(json);
        Plugin.L($"Loaded {FollowerTraitBackups.Count} follower trait backups.");
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

        var cultTraits = DataManager.Instance.CultTraits;
        if (Plugin.UseUnlockedTraitsOnly.Value)
        {
            Plugin.L("Using unlocked traits only for available traits.");
            allTraits.RemoveWhere(a => cultTraits.Contains(a));
        }
        allTraits.RemoveWhere(a => !FollowerTrait.IsPositiveTrait(a));

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

        Plugin.L("All 'positive' traits currently available based on your configuration:");
        foreach (var trait in allTraits)
        {
            Plugin.L($"\t{trait.ToString()}");
        }
        return allTraits;
    }

    internal static void UpdateAllFollowerTraits()
    {
        if (IsNothingNegativePresent()) return;
        foreach (var follower in Helpers.AllFollowers)
        {
            ProcessTraitReplacement(follower.Brain);
        }
    }


    internal static void RestoreOriginalTraits()
    {
        if (IsNothingNegativePresent() || FollowerTraitBackups == null) return;

        foreach (var backup in FollowerTraitBackups)
        {
            var follower = FollowerManager.FindFollowerByID(backup.ID);
            if (follower == null || follower.Brain == null)
            {
                Plugin.L($"Could not find follower with ID {backup.ID} to restore traits. Is the follower dead?");
                continue;
            }

            if (backup.Traits == null)
            {
                Plugin.L($"No traits to restore for {follower.Brain._directInfoAccess.Name}");
                continue;
            }

            RestoreFollowerTraits(follower, backup.Traits);
        }
    }


    private static void ClearAllTraits(Follower follower)
    {
        if (IsNothingNegativePresent()) return;
        foreach (var trait in follower.Brain._directInfoAccess.Traits.ToList())
        {
            follower.Brain.RemoveTrait(trait, Plugin.ShowNotificationsWhenRemovingTraits.Value);
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
                follower.Brain.AddTrait(traitType, Plugin.ShowNotificationsWhenAddingTraits.Value);
            }
            else
            {
                Plugin.L($"Could not parse trait {trait} for follower {follower.Brain._directInfoAccess.Name}");
            }
        }
    }


    private static void SaveBackupToFile(bool log = true)
    {
        if (IsNothingNegativePresent()) return;
        try
        {
            if (FollowerTraitBackups == null || !FollowerTraitBackups.Any())
            {
                if (log)
                {
                    Plugin.L("No follower trait backups to save.");
                }
                return;
            }

            var json = JsonConvert.SerializeObject(FollowerTraitBackups, Formatting.Indented);
            File.WriteAllText(DataPath, json);
            if (log)
            {
                Plugin.L($"Saved {FollowerTraitBackups.Count} follower trait backups to {DataPath}");
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

    internal static bool IsNothingNegativePresent()
    {
        isNothingNegativePresentCache ??= BepInEx.Bootstrap.Chainloader.PluginInfos.Any(plugin => plugin.Value.Instance.Info.Metadata.GUID.Equals("NothingNegative", StringComparison.OrdinalIgnoreCase));
        return isNothingNegativePresentCache.Value;
    }

    private static void ProcessTraitReplacement(FollowerBrain brain)
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

            if (!Plugin.UseUnlockedTraitsOnly.Value && IsExclusiveTrait(trait) && TryReplaceExclusiveTrait(trait, out var replacement))
            {
                // traits[traits.IndexOf(trait)] = replacement;
                brain.RemoveTrait(trait, Plugin.ShowNotificationsWhenRemovingTraits.Value);
                brain.AddTrait(replacement, Plugin.ShowNotificationsWhenAddingTraits.Value);
                Plugin.L($"\tReplacing negative exclusive trait {trait} with exclusive {replacement}");
                continue;
            }

            Plugin.L($"\tRemoving negative trait {trait}");
            brain.RemoveTrait(trait, Plugin.ShowNotificationsWhenRemovingTraits.Value);

            var newTrait = FindPositiveReplacement(brain);
            brain.AddTrait(newTrait, Plugin.ShowNotificationsWhenAddingTraits.Value);

            Plugin.L($"\tAdded replacement positive trait {newTrait}");
        }
    }

    private static void CreateTraitBackup(FollowerBrain brain)
    {
        var stringTraits = brain._directInfoAccess.Traits.Select(trait => trait.ToString()).ToList();
        var traitBackup = new FollowerTraitBackup
        {
            Traits = stringTraits,
            ID = brain._directInfoAccess.ID,
            Name = brain._directInfoAccess.Name
        };

        var alreadyBackedUp = FollowerTraitBackups?.Any(b => b.ID == traitBackup.ID && b.Name == traitBackup.Name) ?? false;
        if (alreadyBackedUp) return;
        Plugin.L($"Backing up original traits for {brain._directInfoAccess.Name}");
        FollowerTraitBackups?.Add(traitBackup);
        SaveBackupToFile();
    }

    private static bool TryReplaceExclusiveTrait(FollowerTrait.TraitType trait, out FollowerTrait.TraitType replacement)
    {
        if (FollowerTrait.ExclusiveTraits.TryGetValue(trait, out replacement)) return true;

        var pair = FollowerTrait.ExclusiveTraits.FirstOrDefault(x => x.Value == trait);
        if (pair.Equals(default(KeyValuePair<FollowerTrait.TraitType, FollowerTrait.TraitType>))) return false;
        replacement = pair.Key;
        return true;
    }

    private static FollowerTrait.TraitType FindPositiveReplacement(FollowerBrain brain)
    {
        if (AllTraits == null)
        {
            GenerateAvailableTraits();
        }
        
        FollowerTrait.TraitType newTrait;
        do
        {
            newTrait = AllTraits!.ElementAt(Random.Range(0, AllTraits!.Count));
        } while (!FollowerTrait.IsPositiveTrait(newTrait) || brain.HasTrait(newTrait));

        return newTrait;
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(FollowerBrain), MethodType.Constructor, [typeof(FollowerInfo)])]
    private static void FollowerBrain_Constructor(ref FollowerBrain __instance)
    {
        if (IsNothingNegativePresent()) return;
        if (!Plugin.NoNegativeTraits.Value) return;
        ProcessTraitReplacement(__instance);
    }

    private static bool IsExclusiveTrait(FollowerTrait.TraitType trait)
    {
        return FollowerTrait.ExclusiveTraits.ContainsKey(trait) || FollowerTrait.ExclusiveTraits.ContainsValue(trait);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(BiomeBaseManager), nameof(BiomeBaseManager.Start))]
    private static void BiomeBaseManager_Start()
    {
        if (IsNothingNegativePresent()) return;
        FollowerTraitBackups?.Clear();
        LoadBackupFromFile();
        GenerateAvailableTraits();
    }

    internal static void GenerateAvailableTraits()
    {
        AllTraits = InitializeAllTraits();
    }

    [Serializable]
    public class FollowerTraitBackup
    {
        public int ID;
        public string Name;
        public List<string> Traits;
    }
}