// Decompiled with JetBrains decompiler
// Type: Unify.PlayerPrefs
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Unify;

internal class PlayerPrefs
{
  private const string SAVE_FILE = "Prefs.dict";

  public static void DeleteAll() => SaveData.Delete("Prefs.dict");

  public static void DeleteKey(string key)
  {
    Debug.LogError((object) "DeleteKey is NOT IMPLEMENETED");
  }

  public static float GetFloat(string key) => SaveData.GetFloat("Prefs.dict", key);

  public static float GetFloat(string key, float defaultValue = 0.5f)
  {
    return PlayerPrefs.HasKey(key) ? PlayerPrefs.GetFloat(key) : defaultValue;
  }

  public static int GetInt(string key) => SaveData.GetInt("Prefs.dict", key);

  public static int GetInt(string key, int defaultValue = 0)
  {
    return PlayerPrefs.HasKey(key) ? PlayerPrefs.GetInt(key) : defaultValue;
  }

  public static string GetString(string key) => SaveData.GetString("Prefs.dict", key);

  public static string GetString(string key, string defaultValue = "\"\"")
  {
    return PlayerPrefs.HasKey(key) ? PlayerPrefs.GetString(key) : defaultValue;
  }

  public static bool HasKey(string key) => SaveData.KeyExists("Prefs.dict", key);

  public static void Save()
  {
  }

  public static void SetFloat(string key, float value)
  {
    SaveData.Put<float>("Prefs.dict", key, value);
  }

  public static void SetInt(string key, int value) => SaveData.Put<int>("Prefs.dict", key, value);

  public static void SetString(string key, string value)
  {
    SaveData.Put<string>("Prefs.dict", key, value);
  }
}
