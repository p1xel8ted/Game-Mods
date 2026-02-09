// Decompiled with JetBrains decompiler
// Type: Localization
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class Localization
{
  public static Localization.LoadFunction loadFunction;
  public static Localization.OnLocalizeNotification onLocalize;
  public static bool localizationHasBeenSet = false;
  public static string[] mLanguages = (string[]) null;
  public static Dictionary<string, string> mOldDictionary = new Dictionary<string, string>();
  public static Dictionary<string, string[]> mDictionary = new Dictionary<string, string[]>();
  public static Dictionary<string, string> mReplacement = new Dictionary<string, string>();
  public static int mLanguageIndex = -1;
  public static string mLanguage;
  public static bool mMerging = false;

  public static Dictionary<string, string[]> dictionary
  {
    get
    {
      if (!Localization.localizationHasBeenSet)
        Localization.LoadDictionary(PlayerPrefs.GetString("Language", "English"));
      return Localization.mDictionary;
    }
    set
    {
      Localization.localizationHasBeenSet = value != null;
      Localization.mDictionary = value;
    }
  }

  public static string[] knownLanguages
  {
    get
    {
      if (!Localization.localizationHasBeenSet)
        Localization.LoadDictionary(PlayerPrefs.GetString("Language", "English"));
      return Localization.mLanguages;
    }
  }

  public static string language
  {
    get
    {
      if (string.IsNullOrEmpty(Localization.mLanguage))
      {
        Localization.mLanguage = PlayerPrefs.GetString("Language", "English");
        Localization.LoadAndSelect(Localization.mLanguage);
      }
      return Localization.mLanguage;
    }
    set
    {
      if (!(Localization.mLanguage != value))
        return;
      Localization.mLanguage = value;
      Localization.LoadAndSelect(value);
    }
  }

  public static bool LoadDictionary(string value)
  {
    byte[] bytes = (byte[]) null;
    if (!Localization.localizationHasBeenSet)
    {
      if (Localization.loadFunction == null)
      {
        TextAsset textAsset = Resources.Load<TextAsset>(nameof (Localization));
        if ((UnityEngine.Object) textAsset != (UnityEngine.Object) null)
          bytes = textAsset.bytes;
      }
      else
        bytes = Localization.loadFunction(nameof (Localization));
      Localization.localizationHasBeenSet = true;
    }
    if (Localization.LoadCSV(bytes))
      return true;
    if (string.IsNullOrEmpty(value))
      value = Localization.mLanguage;
    if (string.IsNullOrEmpty(value))
      return false;
    if (Localization.loadFunction == null)
    {
      TextAsset textAsset = Resources.Load<TextAsset>(value);
      if ((UnityEngine.Object) textAsset != (UnityEngine.Object) null)
        bytes = textAsset.bytes;
    }
    else
      bytes = Localization.loadFunction(value);
    if (bytes == null)
      return false;
    Localization.Set(value, bytes);
    return true;
  }

  public static bool LoadAndSelect(string value)
  {
    if (!string.IsNullOrEmpty(value))
    {
      if (Localization.mDictionary.Count == 0 && !Localization.LoadDictionary(value))
        return false;
      if (Localization.SelectLanguage(value))
        return true;
    }
    if (Localization.mOldDictionary.Count > 0)
      return true;
    Localization.mOldDictionary.Clear();
    Localization.mDictionary.Clear();
    if (string.IsNullOrEmpty(value))
      PlayerPrefs.DeleteKey("Language");
    return false;
  }

  public static void Load(TextAsset asset)
  {
    ByteReader byteReader = new ByteReader(asset);
    Localization.Set(asset.name, byteReader.ReadDictionary());
  }

  public static void Set(string languageName, byte[] bytes)
  {
    ByteReader byteReader = new ByteReader(bytes);
    Localization.Set(languageName, byteReader.ReadDictionary());
  }

  public static void ReplaceKey(string key, string val)
  {
    if (!string.IsNullOrEmpty(val))
      Localization.mReplacement[key] = val;
    else
      Localization.mReplacement.Remove(key);
  }

  public static void ClearReplacements() => Localization.mReplacement.Clear();

  public static bool LoadCSV(TextAsset asset, bool merge = false)
  {
    return Localization.LoadCSV(asset.bytes, asset, merge);
  }

  public static bool LoadCSV(byte[] bytes, bool merge = false)
  {
    return Localization.LoadCSV(bytes, (TextAsset) null, merge);
  }

  public static bool HasLanguage(string languageName)
  {
    int index = 0;
    for (int length = Localization.mLanguages.Length; index < length; ++index)
    {
      if (Localization.mLanguages[index] == languageName)
        return true;
    }
    return false;
  }

  public static bool LoadCSV(byte[] bytes, TextAsset asset, bool merge = false)
  {
    if (bytes == null)
      return false;
    ByteReader byteReader = new ByteReader(bytes);
    BetterList<string> betterList = byteReader.ReadCSV();
    if (betterList.size < 2)
      return false;
    betterList.RemoveAt(0);
    string[] newLanguages = (string[]) null;
    if (string.IsNullOrEmpty(Localization.mLanguage))
      Localization.localizationHasBeenSet = false;
    if (!Localization.localizationHasBeenSet || !merge && !Localization.mMerging || Localization.mLanguages == null || Localization.mLanguages.Length == 0)
    {
      Localization.mDictionary.Clear();
      Localization.mLanguages = new string[betterList.size];
      if (!Localization.localizationHasBeenSet)
      {
        Localization.mLanguage = PlayerPrefs.GetString("Language", betterList[0]);
        Localization.localizationHasBeenSet = true;
      }
      for (int i = 0; i < betterList.size; ++i)
      {
        Localization.mLanguages[i] = betterList[i];
        if (Localization.mLanguages[i] == Localization.mLanguage)
          Localization.mLanguageIndex = i;
      }
    }
    else
    {
      newLanguages = new string[betterList.size];
      for (int i = 0; i < betterList.size; ++i)
        newLanguages[i] = betterList[i];
      for (int i = 0; i < betterList.size; ++i)
      {
        if (!Localization.HasLanguage(betterList[i]))
        {
          int newSize = Localization.mLanguages.Length + 1;
          Array.Resize<string>(ref Localization.mLanguages, newSize);
          Localization.mLanguages[newSize - 1] = betterList[i];
          Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>();
          foreach (KeyValuePair<string, string[]> m in Localization.mDictionary)
          {
            string[] array = m.Value;
            Array.Resize<string>(ref array, newSize);
            array[newSize - 1] = array[0];
            dictionary.Add(m.Key, array);
          }
          Localization.mDictionary = dictionary;
        }
      }
    }
    Dictionary<string, int> languageIndices = new Dictionary<string, int>();
    for (int index = 0; index < Localization.mLanguages.Length; ++index)
      languageIndices.Add(Localization.mLanguages[index], index);
    while (true)
    {
      BetterList<string> newValues;
      do
      {
        newValues = byteReader.ReadCSV();
        if (newValues == null || newValues.size == 0)
          goto label_33;
      }
      while (string.IsNullOrEmpty(newValues[0]));
      Localization.AddCSV(newValues, newLanguages, languageIndices);
    }
label_33:
    if (!Localization.mMerging && Localization.onLocalize != null)
    {
      Localization.mMerging = true;
      Localization.OnLocalizeNotification onLocalize = Localization.onLocalize;
      Localization.onLocalize = (Localization.OnLocalizeNotification) null;
      onLocalize();
      Localization.onLocalize = onLocalize;
      Localization.mMerging = false;
    }
    return true;
  }

  public static void AddCSV(
    BetterList<string> newValues,
    string[] newLanguages,
    Dictionary<string, int> languageIndices)
  {
    if (newValues.size < 2)
      return;
    string newValue = newValues[0];
    if (string.IsNullOrEmpty(newValue))
      return;
    string[] strings = Localization.ExtractStrings(newValues, newLanguages, languageIndices);
    if (Localization.mDictionary.ContainsKey(newValue))
    {
      Localization.mDictionary[newValue] = strings;
      if (newLanguages != null)
        return;
      Debug.LogWarning((object) $"Localization key '{newValue}' is already present");
    }
    else
    {
      try
      {
        Localization.mDictionary.Add(newValue, strings);
      }
      catch (Exception ex)
      {
        Debug.LogError((object) $"Unable to add '{newValue}' to the Localization dictionary.\n{ex.Message}");
      }
    }
  }

  public static string[] ExtractStrings(
    BetterList<string> added,
    string[] newLanguages,
    Dictionary<string, int> languageIndices)
  {
    if (newLanguages == null)
    {
      string[] strings = new string[Localization.mLanguages.Length];
      int i = 1;
      for (int index = Mathf.Min(added.size, strings.Length + 1); i < index; ++i)
        strings[i - 1] = added[i];
      return strings;
    }
    string key = added[0];
    string[] strings1;
    if (!Localization.mDictionary.TryGetValue(key, out strings1))
      strings1 = new string[Localization.mLanguages.Length];
    int index1 = 0;
    for (int length = newLanguages.Length; index1 < length; ++index1)
    {
      string newLanguage = newLanguages[index1];
      int languageIndex = languageIndices[newLanguage];
      strings1[languageIndex] = added[index1 + 1];
    }
    return strings1;
  }

  public static bool SelectLanguage(string language)
  {
    Localization.mLanguageIndex = -1;
    if (Localization.mDictionary.Count == 0)
      return false;
    int index = 0;
    for (int length = Localization.mLanguages.Length; index < length; ++index)
    {
      if (Localization.mLanguages[index] == language)
      {
        Localization.mOldDictionary.Clear();
        Localization.mLanguageIndex = index;
        Localization.mLanguage = language;
        PlayerPrefs.SetString("Language", Localization.mLanguage);
        if (Localization.onLocalize != null)
          Localization.onLocalize();
        UIRoot.Broadcast("OnLocalize");
        return true;
      }
    }
    return false;
  }

  public static void Set(string languageName, Dictionary<string, string> dictionary)
  {
    Localization.mLanguage = languageName;
    PlayerPrefs.SetString("Language", Localization.mLanguage);
    Localization.mOldDictionary = dictionary;
    Localization.localizationHasBeenSet = true;
    Localization.mLanguageIndex = -1;
    Localization.mLanguages = new string[1]{ languageName };
    if (Localization.onLocalize != null)
      Localization.onLocalize();
    UIRoot.Broadcast("OnLocalize");
  }

  public static void Set(string key, string value)
  {
    if (Localization.mOldDictionary.ContainsKey(key))
      Localization.mOldDictionary[key] = value;
    else
      Localization.mOldDictionary.Add(key, value);
  }

  public static string Get(string key, bool warnIfMissing = true)
  {
    if (string.IsNullOrEmpty(key))
      return (string) null;
    if (!Localization.localizationHasBeenSet)
      Localization.LoadDictionary(PlayerPrefs.GetString("Language", "English"));
    if (Localization.mLanguages == null)
    {
      Debug.LogError((object) "No localization data present");
      return (string) null;
    }
    string language = Localization.language;
    if (Localization.mLanguageIndex == -1)
    {
      for (int index = 0; index < Localization.mLanguages.Length; ++index)
      {
        if (Localization.mLanguages[index] == language)
        {
          Localization.mLanguageIndex = index;
          break;
        }
      }
    }
    if (Localization.mLanguageIndex == -1)
    {
      Localization.mLanguageIndex = 0;
      Localization.mLanguage = Localization.mLanguages[0];
      Debug.LogWarning((object) ("Language not found: " + language));
    }
    string str1;
    string[] strArray;
    switch (UICamera.currentScheme)
    {
      case UICamera.ControlScheme.Touch:
        string key1 = key + " Mobile";
        if (Localization.mReplacement.TryGetValue(key1, out str1))
          return str1;
        if (Localization.mLanguageIndex != -1 && Localization.mDictionary.TryGetValue(key1, out strArray) && Localization.mLanguageIndex < strArray.Length)
          return strArray[Localization.mLanguageIndex];
        if (Localization.mOldDictionary.TryGetValue(key1, out str1))
          return str1;
        break;
      case UICamera.ControlScheme.Controller:
        string key2 = key + " Controller";
        if (Localization.mReplacement.TryGetValue(key2, out str1))
          return str1;
        if (Localization.mLanguageIndex != -1 && Localization.mDictionary.TryGetValue(key2, out strArray) && Localization.mLanguageIndex < strArray.Length)
          return strArray[Localization.mLanguageIndex];
        if (Localization.mOldDictionary.TryGetValue(key2, out str1))
          return str1;
        break;
    }
    if (Localization.mReplacement.TryGetValue(key, out str1))
      return str1;
    if (Localization.mLanguageIndex != -1 && Localization.mDictionary.TryGetValue(key, out strArray))
    {
      if (Localization.mLanguageIndex >= strArray.Length)
        return strArray[0];
      string str2 = strArray[Localization.mLanguageIndex];
      if (string.IsNullOrEmpty(str2))
        str2 = strArray[0];
      return str2;
    }
    return Localization.mOldDictionary.TryGetValue(key, out str1) ? str1 : key;
  }

  public static string Format(string key, params object[] parameters)
  {
    return string.Format(Localization.Get(key), parameters);
  }

  [Obsolete("Localization is now always active. You no longer need to check this property.")]
  public static bool isActive => true;

  [Obsolete("Use Localization.Get instead")]
  public static string Localize(string key) => Localization.Get(key);

  public static bool Exists(string key)
  {
    if (!Localization.localizationHasBeenSet)
      Localization.language = PlayerPrefs.GetString("Language", "English");
    return Localization.mDictionary.ContainsKey(key) || Localization.mOldDictionary.ContainsKey(key);
  }

  public static void Set(string language, string key, string text)
  {
    string[] strArray1 = Localization.knownLanguages;
    if (strArray1 == null)
    {
      Localization.mLanguages = new string[1]{ language };
      strArray1 = Localization.mLanguages;
    }
    int index = 0;
    for (int length = strArray1.Length; index < length; ++index)
    {
      if (strArray1[index] == language)
      {
        string[] strArray2;
        if (!Localization.mDictionary.TryGetValue(key, out strArray2))
        {
          strArray2 = new string[strArray1.Length];
          Localization.mDictionary[key] = strArray2;
          strArray2[0] = text;
        }
        strArray2[index] = text;
        return;
      }
    }
    int newSize = Localization.mLanguages.Length + 1;
    Array.Resize<string>(ref Localization.mLanguages, newSize);
    Localization.mLanguages[newSize - 1] = language;
    Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>();
    foreach (KeyValuePair<string, string[]> m in Localization.mDictionary)
    {
      string[] array = m.Value;
      Array.Resize<string>(ref array, newSize);
      array[newSize - 1] = array[0];
      dictionary.Add(m.Key, array);
    }
    Localization.mDictionary = dictionary;
    string[] strArray3;
    if (!Localization.mDictionary.TryGetValue(key, out strArray3))
    {
      strArray3 = new string[strArray1.Length];
      Localization.mDictionary[key] = strArray3;
      strArray3[0] = text;
    }
    strArray3[newSize - 1] = text;
  }

  public delegate byte[] LoadFunction(string path);

  public delegate void OnLocalizeNotification();
}
