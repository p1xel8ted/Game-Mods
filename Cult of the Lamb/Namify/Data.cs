using System.Linq;

namespace Namify;

public static class Data
{
    internal const string NamifyDataPath = "namify_names.json";
    internal const string UserDataPath = "user_names.json";
    public static SortedSet<string> NamifyNames = [];
    public static SortedSet<string> UserNames = [];
    private readonly static COTLDataReadWriter<List<string>> NamifyNameReadWriter = new();
    private readonly static COTLDataReadWriter<List<string>> UserNameReadWriter = new();
    static Data()
    {
        NamifyNameReadWriter.OnReadCompleted += names =>
        {
            NamifyNames = new SortedSet<string>(names);
            Plugin.Log.LogInfo(NamifyNames.Count > 0 ? $"Loaded {NamifyNames.Count} Namify generated names." : "No saved names exist.");
            RemoveFollowerNames();
        };

        NamifyNameReadWriter.OnReadError += delegate
        {
            Plugin.Log.LogWarning("Failed to load saved names!");
        };

        NamifyNameReadWriter.OnWriteCompleted += delegate
        {
            Plugin.Log.LogInfo($"Saved {NamifyNames.Count} Namify generated names!");
        };

        NamifyNameReadWriter.OnWriteError += error =>
        {
            Plugin.Log.LogWarning($"There was an issue saving Namify generated names: {error.Message}");
        };
        
        
        
        UserNameReadWriter.OnReadCompleted += names =>
        {
            UserNames = new SortedSet<string>(names);
            Plugin.Log.LogInfo(UserNames.Count > 0 ? $"Loaded {UserNames.Count} user-generated names." : "No saved names exist.");
            RemoveFollowerNames();
        };

        UserNameReadWriter.OnReadError += delegate
        {
            Plugin.Log.LogWarning("Failed to load saved user-generated names!");
        };

        UserNameReadWriter.OnWriteCompleted += delegate
        {
            Plugin.Log.LogInfo($"Saved {UserNames.Count} user-generated names!");
        };

        UserNameReadWriter.OnWriteError += error =>
        {
            Plugin.Log.LogWarning($"There was an issue saving user-generated names: {error.Message}");
        };
    }

    private static void RemoveFollowerNames()
    {
        foreach (var name in NamifyNames)
        {
            Follower.Followers.RemoveAll(a => a.Brain.Info.Name == name);
        }
        
        foreach (var name in UserNames)
        {
            Follower.Followers.RemoveAll(a => a.Brain.Info.Name == name);
        }
    }

    internal static void LoadData()
    {
        NamifyNameReadWriter.Read(NamifyDataPath);
        UserNameReadWriter.Read(UserDataPath);
    }

    internal static void SaveData()
    {
        NamifyNameReadWriter.Write(NamifyNames.ToList(), NamifyDataPath, false);
        UserNameReadWriter.Write(UserNames.ToList(), UserDataPath, false);
    }

    internal static void GetNamifyNames(Action? onFail = null, Action? onComplete = null)
    {
        var primaryError = false;
        if (NamifyNames.Count > 0)
        {
            return;
        }

        GameManager.GetInstance().StartCoroutine(NamifyNamesGetRequest("https://randommer.io/api/Name?nameType=fullname&quantity=1000", true, req =>
        {
            if (req.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError or UnityWebRequest.Result.DataProcessingError)
            {
                Plugin.Log.LogError($"{req.error}: {req.downloadHandler.text}");
                NotificationCentre.Instance.PlayGenericNotification("There was an error retrieving names for Namify! Trying back-up source...");
                onFail?.Invoke();
                primaryError = true;
            }
            else
            {
                var nameText = JsonConvert.DeserializeObject<string[]>(req.downloadHandler.text);
                foreach (var name in nameText)
                {
                    NamifyNames.AddRange(name.Split());
                }

                SaveData();
                NotificationCentre.Instance.PlayGenericNotification("Names retrieved for Namify!");
                onComplete?.Invoke();
            }
        }));

        if (!primaryError) return;

        GameManager.GetInstance().StartCoroutine(GetNamifyNamesBackupRequest(onFail, onComplete));
    }

    private static IEnumerator GetNamifyNamesBackupRequest(Action? onFail = null, Action? onComplete = null)
    {
        for (var i = 0; i < 10; i++)
        {
            yield return GameManager.GetInstance().StartCoroutine(NamifyNamesGetRequest("https://namey.muffinlabs.com/name.json?count=10&with_surname=true&frequency=all", false, req =>
            {
                if (req.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError or UnityWebRequest.Result.DataProcessingError)
                {
                    Plugin.Log.LogError($"{req.error}: {req.downloadHandler.text}");
                    NotificationCentre.Instance.PlayGenericNotification("There was an error retrieving names for Namify from the backup source!");
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
                    NotificationCentre.Instance.PlayGenericNotification("Names retrieved for Namify from the backup source!");
                    onComplete?.Invoke();
                }
            }));
        }
    }

    private static IEnumerator NamifyNamesGetRequest(string endpoint, bool apiKey, Action<UnityWebRequest> callback)
    {
        using var request = UnityWebRequest.Get(endpoint);
        if (apiKey)
        {
            request.SetRequestHeader("X-Api-Key", Plugin.PersonalApiKey.Value);
        }

        yield return request.SendWebRequest();
        callback(request);
    }
}