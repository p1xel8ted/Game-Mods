// Decompiled with JetBrains decompiler
// Type: DarkTonic.MasterAudio.FilePlayerPrefs
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections;
using System.Globalization;
using System.IO;
using UnityEngine;

#nullable disable
namespace DarkTonic.MasterAudio;

public static class FilePlayerPrefs
{
  public static Hashtable PlayerPrefsHashtable = new Hashtable();
  public static bool _hashTableChanged;
  public static string _serializedOutput = "";
  public static string SerializedInput = "";
  public const string ParametersSeperator = ";";
  public const string KeyValueSeperator = ":";
  public static string FileName = Application.persistentDataPath + "/MAPlayerPrefs.txt";

  static FilePlayerPrefs()
  {
    if (!File.Exists(FilePlayerPrefs.FileName))
      return;
    StreamReader streamReader = new StreamReader(FilePlayerPrefs.FileName);
    FilePlayerPrefs.SerializedInput = streamReader.ReadLine();
    FilePlayerPrefs.Deserialize();
    streamReader.Close();
  }

  public static bool HasKey(string key)
  {
    return FilePlayerPrefs.PlayerPrefsHashtable.ContainsKey((object) key);
  }

  public static void SetString(string key, string value)
  {
    if (!FilePlayerPrefs.PlayerPrefsHashtable.ContainsKey((object) key))
      FilePlayerPrefs.PlayerPrefsHashtable.Add((object) key, (object) value);
    else
      FilePlayerPrefs.PlayerPrefsHashtable[(object) key] = (object) value;
    FilePlayerPrefs._hashTableChanged = true;
  }

  public static void SetInt(string key, int value)
  {
    if (!FilePlayerPrefs.PlayerPrefsHashtable.ContainsKey((object) key))
      FilePlayerPrefs.PlayerPrefsHashtable.Add((object) key, (object) value);
    else
      FilePlayerPrefs.PlayerPrefsHashtable[(object) key] = (object) value;
    FilePlayerPrefs._hashTableChanged = true;
  }

  public static void SetFloat(string key, float value)
  {
    if (!FilePlayerPrefs.PlayerPrefsHashtable.ContainsKey((object) key))
      FilePlayerPrefs.PlayerPrefsHashtable.Add((object) key, (object) value);
    else
      FilePlayerPrefs.PlayerPrefsHashtable[(object) key] = (object) value;
    FilePlayerPrefs._hashTableChanged = true;
  }

  public static void SetBool(string key, bool value)
  {
    if (!FilePlayerPrefs.PlayerPrefsHashtable.ContainsKey((object) key))
      FilePlayerPrefs.PlayerPrefsHashtable.Add((object) key, (object) value);
    else
      FilePlayerPrefs.PlayerPrefsHashtable[(object) key] = (object) value;
    FilePlayerPrefs._hashTableChanged = true;
  }

  public static string GetString(string key)
  {
    return FilePlayerPrefs.PlayerPrefsHashtable.ContainsKey((object) key) ? FilePlayerPrefs.PlayerPrefsHashtable[(object) key].ToString() : (string) null;
  }

  public static string GetString(string key, string defaultValue)
  {
    if (FilePlayerPrefs.PlayerPrefsHashtable.ContainsKey((object) key))
      return FilePlayerPrefs.PlayerPrefsHashtable[(object) key].ToString();
    FilePlayerPrefs.PlayerPrefsHashtable.Add((object) key, (object) defaultValue);
    FilePlayerPrefs._hashTableChanged = true;
    return defaultValue;
  }

  public static int GetInt(string key)
  {
    if (!FilePlayerPrefs.PlayerPrefsHashtable.ContainsKey((object) key))
      return 0;
    object obj = FilePlayerPrefs.PlayerPrefsHashtable[(object) key];
    if (obj is int num1)
      return num1;
    int num2 = int.Parse(obj.ToString());
    FilePlayerPrefs.PlayerPrefsHashtable[(object) key] = (object) num2;
    return num2;
  }

  public static int GetInt(string key, int defaultValue)
  {
    if (FilePlayerPrefs.PlayerPrefsHashtable.ContainsKey((object) key))
      return (int) FilePlayerPrefs.PlayerPrefsHashtable[(object) key];
    FilePlayerPrefs.PlayerPrefsHashtable.Add((object) key, (object) defaultValue);
    FilePlayerPrefs._hashTableChanged = true;
    return defaultValue;
  }

  public static float GetFloat(string key)
  {
    if (!FilePlayerPrefs.PlayerPrefsHashtable.ContainsKey((object) key))
      return 0.0f;
    object obj = FilePlayerPrefs.PlayerPrefsHashtable[(object) key];
    if (obj is float num1)
      return num1;
    float num2 = float.Parse(obj.ToString(), (IFormatProvider) CultureInfo.InvariantCulture);
    FilePlayerPrefs.PlayerPrefsHashtable[(object) key] = (object) num2;
    return num2;
  }

  public static float GetFloat(string key, float defaultValue)
  {
    if (FilePlayerPrefs.PlayerPrefsHashtable.ContainsKey((object) key))
      return (float) FilePlayerPrefs.PlayerPrefsHashtable[(object) key];
    FilePlayerPrefs.PlayerPrefsHashtable.Add((object) key, (object) defaultValue);
    FilePlayerPrefs._hashTableChanged = true;
    return defaultValue;
  }

  public static bool GetBool(string key)
  {
    return FilePlayerPrefs.PlayerPrefsHashtable.ContainsKey((object) key) && (bool) FilePlayerPrefs.PlayerPrefsHashtable[(object) key];
  }

  public static bool GetBool(string key, bool defaultValue)
  {
    if (FilePlayerPrefs.PlayerPrefsHashtable.ContainsKey((object) key))
      return (bool) FilePlayerPrefs.PlayerPrefsHashtable[(object) key];
    FilePlayerPrefs.PlayerPrefsHashtable.Add((object) key, (object) defaultValue);
    FilePlayerPrefs._hashTableChanged = true;
    return defaultValue;
  }

  public static void DeleteKey(string key)
  {
    FilePlayerPrefs.PlayerPrefsHashtable.Remove((object) key);
  }

  public static void DeleteAll() => FilePlayerPrefs.PlayerPrefsHashtable.Clear();

  public static void Flush()
  {
    if (!FilePlayerPrefs._hashTableChanged)
      return;
    FilePlayerPrefs.Serialize();
    StreamWriter text = File.CreateText(FilePlayerPrefs.FileName);
    if (text == null)
      Debug.LogWarning((object) ("PlayerPrefs::Flush() opening file for writing failed: " + FilePlayerPrefs.FileName));
    text.WriteLine(FilePlayerPrefs._serializedOutput);
    text.Close();
    FilePlayerPrefs._serializedOutput = "";
  }

  public static void Serialize()
  {
    IDictionaryEnumerator enumerator = FilePlayerPrefs.PlayerPrefsHashtable.GetEnumerator();
    while (enumerator.MoveNext())
    {
      if (FilePlayerPrefs._serializedOutput != "")
        FilePlayerPrefs._serializedOutput += " ; ";
      FilePlayerPrefs._serializedOutput = $"{FilePlayerPrefs._serializedOutput}{FilePlayerPrefs.EscapeNonSeperators(enumerator.Key.ToString())} : {FilePlayerPrefs.EscapeNonSeperators(enumerator.Value.ToString())} : {enumerator.Value.GetType()?.ToString()}";
    }
  }

  public static void Deserialize()
  {
    string serializedInput = FilePlayerPrefs.SerializedInput;
    string[] separator1 = new string[1]{ " ; " };
    foreach (string str in serializedInput.Split(separator1, StringSplitOptions.None))
    {
      string[] separator2 = new string[1]{ " : " };
      string[] strArray = str.Split(separator2, StringSplitOptions.None);
      FilePlayerPrefs.PlayerPrefsHashtable.Add((object) FilePlayerPrefs.DeEscapeNonSeperators(strArray[0]), FilePlayerPrefs.GetTypeValue(strArray[2], FilePlayerPrefs.DeEscapeNonSeperators(strArray[1])));
      if (strArray.Length > 3)
        Debug.LogWarning((object) $"PlayerPrefs::Deserialize() parameterContent has {strArray.Length.ToString()} elements");
    }
  }

  public static string EscapeNonSeperators(string inputToEscape)
  {
    inputToEscape = inputToEscape.Replace(":", "\\:");
    inputToEscape = inputToEscape.Replace(";", "\\;");
    return inputToEscape;
  }

  public static string DeEscapeNonSeperators(string inputToDeEscape)
  {
    inputToDeEscape = inputToDeEscape.Replace("\\:", ":");
    inputToDeEscape = inputToDeEscape.Replace("\\;", ";");
    return inputToDeEscape;
  }

  public static object GetTypeValue(string typeName, string value)
  {
    switch (typeName)
    {
      case "System.String":
        return (object) value;
      case "System.Int32":
        return (object) Convert.ToInt32(value);
      case "System.Boolean":
        return (object) Convert.ToBoolean(value);
      case "System.Single":
        return (object) Convert.ToSingle(value);
      default:
        Debug.Log((object) ("Unsupported type: " + typeName));
        return (object) null;
    }
  }
}
