// Decompiled with JetBrains decompiler
// Type: I2.Loc.I2BasePersistentStorage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEngine;

#nullable disable
namespace I2.Loc;

public abstract class I2BasePersistentStorage
{
  public virtual void SetSetting_String(string key, string value)
  {
    try
    {
      int length = value.Length;
      int a = 8000;
      if (length <= a)
      {
        PlayerPrefs.SetString(key, value);
      }
      else
      {
        int num = Mathf.CeilToInt((float) length / (float) a);
        for (int index = 0; index < num; ++index)
        {
          int startIndex = a * index;
          PlayerPrefs.SetString($"[I2split]{index}{key}", value.Substring(startIndex, Mathf.Min(a, length - startIndex)));
        }
        PlayerPrefs.SetString(key, "[$I2#@div$]" + num.ToString());
      }
    }
    catch (Exception ex)
    {
      Debug.LogError((object) ("Error saving PlayerPrefs " + key));
    }
  }

  public virtual string GetSetting_String(string key, string defaultValue)
  {
    try
    {
      string settingString = PlayerPrefs.GetString(key, defaultValue);
      if (!string.IsNullOrEmpty(settingString) && settingString.StartsWith("[I2split]", StringComparison.Ordinal))
      {
        int num = int.Parse(settingString.Substring("[I2split]".Length), (IFormatProvider) CultureInfo.InvariantCulture);
        settingString = "";
        for (int index = 0; index < num; ++index)
          settingString += PlayerPrefs.GetString($"[I2split]{index}{key}", "");
      }
      return settingString;
    }
    catch (Exception ex)
    {
      Debug.LogError((object) ("Error loading PlayerPrefs " + key));
      return defaultValue;
    }
  }

  public virtual void DeleteSetting(string key)
  {
    try
    {
      string str = PlayerPrefs.GetString(key, (string) null);
      if (!string.IsNullOrEmpty(str) && str.StartsWith("[I2split]", StringComparison.Ordinal))
      {
        int num = int.Parse(str.Substring("[I2split]".Length), (IFormatProvider) CultureInfo.InvariantCulture);
        for (int index = 0; index < num; ++index)
          PlayerPrefs.DeleteKey($"[I2split]{index}{key}");
      }
      PlayerPrefs.DeleteKey(key);
    }
    catch (Exception ex)
    {
      Debug.LogError((object) ("Error deleting PlayerPrefs " + key));
    }
  }

  public virtual void ForceSaveSettings() => PlayerPrefs.Save();

  public virtual bool HasSetting(string key) => PlayerPrefs.HasKey(key);

  public virtual bool CanAccessFiles() => true;

  public string UpdateFilename(PersistentStorage.eFileType fileType, string fileName)
  {
    switch (fileType)
    {
      case PersistentStorage.eFileType.Persistent:
        fileName = $"{Application.persistentDataPath}/{fileName}";
        break;
      case PersistentStorage.eFileType.Temporal:
        fileName = $"{Application.temporaryCachePath}/{fileName}";
        break;
      case PersistentStorage.eFileType.Streaming:
        fileName = $"{Application.streamingAssetsPath}/{fileName}";
        break;
    }
    return fileName;
  }

  public virtual bool SaveFile(
    PersistentStorage.eFileType fileType,
    string fileName,
    string data,
    bool logExceptions = true)
  {
    if (!this.CanAccessFiles())
      return false;
    try
    {
      fileName = this.UpdateFilename(fileType, fileName);
      File.WriteAllText(fileName, data, Encoding.UTF8);
      return true;
    }
    catch (Exception ex)
    {
      if (logExceptions)
        Debug.LogError((object) $"Error saving file '{fileName}'\n{ex?.ToString()}");
      return false;
    }
  }

  public virtual string LoadFile(
    PersistentStorage.eFileType fileType,
    string fileName,
    bool logExceptions = true)
  {
    if (!this.CanAccessFiles())
      return (string) null;
    try
    {
      fileName = this.UpdateFilename(fileType, fileName);
      return File.ReadAllText(fileName, Encoding.UTF8);
    }
    catch (Exception ex)
    {
      if (logExceptions)
        Debug.LogError((object) $"Error loading file '{fileName}'\n{ex?.ToString()}");
      return (string) null;
    }
  }

  public virtual bool DeleteFile(
    PersistentStorage.eFileType fileType,
    string fileName,
    bool logExceptions = true)
  {
    if (!this.CanAccessFiles())
      return false;
    try
    {
      fileName = this.UpdateFilename(fileType, fileName);
      File.Delete(fileName);
      return true;
    }
    catch (Exception ex)
    {
      if (logExceptions)
        Debug.LogError((object) $"Error deleting file '{fileName}'\n{ex?.ToString()}");
      return false;
    }
  }

  public virtual bool HasFile(
    PersistentStorage.eFileType fileType,
    string fileName,
    bool logExceptions = true)
  {
    if (!this.CanAccessFiles())
      return false;
    try
    {
      fileName = this.UpdateFilename(fileType, fileName);
      return File.Exists(fileName);
    }
    catch (Exception ex)
    {
      if (logExceptions)
        Debug.LogError((object) $"Error requesting file '{fileName}'\n{ex?.ToString()}");
      return false;
    }
  }
}
