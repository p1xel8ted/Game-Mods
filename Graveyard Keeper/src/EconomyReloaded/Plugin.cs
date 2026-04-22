namespace EconomyReloaded;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    private const string EconomySection = "── Economy ──";
    private const string UpdatesSection = "── Updates ──";

    private static readonly Dictionary<string, string> SectionRenames = new()
    {
        ["01. Economy"] = EconomySection,
    };

    internal static ManualLogSource Log { get; private set; }

    internal static ConfigEntry<bool> DynamicBuyPricing { get; private set; }
    internal static ConfigEntry<float> BuyPriceMultiplier { get; private set; }
    internal static ConfigEntry<bool> DynamicSellPricing { get; private set; }
    internal static ConfigEntry<float> SellPriceMultiplier { get; private set; }
    internal static ConfigEntry<bool> CheckForUpdates { get; private set; }

    private void Awake()
    {
        Log = Logger;
        var legacy = MigrateConfig();
        InitConfiguration();
        ApplyLegacyMigration(legacy);
        UpdateChecker.Register(Info, CheckForUpdates);
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), MyPluginInfo.PLUGIN_GUID);
    }

    // Rewrites legacy section headers to the "── Name ──" style and strips out the
    // pre-1.4 "Inflation" / "Deflation" keys so the new settings bind cleanly.
    // Returns the parsed legacy values so ApplyLegacyMigration can port them across.
    private LegacyConfigValues MigrateConfig()
    {
        var result = new LegacyConfigValues();
        var path = Config.ConfigFilePath;
        if (!File.Exists(path)) return result;

        string content;
        try { content = File.ReadAllText(path); }
        catch (Exception ex) { Log.LogWarning($"[Migration] Could not read {path}: {ex.Message}"); return result; }

        var modified = false;

        foreach (var kv in SectionRenames)
        {
            var oldHeader = $"[{kv.Key}]";
            var newHeader = $"[{kv.Value}]";
            if (!content.Contains(oldHeader)) continue;
            content = content.Replace(oldHeader, newHeader);
            modified = true;
        }

        result.Inflation = ExtractAndStripBoolEntry(ref content, "Inflation", ref modified);
        result.Deflation = ExtractAndStripBoolEntry(ref content, "Deflation", ref modified);

        if (!modified) return result;

        try { File.WriteAllText(path, content); }
        catch (Exception ex) { Log.LogWarning($"[Migration] Could not write {path}: {ex.Message}"); return result; }

        Log.LogInfo("[Migration] Legacy Economy Reloaded config format updated to the new settings layout.");
        Config.Reload();
        return result;
    }

    // Scans the INI content line-by-line, captures the boolean value of the target key
    // if present, and removes both the key line and any preceding comment block that
    // BepInEx auto-wrote for it.
    private static bool? ExtractAndStripBoolEntry(ref string content, string key, ref bool modified)
    {
        var normalised = content.Replace("\r\n", "\n");
        var lines = normalised.Split('\n');
        var kept = new List<string>(lines.Length);
        var commentStart = -1;
        bool? extracted = null;

        foreach (var line in lines)
        {
            var trimmed = line.TrimStart();

            if (trimmed.Length == 0 || trimmed.StartsWith("["))
            {
                commentStart = -1;
                kept.Add(line);
                continue;
            }

            if (trimmed.StartsWith("#"))
            {
                if (commentStart == -1) commentStart = kept.Count;
                kept.Add(line);
                continue;
            }

            if (IsKeyAssignment(trimmed, key))
            {
                var eq = trimmed.IndexOf('=');
                if (eq > 0)
                {
                    var value = trimmed.Substring(eq + 1).Trim();
                    if (bool.TryParse(value, out var parsed)) extracted = parsed;
                }
                if (commentStart >= 0)
                {
                    kept.RemoveRange(commentStart, kept.Count - commentStart);
                }
                commentStart = -1;
                modified = true;
                continue;
            }

            commentStart = -1;
            kept.Add(line);
        }

        if (extracted.HasValue)
        {
            content = string.Join("\n", kept);
        }
        return extracted;
    }

    private static bool IsKeyAssignment(string trimmedLine, string key)
    {
        if (!trimmedLine.StartsWith(key)) return false;
        var rest = trimmedLine.Substring(key.Length).TrimStart();
        return rest.StartsWith("=");
    }

    private void InitConfiguration()
    {
        DynamicBuyPricing = Config.Bind(EconomySection, "Dynamic Buy Pricing", true,
            new ConfigDescription(
                "When on, buy prices rise as shops run low on an item (the base game's inflation). When off, every purchase is at the flat base price no matter how much the shop has in stock.",
                null,
                new ConfigurationManagerAttributes { Order = 4 }));

        BuyPriceMultiplier = Config.Bind(EconomySection, "Buy Price Multiplier", 1.0f,
            new ConfigDescription(
                "Final multiplier applied to every buy price. 1.0 is the base rate, 0.5 is half price, 2.0 is double. Works together with the Dynamic Buy Pricing setting above.",
                new AcceptableValueRange<float>(0.1f, 5.0f),
                new ConfigurationManagerAttributes { Order = 3 }));

        DynamicSellPricing = Config.Bind(EconomySection, "Dynamic Sell Pricing", true,
            new ConfigDescription(
                "When on, sell prices drop as shops fill up on an item (the base game's deflation). When off, every sale pays the flat sell price no matter how many the shop already has.",
                null,
                new ConfigurationManagerAttributes { Order = 2 }));

        SellPriceMultiplier = Config.Bind(EconomySection, "Sell Price Multiplier", 0.75f,
            new ConfigDescription(
                "Final multiplier applied to every sell price. 0.75 matches the base game (shops pay three-quarters of the base price). Raise it for easier income, lower it for a tighter economy. Works together with the Dynamic Sell Pricing setting above.",
                new AcceptableValueRange<float>(0.1f, 5.0f),
                new ConfigurationManagerAttributes { Order = 1 }));

        CheckForUpdates = Config.Bind(UpdatesSection, "Check for Updates", true, new ConfigDescription(
            "Show a notice on the main menu when a newer version of this mod is available on NexusMods. Click the notice to open the mod's page.",
            null,
            new ConfigurationManagerAttributes { Order = 0 }));
    }

    // Carries over values from the pre-1.4 two-boolean config so an existing install
    // keeps behaving the same way after the upgrade.
    private void ApplyLegacyMigration(LegacyConfigValues legacy)
    {
        var changed = false;

        if (legacy.Inflation.HasValue)
        {
            DynamicBuyPricing.Value = legacy.Inflation.Value;
            changed = true;
        }

        if (legacy.Deflation.HasValue)
        {
            // Old "Deflation = true"  = vanilla dynamic sell with the 0.75 markdown
            // Old "Deflation = false" = flat sell at 1.0x base (no markdown, no scaling)
            DynamicSellPricing.Value = legacy.Deflation.Value;
            SellPriceMultiplier.Value = legacy.Deflation.Value ? 0.75f : 1.0f;
            changed = true;
        }

        if (!changed) return;

        Log.LogInfo("[Migration] Carried over your previous Inflation/Deflation choices to the new settings.");
        Config.Save();
    }

    private sealed class LegacyConfigValues
    {
        public bool? Inflation;
        public bool? Deflation;
    }
}
