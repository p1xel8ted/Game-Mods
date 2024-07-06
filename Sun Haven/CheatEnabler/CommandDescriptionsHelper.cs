namespace CheatEnabler;

public static class CommandDescriptionsHelper
{

    private readonly static Dictionary<string, Dictionary<string, string>> DescriptionsMap = new();

    internal static Dictionary<string, string> GetLanguageDictionary(string langCode = "en")
    {
        var dict = DescriptionsMap.TryGetValue(langCode, out var dictionary) ? dictionary : null;
        if (dict == null)
        {
            Plugin.LOG.LogError($"Cannot find command descriptions for '{langCode}'");
        }
        return dict;
    }

    internal static void PopulateDictionaries()
    {
        var jsonDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "CheatEnablerLang");
        if (!Directory.Exists(jsonDir))
        {
            Plugin.LOG.LogError($"Cannot find directory at {jsonDir}");
            return;
        }
        var jsonFiles = Directory.GetFiles(jsonDir, "*.json");
        foreach (var filePath in jsonFiles)
        {
            var lang = Path.GetFileNameWithoutExtension(filePath);
            try
            {
                var jsonString = File.ReadAllText(filePath);
                var data = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString);
                DescriptionsMap[lang] = data;
                Plugin.LOG.LogInfo($"Loaded {data.Count} command descriptions for '{lang}'");
            }
            catch (IOException ex)
            {
                Plugin.LOG.LogError($"File reading error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Plugin.LOG.LogError($"JSON parsing error: {ex.Message}");
            }
        }
    }
}