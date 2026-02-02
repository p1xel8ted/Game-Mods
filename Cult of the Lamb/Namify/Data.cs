using Shared;

namespace Namify;

public static class Data
{
    internal const string NamifyDataPath = "namify_names.json";
    internal const string UserDataPath = "user_names.json";

    public static SortedSet<string> NamifyNames = [];
    public static SortedSet<string> UserNames = [];

    private static string SavesDirectory => Path.Combine(Application.persistentDataPath, "saves");
    internal static string NamifyNamesFilePath => Path.Combine(SavesDirectory, NamifyDataPath);
    internal static string UserNamesFilePath => Path.Combine(SavesDirectory, UserDataPath);

    private static void RemoveFollowerNames()
    {
        if (Follower.Followers == null) return;

        foreach (var name in NamifyNames)
        {
            Follower.Followers.RemoveAll(a => a?.Brain?.Info?.Name == name);
        }

        foreach (var name in UserNames)
        {
            Follower.Followers.RemoveAll(a => a?.Brain?.Info?.Name == name);
        }
    }

    internal static void LoadData()
    {
        Directory.CreateDirectory(SavesDirectory);

        // Load Namify names (try JSON first, then migrate from .mp if needed)
        NamifyNames = LoadNamesFromFile(NamifyNamesFilePath);
        if (NamifyNames.Count > 0)
        {
            Plugin.Log.LogInfo($"Loaded {NamifyNames.Count} Namify generated names.");
        }

        // Load User names
        UserNames = LoadNamesFromFile(UserNamesFilePath);
        if (UserNames.Count > 0)
        {
            Plugin.Log.LogInfo($"Loaded {UserNames.Count} user-generated names.");
        }

        RemoveFollowerNames();
    }

    private static SortedSet<string> LoadNamesFromFile(string jsonPath)
    {
        // Try loading .json file first
        if (File.Exists(jsonPath))
        {
            try
            {
                var json = File.ReadAllText(jsonPath);
                var names = JsonConvert.DeserializeObject<List<string>>(json);
                return names != null ? new SortedSet<string>(names) : [];
            }
            catch (Exception e)
            {
                Plugin.Log.LogWarning($"Failed to load {jsonPath}: {e.Message}");
            }
        }

        // Check for old .mp file (MessagePack format from game's previous save system)
        var mpPath = Path.ChangeExtension(jsonPath, ".mp");
        if (File.Exists(mpPath))
        {
            Plugin.Log.LogWarning($"Found old format file: {Path.GetFileName(mpPath)}");
            Plugin.Log.LogWarning("This file was created by the game's old save system and cannot be read.");
            Plugin.Log.LogWarning("Please delete it and the mod will regenerate names from the API.");
        }

        return [];
    }

    internal static void SaveData()
    {
        Directory.CreateDirectory(SavesDirectory);
        SaveNamesToFile(NamifyNamesFilePath, NamifyNames);
        SaveNamesToFile(UserNamesFilePath, UserNames);
    }

    private static void SaveNamesToFile(string path, SortedSet<string> names)
    {
        try
        {
            var json = JsonConvert.SerializeObject(names.ToList(), Formatting.Indented);
            File.WriteAllText(path, json);
            Plugin.Log.LogInfo($"Saved {names.Count} names to {Path.GetFileName(path)}");
        }
        catch (Exception e)
        {
            Plugin.Log.LogError($"Failed to save {path}: {e.Message}");
        }
    }

    internal static void GetNamifyNames(Action onFail = null, Action onComplete = null)
    {
        if (NamifyNames.Count > 0)
        {
            return;
        }

        var gameManager = GameManager.GetInstance();
        if (gameManager == null)
        {
            Plugin.Log.LogInfo("GameManager not ready, cannot fetch names");
            onFail?.Invoke();
            return;
        }

        gameManager.StartCoroutine(NamifyNamesGetRequest("https://randommer.io/api/Name?nameType=fullname&quantity=1000", true, req =>
        {
            if (req.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError or UnityWebRequest.Result.DataProcessingError)
            {
                Plugin.Log.LogError($"{req.error}: {req.downloadHandler.text}");
                NotificationCentre.Instance.PlayGenericNotification(Localization.ApiError);

                // Start backup request here, inside the error callback
                var gameManagerRetry = GameManager.GetInstance();
                if (gameManagerRetry != null)
                {
                    gameManagerRetry.StartCoroutine(GetNamifyNamesBackupRequest(onFail, onComplete));
                }
                else
                {
                    onFail?.Invoke();
                }
            }
            else
            {
                var nameText = JsonConvert.DeserializeObject<string[]>(req.downloadHandler.text);
                foreach (var name in nameText)
                {
                    NamifyNames.AddRange(name.Split());
                }

                SaveData();
                NotificationCentre.Instance.PlayGenericNotification(Localization.NamesRetrieved);
                onComplete?.Invoke();
            }
        }));
    }

    private static IEnumerator GetNamifyNamesBackupRequest(Action onFail = null, Action onComplete = null)
    {
        for (var i = 0; i < 10; i++)
        {
            yield return GameManager.GetInstance().StartCoroutine(NamifyNamesGetRequest("https://namey.muffinlabs.com/name.json?count=10&with_surname=true&frequency=all", false, req =>
            {
                if (req.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError or UnityWebRequest.Result.DataProcessingError)
                {
                    Plugin.Log.LogError($"{req.error}: {req.downloadHandler.text}");
                    NotificationCentre.Instance.PlayGenericNotification(Localization.ApiBackupError);
                    onFail?.Invoke();
                }
                else
                {
                    var nameText = JsonConvert.DeserializeObject<string[]>(req.downloadHandler.text);
                    foreach (var name in nameText)
                    {
                        NamifyNames.Add(name);
                    }

                    SaveData();
                    NotificationCentre.Instance.PlayGenericNotification(Localization.NamesRetrievedBackup);
                    onComplete?.Invoke();
                }
            }));
        }
    }

    private static IEnumerator NamifyNamesGetRequest(string endpoint, bool apiKey, Action<UnityWebRequest> callback)
    {
        var request = UnityWebRequest.Get(endpoint);
        try
        {
            if (apiKey)
            {
                request.SetRequestHeader("X-Api-Key", Plugin.PersonalApiKey.Value);
            }

            yield return request.SendWebRequest();
            callback(request);
        }
        finally
        {
            request?.Dispose();
        }
    }
}
